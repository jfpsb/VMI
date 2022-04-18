using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarMetaIndividualVM : ObservableObject
    {
        private DAOFolhaPagamento daoFolhaPagamento;
        private double _valorMeta;
        private IList<Model.FolhaPagamento> _folhas;
        private IMessageBoxService messageBoxService;
        private ObservableCollection<EntidadeComCampo<Model.FolhaPagamento>> _folhaPagamentos;

        public ICommand AdicionarMetaComando { get; set; }

        public AdicionarMetaIndividualVM(ISession session, IList<Model.FolhaPagamento> folhas, IMessageBoxService messageBoxService)
        {
            _folhas = folhas.OrderBy(o => o.Funcionario.Nome).ToList();
            this.messageBoxService = messageBoxService;
            daoFolhaPagamento = new DAOFolhaPagamento(session);
            FolhaPagamentos = new ObservableCollection<EntidadeComCampo<Model.FolhaPagamento>>(EntidadeComCampo<Model.FolhaPagamento>.CriarListaEntidadeComCampo(_folhas));

            AdicionarMetaComando = new RelayCommand(AdicionarMeta, ValidacaoMeta);
        }

        private async void AdicionarMeta(object obj)
        {
            var folhas = FolhaPagamentos.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();

            foreach (var f in folhas)
            {
                f.MetaDeVenda = ValorMeta;
            }

            try
            {
                await daoFolhaPagamento.InserirOuAtualizar(folhas);
                messageBoxService.Show("Valores de meta adicionados com sucesso em funcionários marcados!", "Adicionar Valor De Meta Individual",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ResetaPropriedades();
            }
            catch (Exception ex)
            {
                messageBoxService.Show("Erro ao adicionar valores de meta em funcionários marcados." +
                    $"Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n{ex.InnerException.Message}", "Adicionar Valor De Meta Individual");
            }
        }

        public void ResetaPropriedades()
        {
            ValorMeta = 0;
            FolhaPagamentos = new ObservableCollection<EntidadeComCampo<Model.FolhaPagamento>>(EntidadeComCampo<Model.FolhaPagamento>.CriarListaEntidadeComCampo(_folhas));
        }

        public bool ValidacaoMeta(object parameter)
        {
            if (ValorMeta <= 0)
                return false;

            if (FolhaPagamentos.Where(w => w.IsChecked).Count() == 0)
                return false;

            return true;
        }

        public ObservableCollection<EntidadeComCampo<Model.FolhaPagamento>> FolhaPagamentos
        {
            get => _folhaPagamentos;
            set
            {
                _folhaPagamentos = value;
                OnPropertyChanged("FolhaPagamentos");
            }
        }
        public double ValorMeta
        {
            get => _valorMeta;
            set
            {
                _valorMeta = value;
                OnPropertyChanged("ValorMeta");
            }
        }
    }
}
