namespace Data.Repositories
{
    using Microsoft.Data.SqlClient;

    public abstract class RepositoryBase<TEntity>
    {
        private readonly string connectionString = "Server=localhost;Database={DATABASE};User Id={USER};Password={PASSWORD};TrustServerCertificate=True;";

        public List<TEntity> List(string sql, Dictionary<string, object>? parameters = null)
        {
            return this.ExecuteQuery(sql, this.EntityResultListMapper, parameters) ?? [];
        }

        public virtual TEntity? Get(string sql, Dictionary<string, object> parameters)
        {
            return this.ExecuteQuery(sql, this.EntityResultMapper, parameters);
        }

        public void Persist(string sql, Dictionary<string, object> parameters)
        {
            using SqlConnection connection = new (this.connectionString);
            connection.Open();

            using SqlCommand command = new (sql, connection);
            QueryParametersMapper(command, parameters);
            command.ExecuteNonQuery();
        }

        internal static void QueryParametersMapper(SqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
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

        private TMappedData? ExecuteQuery<TMappedData>(string sql, Func<SqlDataReader, TMappedData> dataMapper, Dictionary<string, object>? parameters)
        {
            using SqlConnection connection = new (this.connectionString);
            connection.Open();

            using SqlCommand command = new (sql, connection);

            if (parameters != null)
            {
                QueryParametersMapper(command, parameters);
            }

            using SqlDataReader reader = command.ExecuteReader();

            bool hasData = reader.Read();
            return hasData ? dataMapper(reader) : default;
        }
    }
}
