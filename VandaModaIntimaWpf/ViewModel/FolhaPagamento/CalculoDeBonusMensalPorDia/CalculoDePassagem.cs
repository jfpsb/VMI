using NHibernate;
using System;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDePassagem : ICalculoDeBonus
    {
        private IWindowService _windowService;

        public CalculoDePassagem()
        {
            _windowService = new WindowService();
        }

        public void AbrirAdicionarBonus(ISession session, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia)
        {
            _windowService.ShowDialog(new SalvarBonusPorMesVM(session, DataEscolhida, Total, valorDiario, numDias, primeiroDia, ultimoDia, new SalvarPassagem()), null);
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
