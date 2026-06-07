namespace Data.Repositories
{
    using Microsoft.Data.SqlClient;

    public abstract class RepositoryBase<TEntity>
    {
        private readonly string connectionString = "Server=localhost;Database=controle_financeiro;User Id=sa;Password=admin;TrustServerCertificate=True;";

        public List<TEntity> List(string sql)
        {
            return this.ExecuteQuery(sql, this.EntityResultListMapper) ?? [];
        }

        public virtual TEntity? Get(string sql)
        {
            return this.ExecuteQuery(sql, this.EntityResultMapper);
        }

        public void Persist(string sql, Dictionary<string, object> parameters)
        {
            using SqlConnection connection = new (this.connectionString);
            connection.Open();

            using SqlCommand command = new (sql, connection);
            EntityPersistenceMapper(command, parameters);
            command.ExecuteNonQuery();
        }

        internal abstract TEntity EntityResultMapper(SqlDataReader reader);

        internal List<TEntity> EntityResultListMapper(SqlDataReader reader)
        {
            List<TEntity> result = [];

            while (reader.Read())
            {
                result.Add(this.EntityResultMapper(reader));
            }

            return result;
        }

        internal static void EntityPersistenceMapper(SqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }

        private TMappedData? ExecuteQuery<TMappedData>(string sql, Func<SqlDataReader, TMappedData> dataMapper)
        {
            using SqlConnection connection = new (this.connectionString);
            connection.Open();

            using SqlCommand command = new (sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            bool hasData = reader.Read();
            return hasData ? dataMapper(reader) : default;
        }
    }
}
