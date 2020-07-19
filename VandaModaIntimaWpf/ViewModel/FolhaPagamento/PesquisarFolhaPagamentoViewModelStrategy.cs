using NHibernate;
using System;
using System.Collections.Generic;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class PesquisarFolhaPagamentoViewModelStrategy : IPesquisarViewModelStrategy<FolhaPagamentoModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirEditar(FolhaPagamentoModel entidade, ISession session)
        {
            throw new NotImplementedException();
        }

        public void AbrirExportarSQL(object parameter, IList<FolhaPagamentoModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(FolhaPagamentoModel e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(FolhaPagamentoModel e)
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeNaoDeletada()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesDeletadas()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            throw new NotImplementedException();
        }

        public void RestauraEntidade(FolhaPagamentoModel original, FolhaPagamentoModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}
