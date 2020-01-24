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
            throw new NotImplementedException();
        }

        public string MensagemApagarEntidadeCerteza(RecebimentoCartaoModel e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(RecebimentoCartaoModel e)
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

        public void RestauraEntidade(RecebimentoCartaoModel original, RecebimentoCartaoModel backup)
        {
            throw new NotImplementedException();
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}
