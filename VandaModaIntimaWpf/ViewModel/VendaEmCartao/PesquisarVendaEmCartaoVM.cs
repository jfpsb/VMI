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
        private DateTime _dataEscolhida;
        private double _totalBruto;
        private double _totalLiquido;

        public PesquisarVendaEmCartaoVM()
        {
            daoLoja = new DAOLoja(Session);
            daoOperadora = new DAO<OperadoraCartao>(Session);
            daoEntidade = new DAOVendaEmCartao(Session);
            pesquisarViewModelStrategy = new PesquisarVendaEmCartaoVMStrategy();

            DataEscolhida = DateTime.Now;

            var task1 = GetLojas();
            task1.Wait();

            var task2 = GetOperadoras();
            task2.Wait();

            PropertyChanged += PesquisarVendaEmCartaoVM_PropertyChanged;
        }

        private void PesquisarVendaEmCartaoVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DataEscolhida":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "Operadora":
                    OnPropertyChanged("TermoPesquisa");
                    break;
                case "Loja":
                    OnPropertyChanged("TermoPesquisa");
                    break;
            }
        }

        private async Task GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
            Loja = Lojas[0];
        }

        private async Task GetOperadoras()
        {
            Operadoras = new ObservableCollection<Model.OperadoraCartao>(await daoOperadora.Listar());
            Operadoras.Insert(0, new OperadoraCartao() { Nome = "TODAS AS OPERADORAS" });
            Operadora = Operadoras[0];
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            return new CadastrarVendaEmCartaoVM(_session);
        }

        public override object GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public override async Task PesquisaItens(string termo)
        {
            IList<Model.VendaEmCartao> vendas;

            if (Operadora.Nome.StartsWith("TODAS"))
            {
                vendas = await (daoEntidade as DAOVendaEmCartao).ListarPorMesPorLoja(DataEscolhida, Loja);
            }
            else
            {
                vendas = await (daoEntidade as DAOVendaEmCartao).ListarPorMesPorLojaOperadora(DataEscolhida, Loja, Operadora);
            }

            Entidades = new ObservableCollection<EntidadeComCampo<Model.VendaEmCartao>>(EntidadeComCampo<Model.VendaEmCartao>.CriarListaEntidadeComCampo(vendas));
            TotalBruto = vendas.Sum(s => s.ValorBruto);
            TotalLiquido = vendas.Sum(s => s.ValorLiquido);
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

        public double TotalBruto
        {
            get
            {
                return _totalBruto;
            }

            set
            {
                _totalBruto = value;
                OnPropertyChanged("TotalBruto");
            }
        }

        public double TotalLiquido
        {
            get
            {
                return _totalLiquido;
            }

            set
            {
                _totalLiquido = value;
                OnPropertyChanged("TotalLiquido");
            }
        }
    }
}
