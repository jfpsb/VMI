using System;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDeAlmoco : ICalculoDeBonus
    {
        public void AbrirAdicionarBonus(DateTime DataEscolhida, double Total, int numDias, DateTime primeiroDia, DateTime ultimoDia, IMessageBoxService messageBoxService)
        {
            SalvarBonusPorMesVM adicionarBonusVM = new SalvarBonusPorMesVM(DataEscolhida, Total, numDias, primeiroDia, ultimoDia, messageBoxService, new SalvarAlmoco());
            SalvarBonusDeFuncionario adicionarBonus = new SalvarBonusDeFuncionario()
            {
                DataContext = adicionarBonusVM
            };
            adicionarBonus.ShowDialog();
        }

        public string MenuItemHeader1()
        {
            return "Adicionar Vale Alimentação À Folha";
        }

        public double ValorDiario()
        {
            return Config.Instancia.ValorDiarioValeAlimentacao;
        }

        public string WindowCaption()
        {
            return "Cálculo de Vale Alimentação";
        }
    }
}
