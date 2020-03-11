using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AbrirCadastrar(object parameter)
        {
            CadastrarContagem cadastrarContagem = new CadastrarContagem();
            cadastrarContagem.ShowDialog();
        }

        public bool? AbrirEditar(ContagemModel entidade)
        {
            EditarContagem editarContagem = new EditarContagem(entidade);
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
