namespace Data.Repositories
{
    // TODO implementar conexão com banco
    public class RepositoryBase<TEntity, TId>
    {

        public List<TEntity> List(string sql)
        {
            return
                [];
        }

        public TEntity? Get(string sql)
        {
            return default;
        }

        public TEntity? Update(string sql)
        {
            return default;
        }

    }
}
