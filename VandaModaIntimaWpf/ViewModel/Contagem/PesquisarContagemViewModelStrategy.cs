using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View.Contagem;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class PesquisarContagemViewModelStrategy : IPesquisarViewModelStrategy<ContagemModel>
    {
        public void AbrirAjuda(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool? AbrirCadastrar(object parameter, ISession session)
        {
            CadastrarContagemViewModel cadastrarContagemViewModel = new CadastrarContagemViewModel(session);
            CadastrarContagem cadastrarContagem = new CadastrarContagem()
            {
                DataContext = cadastrarContagemViewModel
            };

            return cadastrarContagem.ShowDialog();
        }

        public bool? AbrirEditar(ContagemModel clone, ISession session)
        {
            EditarContagemViewModel editarContagemViewModel = new EditarContagemViewModel(session)
            {
                Contagem = clone
            };

            EditarContagem editarContagem = new EditarContagem()
            {
                DataContext = editarContagemViewModel
            };

            return editarContagem.ShowDialog();
        }

        public void AbrirExportarSQL(object parameter, IList<ContagemModel> entidades)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(ContagemModel e)
        {
            return string.Format(StringResource.GetString("certeza_deletar_contagem"), e.GetContextMenuHeader);
        }

        public string MensagemApagarMarcados()
        {
            return StringResource.GetString("deseja_apagar_contagem_marcadas");
        }

        public string MensagemEntidadeDeletada(ContagemModel e)
        {
            return string.Format(StringResource.GetString("contagem_deletada_com_sucesso"), e.GetContextMenuHeader);
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return StringResource.GetString("contagem_nao_deletada");
        }

        public string MensagemEntidadesDeletadas()
        {
            return StringResource.GetString("contagens_deletadas_com_sucesso");
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return StringResource.GetString("contagens_nao_deletadas");
        }

        public void RestauraEntidade(ContagemModel original, ContagemModel backup)
        {
            original.Loja = backup.Loja;
            original.Data = backup.Data;
            original.TipoContagem = backup.TipoContagem;
            original.Finalizada = backup.Finalizada;
        }

        public string TelaApagarCaption()
        {
            return StringResource.GetString("apagar_contagens");
        }
    }
}
