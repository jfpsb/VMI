using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.TipoGrade
{
    public class CadastrarTipoGradeVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Tipo de Grade Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Criação de Tipo de Grade Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Tipo de Grade";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Tipo de Grade";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Tipo de Grade Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Tipo de Grade";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Tipo de Grade Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Tipo de Grade";
        }
    }
}
