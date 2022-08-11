using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class PesquisarPixVMStrategy : IPesquisarMsgVMStrategy<Model.Pix>
    {
        public string MensagemApagarEntidadeCerteza(Model.Pix e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(Model.Pix e)
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

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisar Pix";
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}
