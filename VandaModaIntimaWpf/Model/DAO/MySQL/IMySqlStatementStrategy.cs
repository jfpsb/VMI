using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public interface IMySqlStatementStrategy
    {
        void Insert(IModel model);
        void Update(IModel model);
        void Delete(IModel model);
        void Insert(IList<IModel> models);
        void Update(IList<IModel> models);
        void Delete(IList<IModel> models);
    }
}
