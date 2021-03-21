using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusPassagemFuncionarioVM : ObservableObject
    {
        private ISession _session;
        private DAOFuncionario daoFuncionario;
        private DAOBonus daoBonus;
        private IMessageBoxService MessageBoxService;
        private double _valorPassagem;
        private DateTime _dataEscolhida;
        private ObservableCollection<EntidadeComCampo<Model.Funcionario>> _entidades;

        public ICommand AdicionarBonusPassagemComando { get; set; }

        public AdicionarBonusPassagemFuncionarioVM(DateTime dataEscolhida, double valorPassagem, IMessageBoxService messageBoxService)
        {
            _session = SessionProvider.GetSession();
            ValorPassagem = valorPassagem;
            DataEscolhida = dataEscolhida;
            MessageBoxService = messageBoxService;
            daoFuncionario = new DAOFuncionario(_session);
            daoBonus = new DAOBonus(_session);

            AdicionarBonusPassagemComando = new RelayCommand(AdicionarBonusPassagem);

            GetFuncionarios();
        }

        private async void AdicionarBonusPassagem(object obj)
        {
            var marcados = Entidades.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();

            if (marcados.Count == 0)
            {
                MessageBoxService.Show("Nenhum Funcionário Foi Selecionado!");
            }
            else
            {
                DateTime dataFolha = DataEscolhida.AddMonths(-1);
                IList<Model.Bonus> bonuses = new List<Bonus>();
                foreach (var funcionario in marcados)
                {
                    Model.Bonus bonus = new Model.Bonus();
                    var now = DateTime.Now;

                    bonus.Funcionario = funcionario;
                    bonus.Data = now;
                    bonus.Descricao = "PASSAGEM DE ÔNIBUS";
                    bonus.Valor = ValorPassagem;
                    bonus.MesReferencia = dataFolha.Month;
                    bonus.AnoReferencia = dataFolha.Year;

                    bonuses.Add(bonus);
                }

                var result = await daoBonus.Inserir(bonuses);

                if (result)
                {
                    MessageBoxService.Show("Os Bônus de Passagem de Ônibus Foram Inseridos Com Sucesso Nos Funcionários Marcados!", "Adicionando Bônus Nas Folhas");
                }
                else
                {
                    MessageBoxService.Show("Houve Um Erro Ao Inserir Os Bônus!");
                }
            }
        }

        public ObservableCollection<EntidadeComCampo<Model.Funcionario>> Entidades
        {
            get { return _entidades; }
            set
            {
                _entidades = value;
                OnPropertyChanged("Entidades");
            }
        }

        public double ValorPassagem
        {
            get => _valorPassagem;
            set
            {
                _valorPassagem = value;
                OnPropertyChanged("ValorPassagem");
            }
        }
        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }

        private async void GetFuncionarios()
        {
            Entidades = new ObservableCollection<EntidadeComCampo<Model.Funcionario>>(EntidadeComCampo<Model.Funcionario>.ConverterIList(await daoFuncionario.Listar<Model.Funcionario>()));
        }

        public void DisposeSession()
        {
            SessionProvider.FechaSession(_session);
        }
    }
}
