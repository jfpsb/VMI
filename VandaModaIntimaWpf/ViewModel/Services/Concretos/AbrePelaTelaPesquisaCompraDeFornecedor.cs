using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbrePelaTelaPesquisaCompraDeFornecedor : IAbrePelaTelaPesquisaService<Model.CompraDeFornecedor>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarCompraDeFornecedorVM viewModel = new CadastrarCompraDeFornecedorVM(session, new MessageBoxService(), false);
            SalvarCompraDeFornecedor view = new SalvarCompraDeFornecedor
            {
                DataContext = viewModel
            };
            return view.ShowDialog();
        }

        public bool? AbrirEditar(Model.CompraDeFornecedor clone, ISession session)
        {
            EditarCompraDeFornecedorVM viewModel = new EditarCompraDeFornecedorVM(session, clone, new MessageBoxService());
            SalvarCompraDeFornecedor view = new SalvarCompraDeFornecedor
            {
                DataContext = viewModel
            };
            return view.ShowDialog();
        }

        public void AbrirExportarSQL(IList<Model.CompraDeFornecedor> entidades, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirImprimir(IList<Model.CompraDeFornecedor> lista)
        {
            throw new NotImplementedException();
        }
    }
}
