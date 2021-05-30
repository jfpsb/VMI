using System;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class PesquisarRepresentanteVMStrategy : IPesquisarMsgVMStrategy<Model.Representante>
    {
        public string MensagemApagarEntidadeCerteza(Model.Representante e)
        {
            return $"Tem Certeza Que Deseja Apagar o Representante {e.Nome}";
        }

        public string MensagemApagarMarcados()
        {
            return "Tem Certeza Que Deseja Apagar Os Representantes Marcados?";
        }

        public string MensagemEntidadeDeletada(Model.Representante e)
        {
            return $"Representante {e.Nome} Foi Deletado!";
        }

        public string MensagemEntidadeNaoDeletada()
        {
            return "Erro Ao Apagar Representante!";
        }

        public string MensagemEntidadesDeletadas()
        {
            return "Representantes Marcados Foram Apagados Com Sucesso!";
        }

        public string MensagemEntidadesNaoDeletadas()
        {
            return "Erro Ao Apagar Representantes Marcados!";
        }

        public string PesquisarEntidadeCaption()
        {
            return "Pesquisa De Representante";
        }

        public string TelaApagarCaption()
        {
            return "Apagar Representante";
        }
    }
}
