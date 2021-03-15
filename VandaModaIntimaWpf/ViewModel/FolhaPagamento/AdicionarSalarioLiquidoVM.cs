using NHibernate;
using System;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarSalarioLiquidoVM : ACadastrarViewModel<Model.FolhaPagamento>
    {
        private string _salarioLiquido;

        public AdicionarSalarioLiquidoVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService) : base(session, messageBoxService, false)
        {
            _session = session;
            daoEntidade = new DAOFolhaPagamento(session);
            viewModelStrategy = new AdicionarSalarioLiquidoVMStrategy();
            Entidade = folha;
            AposInserirNoBancoDeDados += FecharTela;
            SalarioLiquido = "0";
        }

        private void FecharTela(AposInserirBDEventArgs e)
        {
            if (e.Sucesso)
            {
                MessageBoxService.Show("Horas Extras Adicionadas Com Sucesso!");

                if (e.Parametro is ICloseable closeable)
                {
                    closeable.Close();
                }
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {

        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;
            double salario = 0;

            if (string.IsNullOrEmpty(SalarioLiquido) || !double.TryParse(SalarioLiquido, out salario) || salario <= 0.0)
            {
                Console.WriteLine(salario.ToString());
                BtnSalvarToolTip += "Informe Um Valor De Salário Líquido Válido!\n";
                valido = false;
            }
            else
            {
                Entidade.SalarioLiquido = salario;
            }

            return valido;
        }

        public string SalarioLiquido
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
