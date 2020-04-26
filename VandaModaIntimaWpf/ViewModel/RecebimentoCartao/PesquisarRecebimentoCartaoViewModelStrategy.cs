using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.OperadoraCartao;
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
            return string.Format(StringResource.GetString("certeza_deletar_recebimento"), e.MesAno, e.Loja.Nome);
        }

        public string MensagemApagarMarcados()
        {
            return StringResource.GetString("deseja_apagar_recebimento_marcados");
        }

        public string MensagemEntidadeDeletada(RecebimentoCartaoModel e)
        {
            return string.Format(StringResource.GetString("recebimento_deletado_com_sucesso"), e.GetContextMenuHeader);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return StringResource.GetString("recebimento_nao_deletado");
        }

        public string MensagemEntidadesDeletadas()
        {
            return StringResource.GetString("recebimentos_deletados_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return StringResource.GetString("recebimentos_nao_deletados");
        }

        public void RestauraEntidade(RecebimentoCartaoModel original, RecebimentoCartaoModel backup)
        {

        }

        public string TelaApagarCaption()
        {
            return StringResource.GetString("apagar_recebimentos");
        }

        public void AbrirCadastrarOperadoraCartao()
        {
            //TODO: implementar ViewModel
            CadastrarOperadoraCartao cadastrarOperadoraCartao = new CadastrarOperadoraCartao();
            cadastrarOperadoraCartao.ShowDialog();
        }
    }
}
