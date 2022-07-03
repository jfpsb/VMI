using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class PesquisarPontoEletronicoVM : APesquisarViewModel<Model.PontoEletronico>
    {
        private DAOFuncionario daoFuncionario;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private IList<Model.Funcionario> funcionarios;
        private DateTime _dataEscolhida;

        public PesquisarPontoEletronicoVM()
        {
            daoEntidade = new DAOPontoEletronico(_session);
            daoFuncionario = new DAOFuncionario(_session);
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();

            var task = GetFuncionarios();
            task.Wait();

            DataEscolhida = DateTime.Now; //Realiza pesquisa ao atribuir data
        }

        private async Task GetFuncionarios()
        {
            funcionarios = await daoFuncionario.ListarNaoDemitidos();
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

            foreach (var f in funcionarios)
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
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
