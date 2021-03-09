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
