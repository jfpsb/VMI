namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class MySqlStatement
    {
        private IMySqlStatementStrategy mySqlStatementStrategy;

        public MySqlStatement(IMySqlStatementStrategy mySqlStatementStrategy)
        {
            this.mySqlStatementStrategy = mySqlStatementStrategy;
        }

        public void Insert(IModel model)
        {
            mySqlStatementStrategy.Insert(model);
        }
        public void Update(IModel model)
        {
            mySqlStatementStrategy.Update(model);
        }
        public void Delete(IModel model)
        {
            mySqlStatementStrategy.Delete(model);
        }
    }
}
