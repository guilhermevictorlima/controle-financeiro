namespace Data.Repositories
{
    using Data.Entities;
    using Microsoft.Data.SqlClient;

    public class LancamentoFinanceiroRepository : RepositoryBase<string, int>
    {
        public string Get()
        {
            return this.Get("SELECT @@SERVICENAME as service_name") ?? "Nenhum resultado encontrado";
        }

        internal override string EntityMapper(SqlDataReader reader)
        {
            return $"Service Name: {reader.GetString(0)}";
        }
    }
}
