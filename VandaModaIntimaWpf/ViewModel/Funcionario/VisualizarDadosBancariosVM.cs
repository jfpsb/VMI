using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class VisualizarDadosBancariosVM : ObservableObject
    {
        private ObservableCollection<Model.ChavePix> _chavesPix;
        private ObservableCollection<Model.ContaBancaria> _contasBancarias;
        private ChavePix _chavePix;
        private ContaBancaria _contaBancaria;
        public ICommand CopiarChavePixComando { get; set; }

        public VisualizarDadosBancariosVM(Model.Funcionario funcionario)
        {
            ChavesPix = new ObservableCollection<Model.ChavePix>(funcionario.ChavesPix);
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(funcionario.ContasBancarias);
            CopiarChavePixComando = new RelayCommand(CopiarChavePix);
        }

        private void CopiarChavePix(object obj)
        {
            Clipboard.SetText(ChavePix.Chave);
        }

        public ObservableCollection<ChavePix> ChavesPix
        {
            get
            {
                return _chavesPix;
            }

            set
            {
                _chavesPix = value;
                OnPropertyChanged("ChavesPix");
            }
        }

        public ObservableCollection<ContaBancaria> ContasBancarias
        {
            get
            {
                return _contasBancarias;
            }

            set
            {
                _contasBancarias = value;
                OnPropertyChanged("ContasBancarias");
            }
        }

        public ChavePix ChavePix
        {
            get
            {
                return _chavePix;
            }

            set
            {
                _chavePix = value;
                OnPropertyChanged("ChavePix");
            }
        }

        public ContaBancaria ContaBancaria
        {
            get
            {
                return _contaBancaria;
            }

            set
            {
                _contaBancaria = value;
                OnPropertyChanged("ContaBancaria");
            }
        }
    }
}
