using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaVM : ACadastrarViewModel<LojaModel>
    {
        private Model.AliquotasImposto _aliquota;
        private ObservableCollection<Model.AliquotasImposto> _aliquotas;
        private bool _isMatriz;
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public ICommand AdicionarAliquotaComando { get; set; }

        public CadastrarLojaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarLojaVMStrategy();
            daoEntidade = new DAOLoja(_session);
            Entidade = new LojaModel();
            GetMatrizes();
            AntesDeInserirNoBancoDeDados += ConfiguraLojaAntesDeInserir;
            AdicionarAliquotaComando = new RelayCommand(AdicionarAliquota);
            Aliquota = new Model.AliquotasImposto();
        }

        private void AdicionarAliquota(object obj)
        {
            Aliquota.DataInsercao = DateTime.Now;
            Aliquota.Loja = Entidade;
            Aliquota.Simples /= 100;
            Aliquota.Icms /= 100;
            Aliquotas.Add(Aliquota);
            Aliquota = new Model.AliquotasImposto();
        }

        private void ConfiguraLojaAntesDeInserir()
        {
            if (Entidade.Matriz?.Cnpj == null)
                Entidade.Matriz = null;

            Entidade.Aliquotas.Clear();

            if (Aliquotas != null)
            {
                foreach (var a in Aliquotas)
                {
                    Entidade.Aliquotas.Add(a);
                }
            }
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Cnpj?.Trim()))
            {
                BtnSalvarToolTip += "O Campo de CNPJ Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
            {
                BtnSalvarToolTip += "O Campo de Nome Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (Entidade.Aluguel.ToString()?.Trim().Length == 0 || Entidade.Aluguel <= 0.0)
            {
                BtnSalvarToolTip += "O Campo de Aluguel Não Pode Ser Vazio Ou Inválido!\n";
                return false;
            }

            return valido;
        }
        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new LojaModel();
            Entidade.Cnpj = Entidade.Nome = Entidade.Telefone = Entidade.Endereco = Entidade.InscricaoEstadual = string.Empty;
            Entidade.Matriz = Matrizes[0];
        }
        private async void GetMatrizes()
        {
            DAOLoja daoLoja = (DAOLoja)daoEntidade;
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoEntidade.ListarPorId(Entidade.Cnpj);

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
                case "Matriz":
                    IsMatriz = Entidade.Matriz?.Cnpj == null;
                    break;
            }
        }

        public ObservableCollection<Model.AliquotasImposto> Aliquotas
        {
            get => _aliquotas;
            set
            {
                _aliquotas = value;
                OnPropertyChanged("Aliquotas");
            }
        }

        public Model.AliquotasImposto Aliquota
        {
            get => _aliquota;
            set
            {
                _aliquota = value;
                OnPropertyChanged("Aliquota");
            }
        }

        public bool IsMatriz
        {
            get => _isMatriz;
            set
            {
                _isMatriz = value;
                OnPropertyChanged("IsMatriz");
            }
        }
    }
}
