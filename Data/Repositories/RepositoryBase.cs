namespace Data.Repositories
{
    using Microsoft.Data.SqlClient;

    public abstract class RepositoryBase<TEntity, TId>
    {
        private readonly string connectionString = "Server=localhost;Database=controle_financeiro;User Id=sa;Password=admin;TrustServerCertificate=True;";

        public List<TEntity> List(string sql)
        {
            return this.ExecuteSQL(sql, this.EntityListMapper) ?? [];
        }

        public virtual TEntity? Get(string sql)
        {
            return this.ExecuteSQL(sql, this.EntityMapper);
        }

        public TEntity? Update(string sql)
        {
            return default;
        }

        internal abstract TEntity EntityMapper(SqlDataReader reader);

        internal List<TEntity> EntityListMapper(SqlDataReader reader)
        {
            List<TEntity> result = [];

            while (reader.Read())
            {
                result.Add(this.EntityMapper(reader));
            }

            return result;
        }

        private TMappedData? ExecuteSQL<TMappedData>(string sql, Func<SqlDataReader, TMappedData> dataMapper)
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
