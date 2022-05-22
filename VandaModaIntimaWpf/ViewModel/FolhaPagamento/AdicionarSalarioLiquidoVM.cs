using NHibernate;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarSalarioLiquidoVM : ACadastrarViewModel<Model.FolhaPagamento>
    {
        private double _salarioLiquido;

        public AdicionarSalarioLiquidoVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService) : base(session, messageBoxService, false)
        {
            _session = session;
            daoEntidade = new DAOFolhaPagamento(session);
            viewModelStrategy = new AdicionarSalarioLiquidoVMStrategy();
            Entidade = folha;
            AposInserirNoBancoDeDados += FecharTela;
            PropertyChanged += SetaValorSalarioLiquido;
        }

        private void SetaValorSalarioLiquido(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SalarioLiquido"))
            {
                Entidade.SalarioLiquido = SalarioLiquido;
            }
        }

        private void FecharTela(AposInserirBDEventArgs e)
        {
            if (e.Sucesso)
            {
                if (e.Parametro is ICloseable closeable)
                {
                    closeable.Close();
                }
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {

        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (SalarioLiquido <= 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor De Salário Líquido Válido!\n";
                valido = false;
            }

            return valido;
        }

        public double SalarioLiquido
        {
            get => _salarioLiquido;
            set
            {
                _salarioLiquido = value;
                OnPropertyChanged("SalarioLiquido");
            }
        }
    }
}
