using NHibernate;
using System;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDePassagem : ICalculoDeBonus
    {
        private IOpenViewService openView;

        public CalculoDePassagem()
        {
            openView = new OpenView();
        }

        public void AbrirAdicionarBonus(ISession session, IMessageBoxService messageBox, bool isUpdate, DateTime DataEscolhida, double Total, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia)
        {
            openView.ShowDialog(new SalvarBonusPorMesVM(session, messageBox, isUpdate, DataEscolhida, Total, valorDiario, numDias, primeiroDia, ultimoDia, new SalvarPassagem()));
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
