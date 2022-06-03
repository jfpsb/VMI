using NHibernate;
using System;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDeAlmoco : ICalculoDeBonus
    {
        IWindowService _windowService;
        public CalculoDeAlmoco()
        {
            _windowService = new WindowService();
        }
        public void AbrirAdicionarBonus(ISession session, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia)
        {
            _windowService.ShowDialog(new SalvarBonusPorMesVM(session, DataEscolhida, Total, valorDiario, numDias, primeiroDia, ultimoDia, new SalvarAlmoco()), null);
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
