using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class VisualizarHoraExtraFaltasVM : APesquisarViewModel<Model.HoraExtra>
    {
        private DAOFuncionario daoFuncionario;
        private DAOFaltas daoFaltas;
        private ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> _listaHoraExtra;
        private IList<Model.Funcionario> funcionarios;
        private IList<Model.FolhaPagamento> folhas;
        private DateTime _dataEscolhida;

        public ICommand AbrirImprimirHEComando { get; set; }

        public VisualizarHoraExtraFaltasVM(IList<Model.FolhaPagamento> listaFolhas, DateTime dataEscolhida)
        {
            daoEntidade = new DAOHoraExtra(_session);
            daoFuncionario = new DAOFuncionario(_session);
            daoFaltas = new DAOFaltas(_session);
            folhas = listaFolhas;
            ListaHoraExtra = new ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>>();
            GetFuncionarios();
            DataEscolhida = dataEscolhida;
            AbrirImprimirHEComando = new RelayCommand(AbrirImprimirHE);
        }

        private void AbrirImprimirHE(object obj)
        {
            //TODO: implementar viewmodel
            TelaRelatorioHoraExtraFaltas telaRelatorioHoraExtra = new TelaRelatorioHoraExtraFaltas(_session, ListaHoraExtra);
            telaRelatorioHoraExtra.ShowDialog();
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override Task PesquisaItens(string termo)
        {
            var daoHoraExtra = daoEntidade as DAOHoraExtra;

            ListaHoraExtra.Clear();

            foreach (var f in folhas)
            {
                var horasExtras = await daoHoraExtra.ListarPorAnoMesFuncionario(DataEscolhida.Year, DataEscolhida.Month, f.Funcionario);
                var falta = await daoFaltas.ListarFaltasPorMesFuncionarioSoma(DataEscolhida.Year, DataEscolhida.Month, f.Funcionario);

                if (falta == null)
                    falta = new Model.Faltas();

                var he100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/100%")).FirstOrDefault();
                var heNormal = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Equals("HORA EXTRA C/060%")).FirstOrDefault();

                if (he100 == null)
                    he100 = new Model.HoraExtra();

                if (heNormal == null)
                    heNormal = new Model.HoraExtra();

                var possuiBonusPagoEmFolha = f.Bonus.Where(w => w.PagoEmFolha).Count() > 0;

                if (he100.EmTimeSpan.TotalSeconds == 0
                    && heNormal.EmTimeSpan.TotalSeconds == 0
                    && falta.EmTimeSpan.TotalSeconds == 0
                    && !possuiBonusPagoEmFolha)
                    continue;

                var tupla = Tuple.Create(f, he100, heNormal, falta, DataEscolhida);
                ListaHoraExtra.Add(tupla);
            }
        }

        private async void GetFuncionarios()
        {
            funcionarios = await daoFuncionario.Listar();
        }

        protected override WorksheetContainer<Model.HoraExtra>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Tuple<Model.FolhaPagamento, Model.HoraExtra, Model.HoraExtra, Model.Faltas, DateTime>> ListaHoraExtra
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
