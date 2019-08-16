﻿using NHibernate;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Arquivo;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel : ObservableObject, IPesquisarViewModel
    {
        protected ISession _session;
        private string termoPesquisa;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        protected ExcelStrategy excelStrategy;
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }
        public ICommand ImportarExcelComando { get; set; }

        public APesquisarViewModel()
        {
            AbrirCadastrarComando = new RelayCommand(AbrirCadastrar, (object parameter) => { return true; });
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox, (object parameter) => { return true; });
            AbrirEditarComando = new RelayCommand(AbrirEditar, (object parameter) => { return true; });
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados, (object parameter) => { return true; });
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados, (object parameter) => { return true; });
            ExportarExcelComando = new RelayCommand(ExportarExcel, (object parameter) => { return true; });
            ImportarExcelComando = new RelayCommand(ImportarExcel, (object parameter) => { return true; });

            _session = SessionProvider.GetSession();
        }

        public abstract void AbrirCadastrar(object parameter);
        public abstract void AbrirApagarMsgBox(object parameter);
        public abstract void AbrirEditar(object parameter);
        public abstract void ChecarItensMarcados(object parameter);
        public abstract void ApagarMarcados(object parameter);
        public abstract void ExportarExcel(object parameter);
        public abstract void ImportarExcel(object parameter);
        public abstract void GetItems(string termo);
        protected void PesquisarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    GetItems(termoPesquisa);
                    break;
            }
        }
        public string TermoPesquisa
        {
            get { return termoPesquisa; }
            set
            {
                termoPesquisa = value;
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public Visibility VisibilidadeBotaoApagarSelecionado
        {
            get { return visibilidadeBotaoApagarSelecionado; }
            set
            {
                visibilidadeBotaoApagarSelecionado = value;
                OnPropertyChanged("VisibilidadeBotaoApagarSelecionado");
            }
        }
        public void DisposeSession()
        {
            SessionProvider.FechaSession();
        }
    }
}
