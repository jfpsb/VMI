using NHibernate;
using System;
using System.Collections.Generic;
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
            return $"Tem Certeza Que Deseja Apagar a Contagem {e.GetContextMenuHeader}?";
        }

        public string MensagemApagarMarcados()
        {
            return "Desejar Apagar as Contagens Marcadas?";
        }

        public string MensagemEntidadeDeletada(ContagemModel e)
        {
            return $"Contagem {e.GetContextMenuHeader} Foi Deletada Com Sucesso";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Contagem Não Foi Deletada";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Contagens Foram Deletadas Com Sucesso";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Contagens Não Foram Deletadas";
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
            return "Apagar Contagem(ns)";
        }
    }
}
