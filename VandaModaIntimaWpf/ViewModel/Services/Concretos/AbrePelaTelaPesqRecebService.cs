using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.View.OperadoraCartao;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    class AbrePelaTelaPesqRecebService : IAbrePelaTelaPesquisaService<Model.RecebimentoCartao>
    {
        public void AbrirAjuda()
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(ISession session)
        {
            CadastrarRecebimentoVM cadastrarRecebimentoCartaoViewModel = new CadastrarRecebimentoVM(session, new MessageBoxService());
            CadastrarRecebimentoCartao cadastrar = new CadastrarRecebimentoCartao()
            {
                DataContext = cadastrarRecebimentoCartaoViewModel
            };
            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(Model.RecebimentoCartao clone, ISession session)
        {
            return false;
        }

        public void AbrirExportarSQL(IList<Model.RecebimentoCartao> entidades)
        {
            throw new NotImplementedException();
        }

        public void AbrirCadastrarOperadoraCartao()
        {
            //TODO: implementar ViewModel
            CadastrarOperadoraCartao cadastrarOperadoraCartao = new CadastrarOperadoraCartao();
            cadastrarOperadoraCartao.ShowDialog();
        }
    }
}
