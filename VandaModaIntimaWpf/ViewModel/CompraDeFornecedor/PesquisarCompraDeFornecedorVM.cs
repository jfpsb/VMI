using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class PesquisarCompraDeFornecedorVM : APesquisarViewModel<Model.CompraDeFornecedor>
    {
        private int pesquisarPor;
        private Model.Loja _loja;
        private ObservableCollection<Model.Loja> _lojas;
        private DAOLoja daoLoja;
        private DateTime _dataEscolhida;
        private double _totalEmCompras;

        public PesquisarCompraDeFornecedorVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.CompraDeFornecedor> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOCompraDeFornecedor(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarCompraDeFornecedorVMStrategy();
            DataEscolhida = DateTime.Now;

            GetLojas();

            TermoPesquisa = "";
            PesquisarPor = 0;
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
            Lojas.Insert(0, new Model.Loja { Nome = "TODAS AS LOJAS" });

        }

        public int PesquisarPor
        {
            get => pesquisarPor;
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("PesquisarPor");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public Model.Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public ObservableCollection<Model.Loja> Lojas
        {
            get => _lojas;
            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
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

        public double TotalEmCompras
        {
            get => _totalEmCompras;
            set
            {
                _totalEmCompras = value;
                OnPropertyChanged("TotalEmCompras");
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public async override Task PesquisaItens(string termo)
        {
            var dao = daoEntidade as DAOCompraDeFornecedor;

            switch (pesquisarPor)
            {
                case 0: //Todos
                    Entidades = new ObservableCollection<EntidadeComCampo<Model.CompraDeFornecedor>>(EntidadeComCampo<Model.CompraDeFornecedor>.CriarListaEntidadeComCampo(await dao.ListarPorData(DataEscolhida)));
                    break;
                case 1: //Fornecedor
                    Entidades = new ObservableCollection<EntidadeComCampo<Model.CompraDeFornecedor>>(EntidadeComCampo<Model.CompraDeFornecedor>.CriarListaEntidadeComCampo(await dao.ListarPorFornecedor(TermoPesquisa, DataEscolhida)));
                    break;
                case 2: //Representante
                    Entidades = new ObservableCollection<EntidadeComCampo<Model.CompraDeFornecedor>>(EntidadeComCampo<Model.CompraDeFornecedor>.CriarListaEntidadeComCampo(await dao.ListarPorRepresentante(TermoPesquisa, DataEscolhida)));
                    break;
            }

            if (Loja?.Cnpj != null)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<Model.CompraDeFornecedor>>(Entidades.Where(w => w.Entidade.Loja == Loja));
            }

            TotalEmCompras = Entidades.Select(s => s.Entidade).Sum(sum => sum.Valor);
        }

        protected override WorksheetContainer<Model.CompraDeFornecedor>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }
    }
}
