namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class SalvarAlmoco : ISalvarBonus
    {
        public string DescricaoBonus(int numDias)
        {
            return $"ALIMENTAÇÃO ({numDias} DIAS)";
        }

        public string MensagemCaption()
        {
            return "Adicionando Vale Alimentação Nas Folhas De Pagamento";
        }

        public string MensagemInseridoErro()
        {
            return "O Valor de Vale de Alimentação Foi Inserido Com Sucesso Como Bônus Nos Funcionários Marcados!";
        }

        public string MensagemInseridoSucesso()
        {
            return "Erro Ao Inserir O Valor de Vale Alimentação!";
        }

        public string RecebeRegularmenteHeader()
        {
            return "Recebe Almoço Regularmente";
        }
    }
}
