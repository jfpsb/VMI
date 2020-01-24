using System;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class PesquisarRecebimentoViewModel : APesquisarViewModel<RecebimentoCartaoModel>
    {
        private DAOLoja daoLoja;
        private LojaModel matriz;
        private DateTime dataEscolhida = DateTime.Now;
        private int matrizComboBoxIndex;
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public PesquisarRecebimentoViewModel() : base("RecebimentoCartao")
        {
            daoEntidade = new DAORecebimentoCartao(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarRecebimentoCartaoViewModelStrategy();
            GetMatrizes();
            MatrizComboBoxIndex = 0;
        }
        public override async void GetItems(string termo)
        {
            DAORecebimentoCartao daoRecebimento = (DAORecebimentoCartao)daoEntidade;
            if (MatrizComboBoxIndex != 0)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.ConverterIList(await daoRecebimento.ListarPorMesAnoLojaSum(DataEscolhida.Month, DataEscolhida.Year, Matriz)));
            }
            else
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.ConverterIList(await daoRecebimento.ListarPorMesAnoSum(DataEscolhida.Month, DataEscolhida.Year)));
            }
        }

        public async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel("SELECIONE UMA MATRIZ"));
        }
        public LojaModel Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public int MatrizComboBoxIndex
        {
            get { return matrizComboBoxIndex; }
            set
            {
                matrizComboBoxIndex = value;
                OnPropertyChanged("MatrizComboBoxIndex");
            }
        }
        public DateTime DataEscolhida
        {
            get { return dataEscolhida; }
            set
            {
                dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
