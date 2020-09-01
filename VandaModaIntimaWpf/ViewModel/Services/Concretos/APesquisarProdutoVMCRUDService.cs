using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    class APesquisarProdutoVMCRUDService : IAPesquisarVMCRUDService<Model.Produto>
    {
        private DAOProduto _dao;
        public APesquisarProdutoVMCRUDService(DAO dao)
        {
            _dao = (DAOProduto)dao;
        }
        public async Task<bool> Deletar(object entidade)
        {
            return await _dao.Deletar(entidade);
        }

        public Task<bool> Deletar(IList<Model.Produto> entidades)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<EntidadeComCampo<Model.Produto>> PesquisaItens(int pesquisaIndex, string termo)
        {
            throw new NotImplementedException();
        }
    }
}
