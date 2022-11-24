using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class PesquisarVendaEmCartaoVM : APesquisarViewModel<Model.VendaEmCartao>
    {
        private ObservableCollection<Model.Loja> lojas;
        private ObservableCollection<Model.OperadoraCartao> operadoras;
        private Model.Loja _loja;
        private Model.OperadoraCartao _operadora;
        private DAOLoja daoLoja;
        private DAO<Model.OperadoraCartao> daoOperadora;

        public PesquisarVendaEmCartaoVM()
        {
            daoLoja = new DAOLoja(Session);
            daoOperadora = new DAO<OperadoraCartao>(Session);
            daoEntidade = new DAO<Model.VendaEmCartao>(Session);
            pesquisarViewModelStrategy = new PesquisarVendaEmCartaoVMStrategy();

            var task1 = GetLojas();
            task1.Wait();

            var task2 = GetOperadoras();
            task2.Wait();
        }

        private async Task GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
        }

        private async Task GetOperadoras()
        {
            Operadoras = new ObservableCollection<Model.OperadoraCartao>(await daoOperadora.Listar());
            Operadoras.Insert(0, new OperadoraCartao() { Nome = "TODAS AS OPERADORAS" });
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
            throw new NotImplementedException();
        }

        public override Task PesquisaItens(string termo)
        {
            throw new NotImplementedException();
        }

        protected override WorksheetContainer<Model.VendaEmCartao>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Model.Loja> Lojas
        {
            get
            {
                return lojas;
            }

            set
            {
                lojas = value;
                OnPropertyChanged("Loja");
            }
        }

        public ObservableCollection<OperadoraCartao> Operadoras
        {
            get
            {
                return operadoras;
            }

            set
            {
                operadoras = value;
                OnPropertyChanged("Operadoras");
            }
        }

        public Model.Loja Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public OperadoraCartao Operadora
        {
            get
            {
                return _operadora;
            }

            set
            {
                _operadora = value;
                OnPropertyChanged("Operadora");
            }
        }
    }
}
