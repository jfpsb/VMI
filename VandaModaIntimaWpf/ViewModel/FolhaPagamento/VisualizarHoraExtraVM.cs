using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class VisualizarHoraExtraVM : APesquisarViewModel<Bonus>
    {
        private DAOFuncionario daoFuncionario;
        private ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>> _listaHoraExtra;
        private IList<Model.Funcionario> funcionarios;
        private DateTime _dataEscolhida;

        public ICommand AbrirImprimirHEComando { get; set; }

        public VisualizarHoraExtraVM(DateTime dataEscolhida, IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Bonus> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOBonus(_session);
            daoFuncionario = new DAOFuncionario(_session);

            ListaHoraExtra = new ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>>();

            GetFuncionarios();

            DataEscolhida = dataEscolhida;

            AbrirImprimirHEComando = new RelayCommand(AbrirImprimirHE);
        }

        private void AbrirImprimirHE(object obj)
        {
            TelaRelatorioHoraExtra telaRelatorioHoraExtra = new TelaRelatorioHoraExtra(ListaHoraExtra);
            telaRelatorioHoraExtra.ShowDialog();
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override void PesquisaItens(string termo)
        {
            var daoBonus = (DAOBonus)daoEntidade;

            ListaHoraExtra.Clear();

            foreach (var func in funcionarios.OrderBy(o => o.Nome))
            {
                TimeSpan he100 = new TimeSpan(), he55 = new TimeSpan();

                var valorHe100 = await daoBonus.ListarHoraExtra100(func, DataEscolhida.Month, DataEscolhida.Year);

                if (valorHe100 != null)
                {
                    var valorHora = valorHe100.BaseCalculo / 220;
                    var qntHoras = Math.Round(valorHe100.Valor / (valorHora * 2), 2, MidpointRounding.AwayFromZero);

                    he100 = new TimeSpan((int)qntHoras, (int)((qntHoras % 1) * 60), 0);
                }

                var valorHe55 = await daoBonus.ListarHoraExtra55(func, DataEscolhida.Month, DataEscolhida.Year);

                if (valorHe55 != null)
                {
                    var valorHora = valorHe55.BaseCalculo / 220;
                    var qntHoras = Math.Round(valorHe55.Valor / (valorHora * 1.55), 2, MidpointRounding.AwayFromZero);

                    he55 = new TimeSpan((int)qntHoras, (int)((qntHoras % 1) * 60), 0);
                }

                ListaHoraExtra.Add(Tuple.Create(func, he100, he55, DataEscolhida));
            }
        }

        private async void GetFuncionarios()
        {
            funcionarios = await daoFuncionario.Listar<Model.Funcionario>();
        }

        public ObservableCollection<Tuple<Model.Funcionario, TimeSpan, TimeSpan, DateTime>> ListaHoraExtra
        {
            get => _listaHoraExtra;
            set
            {
                _listaHoraExtra = value;
                OnPropertyChanged("ListaHoraExtra");
            }
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
