using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarMetaIndividualVM : ObservableObject
    {
        private DAOFolhaPagamento daoFolhaPagamento;
        private double _valorMeta;
        private IList<Model.FolhaPagamento> _folhas;
        private IMessageBoxService _messageBoxService;
        private ObservableCollection<EntidadeComCampo<Model.FolhaPagamento>> _folhaPagamentos;

        public ICommand AdicionarMetaComando { get; set; }

        public AdicionarMetaIndividualVM(ISession session, IList<Model.FolhaPagamento> folhas, IMessageBoxService messageBoxService)
        {
            _folhas = folhas.OrderBy(o => o.Funcionario.Nome).ToList();
            _messageBoxService = messageBoxService;
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

            var result = await daoFolhaPagamento.InserirOuAtualizar(folhas);

            if (result)
            {
                _messageBoxService.Show("Valores De Meta Adicionados Com Sucesso Em Funcionários Marcados!", "Adicionar Valor De Meta Individual");
                ResetaPropriedades();
            }
            else
            {
                _messageBoxService.Show("Erro Ao Adicionar Valores De Meta Em Funcionários Marcados!", "Adicionar Valor De Meta Individual");
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
