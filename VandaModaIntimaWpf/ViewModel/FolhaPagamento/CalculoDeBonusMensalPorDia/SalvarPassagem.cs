namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarPassagem : ISalvarBonus
    {
        public string DescricaoBonus(int numDias)
        {
            return $"PASSAGEM DE ÔNIBUS ({numDias} DIAS)";
        }

        public string MensagemCaption()
        {
            return "Adicionando Passagem Nas Folhas De Pagamento";
        }

        public string MensagemInseridoErro()
        {
            return "O Valor de Passagem Foi Inserido Com Sucesso Como Bônus Nos Funcionários Marcados!";
        }

        public string MensagemInseridoSucesso()
        {
            return "Erro Ao Inserir O Valor de Passagens!";
        }

        public string RecebeRegularmenteHeader()
        {
            return "Recebe Passagem Regularmente";
        }
    }
}
