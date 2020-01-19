﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaViewModel : ACadastrarViewModel
    {
        protected DAOLoja daoLoja;
        protected LojaModel lojaModel;
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public CadastrarLojaViewModel() : base("Loja")
        {
            daoLoja = new DAOLoja(_session);
            lojaModel = new LojaModel();
            lojaModel.PropertyChanged += CadastrarViewModel_PropertyChanged;
            GetMatrizes();
        }
        public override bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Loja.Cnpj) || string.IsNullOrEmpty(Loja.Nome))
            {
                return false;
            }

            return true;
        }

        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz != null && Loja.Matriz.Nome.Equals("SELECIONE A MATRIZ"))
                Loja.Matriz = null;

            var result = await daoLoja.Inserir(Loja);

            if (result)
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
            Matrizes.Insert(0, new LojaModel("SELECIONE A MATRIZ"));
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
    }
}