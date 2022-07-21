using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class PesquisarPontoEletronicoVM : APesquisarViewModel<Model.PontoEletronico>
    {
        private DAOFuncionario daoFuncionario;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private IList<Model.Funcionario> _funcionarios;
        private Model.Funcionario _funcionario;
        private DateTime _dataEscolhida;
        private int _pesquisarPor;

        public ICommand AbrirConsolidarPontosComando { get; set; }

        public PesquisarPontoEletronicoVM()
        {
            daoEntidade = new DAOPontoEletronico(_session);
            daoFuncionario = new DAOFuncionario(_session);
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();

            var task = GetFuncionarios();
            task.Wait();

            Funcionario = Funcionarios[0];

            DataEscolhida = DateTime.Now; //Realiza pesquisa ao atribuir data
            PropertyChanged += PesquisarPontoEletronicoVM_PropertyChanged;

            PesquisarPor = 0;

            AbrirConsolidarPontosComando = new RelayCommand(AbrirConsolidarPontos);
        }

        private void AbrirConsolidarPontos(object obj)
        {
            new ConsolidarPontosEletronicos().ShowDialog();
        }

        private void PesquisarPontoEletronicoVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PesquisarPor":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "Funcionario":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "DataEscolhida":
                    OnPropertyChanged("TermoPesquisa");
                    break;
            }
        }

        private async Task GetFuncionarios()
        {
            Funcionarios = await daoFuncionario.ListarNaoDemitidos();
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            return null;
        }

        public override async Task PesquisaItens(string termo)
        {
            PontosEletronicos.Clear();

            var dao = daoEntidade as DAOPontoEletronico;

            if (PesquisarPor == 0)
            {
                foreach (var f in Funcionarios)
                {
                    var ponto = await dao.ListarPorDiaFuncionario(DataEscolhida, f);

                    if (ponto == null)
                    {
                        ponto = new Model.PontoEletronico
                        {
                            Funcionario = f,
                            Dia = DateTime.Now
                        };
                    }

                    PontosEletronicos.Add(ponto);
                }
            }
            else
            {
                var dias = DateTimeUtil.RetornaDiasEmMes(DataEscolhida.Year, DataEscolhida.Month);

                foreach (var dia in dias)
                {
                    var ponto = await dao.ListarPorDiaFuncionario(dia, Funcionario);

                    if (ponto == null)
                    {
                        ponto = new Model.PontoEletronico
                        {
                            Funcionario = Funcionario,
                            Dia = dia
                        };
                    }

                    PontosEletronicos.Add(ponto);
                }
            }
        }

        protected override WorksheetContainer<Model.PontoEletronico>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Model.PontoEletronico> PontosEletronicos
        {
            get
            {
                return _pontosEletronicos;
            }

            set
            {
                _pontosEletronicos = value;
                OnPropertyChanged("PontosEletronicos");
            }
        }

        public DateTime DataEscolhida
        {
            get
            {
                return _dataEscolhida;
            }

            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }

        public int PesquisarPor
        {
            get
            {
                return _pesquisarPor;
            }

            set
            {
                _pesquisarPor = value;
                OnPropertyChanged("PesquisarPor");
            }
        }

        public IList<Model.Funcionario> Funcionarios
        {
            get
            {
                return _funcionarios;
            }

            set
            {
                _funcionarios = value;
                OnPropertyChanged("Funcionarios");
            }
        }

        public Model.Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
    }
}
