using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaViewModel : ACadastrarViewModel
    {
        protected DAOLoja daoLoja;
        protected LojaModel lojaModel;
        private int matrizComboBoxIndex = 0;
        private LojaModel _matriz;
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public CadastrarLojaViewModel(ISession session)
        {
            _session = session;
            daoLoja = new DAOLoja(_session);
            lojaModel = new LojaModel();
            lojaModel.PropertyChanged += CadastrarViewModel_PropertyChanged;
            GetMatrizes();
            Matriz = Matrizes[0];
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Loja.Cnpj) || string.IsNullOrEmpty(Loja.Nome))
            {
                return false;
            }

            return true;
        }

        public override async void Salvar(object parameter)
        {
            _result = await daoLoja.Inserir(Loja);

            if (_result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Loja Cadastrada Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Loja");
        }

        public override void ResetaPropriedades()
        {
            Loja = new LojaModel();
            Loja.Cnpj = Loja.Nome = Loja.Telefone = Loja.Endereco = Loja.InscricaoEstadual = string.Empty;
            Loja.Matriz = Matrizes[0];
        }

        public LojaModel Loja
        {
            get { return lojaModel; }
            set
            {
                lojaModel = value;
                OnPropertyChanged("Loja");
            }
        }
        private async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(StringResource.GetString("matriz_nao_selecionada")));
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoLoja.ListarPorId(Loja.Cnpj);

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
        public int MatrizComboBoxIndex
        {
            get { return matrizComboBoxIndex; }
            set
            {
                matrizComboBoxIndex = value;
                OnPropertyChanged("MatrizComboBoxIndex");
            }
        }

        public LojaModel Matriz
        {
            get
            {
                return _matriz;
            }

            set
            {
                _matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
    }
}
