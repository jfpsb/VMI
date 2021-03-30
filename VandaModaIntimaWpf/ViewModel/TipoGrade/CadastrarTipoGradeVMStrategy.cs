using System;

namespace VandaModaIntimaWpf.ViewModel.TipoGrade
{
    public class CadastrarTipoGradeVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Tipo de Grade Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Tipo de Grade";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Tipo de Grade";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Tipo de Grade Foi Inserido Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Tipo De Grade";
        }
    }
}
