using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class CadastrarFuncionarioVM : ACadastrarViewModel<FuncionarioModel>
    {
        private DAOLoja daoLoja;
        private string _salarioString;
        public ObservableCollection<LojaModel> Lojas { get; set; }

        public CadastrarFuncionarioVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarFuncionarioVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            daoLoja = new DAOLoja(_session);

            GetLojas();

            Entidade = new FuncionarioModel
            {
                Loja = Lojas[0]
            };

            Entidade.PropertyChanged += Entidade_PropertyChanged;
            PropertyChanged += CadastrarFuncionarioVM_PropertyChanged;
            AntesDeInserirNoBancoDeDados += ConfiguraFuncionarioAntesDeInserir;

            Salario = "";
        }

        private void CadastrarFuncionarioVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Entidade"))
            {
                if (Entidade.Salario != 0.0)
                {
                    Salario = Entidade.Salario.ToString();
                }
            }
        }

        private void ConfiguraFuncionarioAntesDeInserir()
        {
            if (Entidade.Loja.Cnpj == null)
                Entidade.Loja = null;
        }

        override public async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cpf":
                    var result = await daoEntidade.ListarPorId(Entidade.Cpf);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;

                case "SalarioFamilia":
                    if (!Entidade.SalarioFamilia)
                        Entidade.NumDependentes = 0;
                    break;
            }
        }
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarExcetoDeposito());
        }
        public override void ResetaPropriedades()
        {
            Entidade = new FuncionarioModel
            {
                Loja = Lojas[0]
            };
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Cpf) || string.IsNullOrEmpty(Entidade.Nome))
            {
                BtnSalvarToolTip += "CPF Ou Nome Não Podem Ser Vazios!\n";
                valido = false;
            }

            if (Entidade.SalarioFamilia && Entidade.NumDependentes <= 0)
            {
                BtnSalvarToolTip += "Informe Um Número Válido de Dependentes!\n";
                valido = false;
            }

            double salario = 0.0;
            if (Salario.Trim().Length == 0 || !double.TryParse(Salario, out salario) || salario <= 0.0)
            {
                BtnSalvarToolTip += "Informe Um Valor Válido de Salário!\n";
                valido = false;
            }

            Entidade.Salario = salario;

            return valido;
        }

        public string Salario
        {
            get => _salarioString;
            set
            {
                _salarioString = value;
                OnPropertyChanged("Salario");
            }
        }
    }
}
