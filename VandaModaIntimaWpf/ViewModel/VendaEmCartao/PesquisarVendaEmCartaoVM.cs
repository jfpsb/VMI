using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
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
        private DAODespesa daoDespesa;
        private DAOTipoDespesa daoTipoDespesa;
        private DateTime _dataEscolhida;
        private double _totalBruto;
        private double _totalLiquido;
        private double _taxaOperadora;

        public ICommand SalvarTaxaComoDespesaComando { get; set; }

        public PesquisarVendaEmCartaoVM()
        {
            daoLoja = new DAOLoja(Session);
            daoOperadora = new DAO<OperadoraCartao>(Session);
            daoEntidade = new DAOVendaEmCartao(Session);
            daoDespesa = new DAODespesa(Session);
            daoTipoDespesa = new DAOTipoDespesa(Session);
            pesquisarViewModelStrategy = new PesquisarVendaEmCartaoVMStrategy();

            DataEscolhida = DateTime.Now;

            var task1 = GetLojas();
            task1.Wait();

            var task2 = GetOperadoras();
            task2.Wait();

            PropertyChanged += PesquisarVendaEmCartaoVM_PropertyChanged;

            SalvarTaxaComoDespesaComando = new RelayCommand(SalvarTaxaComoDespesa, SalvarTaxaValidacao);
        }

        private async void SalvarTaxaComoDespesa(object obj)
        {
            try
            {
                DateTime ultimoDiaMes = new DateTime(DataEscolhida.Year, DataEscolhida.Month, 1).AddMonths(1).AddDays(-1);
                Model.Despesa despesa = new Model.Despesa();
                despesa.Data = ultimoDiaMes;
                despesa.Loja = Loja;
                despesa.Descricao = $"TARIFA MÁQUINA DE CARTÃO {Operadora.Nome}";
                despesa.TipoDespesa = await daoTipoDespesa.RetornaTipoDespesaEmpresarial();
                despesa.Valor = TaxaOperadora;

                await daoDespesa.Inserir(despesa);
                _messageBoxService.Show("Despesa salva com sucesso.", pesquisarViewModelStrategy.PesquisarEntidadeCaption(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Erro ao cadastrar despesa. " +
                    $"Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n{ex.InnerException.Message}", pesquisarViewModelStrategy.PesquisarEntidadeCaption(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SalvarTaxaValidacao(object arg)
        {
            if (TaxaOperadora == 0.0 || Operadora.Nome.StartsWith("TODAS")) return false;
            return true;
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
            TaxaOperadora = TotalBruto - TotalLiquido;
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

        public double TaxaOperadora
        {
            get
            {
                return _taxaOperadora;
            }

            set
            {
                _taxaOperadora = value;
                OnPropertyChanged("TaxaOperadora");
            }
        }
    }
}
