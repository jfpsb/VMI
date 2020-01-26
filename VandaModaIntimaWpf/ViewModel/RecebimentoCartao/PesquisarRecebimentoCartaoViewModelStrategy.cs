using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AbrirCadastrar(object parameter)
        {
            CadastrarRecebimentoCartao cadastrar = new CadastrarRecebimentoCartao();
            cadastrar.ShowDialog();
        }

        public bool? AbrirEditar(RecebimentoCartaoModel entidade)
        {
            return false;
        }

        public string MensagemApagarEntidadeCerteza(RecebimentoCartaoModel e)
        {
            return $"Tem Certeza Que Deseja Apagar o Recebimento de {e.MesAno} da Loja {e.Loja.Nome}?";
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
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
