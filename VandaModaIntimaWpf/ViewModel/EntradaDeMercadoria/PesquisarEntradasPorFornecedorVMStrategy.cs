using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class PesquisarEntradasPorFornecedorVMStrategy : IPesquisarMsgVMStrategy<Model.EntradaMercadoriaProdutoGrade>
    {
        public string MensagemApagarEntidadeCerteza(EntradaMercadoriaProdutoGrade e)
        {
            throw new NotImplementedException();
        }

        public string MensagemApagarMarcados()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeDeletada(EntradaMercadoriaProdutoGrade e)
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
            return "Entradas De Mercadoria Por Fornecedor";
        }

        public string TelaApagarCaption()
        {
            throw new NotImplementedException();
        }
    }
}
