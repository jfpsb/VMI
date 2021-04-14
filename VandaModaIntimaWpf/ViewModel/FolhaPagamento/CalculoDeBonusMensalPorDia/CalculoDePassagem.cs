using System;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDePassagem : ICalculoDeBonus
    {
        public void AbrirAdicionarBonus(DateTime DataEscolhida, double Total, int numDias, IMessageBoxService messageBoxService)
        {
            SalvarBonusDeFuncionarioVM adicionarBonusVM = new SalvarBonusDeFuncionarioVM(DataEscolhida, Total, numDias, messageBoxService, new SalvarPassagem());
            SalvarBonusDeFuncionario adicionarBonus = new SalvarBonusDeFuncionario()
            {
                DataContext = adicionarBonusVM
            };
            adicionarBonus.ShowDialog();
        }

        public string MenuItemHeader1()
        {
            return "Adicionar Bônus de Passagem À Folha";
        }

        public double ValorDiario()
        {
            return Config.Instancia.ValorDiarioPassagemOnibus;
        }

        public string WindowCaption()
        {
            return "Cálculo de Passagem Ônibus";
        }
    }
}
