using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class PesquisarRecebimentoCartaoViewModelStrategy : IPesquisarViewModelStrategy<RecebimentoCartaoModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarRecebimentoCartaoViewModel cadastrarRecebimentoCartaoViewModel = new CadastrarRecebimentoCartaoViewModel(session);
            CadastrarRecebimentoCartao cadastrar = new CadastrarRecebimentoCartao()
            {
                DataContext = cadastrarRecebimentoCartaoViewModel
            };
            return cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(RecebimentoCartaoModel clone, ISession session)
        {
            return false;
        }

        public void AbrirExportarSQL(object parameter, IList<RecebimentoCartaoModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(RecebimentoCartaoModel e)
        {
            return $"Tem Certeza Que Deseja Apagar o Recebimento de {e.MesAno} da Loja {e.Loja.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Deseja Apagar os Recebimentos Marcados?";
        }

        public string MensagemEntidadeDeletada(RecebimentoCartaoModel e)
        {
            return $"Recebimento {e.GetContextMenuHeader} Deletado Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Recebimento Não Foi Deletado";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Recebimentos Deletados Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Apagar Recebimentos";
        }

        public void RestauraEntidade(RecebimentoCartaoModel original, RecebimentoCartaoModel backup)
        {

        }

        public string TelaApagarCaption()
        {
            return "Apagar Recebimento(s)";
        }
    }
}
