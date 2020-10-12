using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    public class EditarFornecedorVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Fornecedor";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Fornecedor Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Fornecedor";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Fornecedor Foi Atualizado Com Sucesso";
        }
    }
}
