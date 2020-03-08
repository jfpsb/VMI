using FinancerData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using OperadoraCartaoModel = VandaModaIntimaWpf.Model.OperadoraCartao;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class CadastrarRecebimentoCartaoViewModel : ACadastrarViewModel
    {
        private DAO daoRecebimentoCartao;
        private DAO daoOperadoraCartao;
        private DAOLoja daoLoja;
        private int matrizComboBoxIndex;
        public LojaModel Matriz { get; set; }
        public DateTime DataEscolhida { get; set; }
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        private ObservableCollection<RecebimentoCartaoModel> recebimentos = new ObservableCollection<RecebimentoCartaoModel>();
        private double totalRecebido;
        private double totalOperadora;

        public ICommand AbrirOfxComando { get; set; }
        public CadastrarRecebimentoCartaoViewModel() : base("RecebimentoCartao")
        {
            daoRecebimentoCartao = new DAORecebimentoCartao(_session);
            daoOperadoraCartao = new DAOOperadoraCartao(_session);
            daoLoja = new DAOLoja(_session);

            AbrirOfxComando = new RelayCommand(AbrirOfx, ValidaAbrirOfx);

            PropertyChanged += CadastrarViewModel_PropertyChanged;
            Recebimentos.CollectionChanged += RecebimentosChanged;

            GetMatrizes();
            DataEscolhida = DateTime.Now;
        }
        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ValorOperadora":
                    // Calcula os totais quando mudo o valor na célula do DataGrid
                    CalculaTotais();
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            DataEscolhida = DateTime.Now;
            Recebimentos.Clear();
            Recebimentos = new ObservableCollection<RecebimentoCartaoModel>();
            MatrizComboBoxIndex = 0;
        }

        public override async void Salvar(object parameter)
        {
            var result = await daoRecebimentoCartao.Inserir(Recebimentos);

            if (result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Recebimento Cadastrado Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Recebimento");
        }

        public override bool ValidaModel(object parameter)
        {
            if (MatrizComboBoxIndex == 0 || Recebimentos.Count == 0)
            {
                return false;
            }

            return true;
        }
        private bool ValidaAbrirOfx(object parameter)
        {
            if (MatrizComboBoxIndex == 0)
            {
                return false;
            }

            return true;
        }
        public async void AbrirOfx(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivo OFX (*.ofx)|*.ofx";

            if (openFileDialog.ShowDialog() == true)
            {
                var caminho = openFileDialog.FileName;
                XElement doc = ImportOfx.toXElement(caminho);
                IList<OperadoraCartaoModel> operadoras = await daoOperadoraCartao.Listar<OperadoraCartaoModel>();
                Dictionary<OperadoraCartaoModel, double> recebimentoPorOperadora = new Dictionary<OperadoraCartaoModel, double>();

                foreach (var transacao in doc.Descendants("STMTTRN"))
                {
                    if (transacao.Element("TRNTYPE").Value == "CREDIT")
                    {
                        string memo = transacao.Element("MEMO").Value;

                        foreach (OperadoraCartaoModel operadora in operadoras)
                        {
                            bool contemId = false;

                            foreach (string id in operadora.IdentificadoresBanco)
                            {
                                if (memo.Contains(id))
                                {
                                    contemId = true;
                                    break;
                                }
                            }

                            if (contemId)
                            {
                                double valor = double.Parse(transacao.Element("TRNAMT").Value.Replace('.', ','));

                                if (recebimentoPorOperadora.ContainsKey(operadora))
                                {
                                    recebimentoPorOperadora[operadora] += valor;
                                }
                                else
                                {
                                    recebimentoPorOperadora.Add(operadora, valor);
                                }

                                break;
                            }
                        }
                    }
                }

                Recebimentos.Clear();

                foreach (var rpo in recebimentoPorOperadora)
                {
                    RecebimentoCartaoModel recebimento = new RecebimentoCartaoModel();

                    recebimento.Mes = DataEscolhida.Month;
                    recebimento.Ano = DataEscolhida.Year;
                    recebimento.Loja = Matriz;
                    recebimento.OperadoraCartao = rpo.Key;
                    recebimento.Recebido = rpo.Value;

                    recebimento.PropertyChanged += CadastrarViewModel_PropertyChanged;

                    Recebimentos.Add(recebimento);
                }
            }
        }
        public double TotalRecebido
        {
            get { return Math.Round(totalRecebido, 2, MidpointRounding.AwayFromZero); }
            set
            {
                totalRecebido = value;
                OnPropertyChanged("TotalRecebido");
            }
        }
        public double TotalOperadora
        {
            get { return Math.Round(totalOperadora, 2, MidpointRounding.AwayFromZero); }
            set
            {
                totalOperadora = value;
                OnPropertyChanged("TotalOperadora");
            }
        }
        public ObservableCollection<RecebimentoCartaoModel> Recebimentos
        {
            get { return recebimentos; }
            set
            {
                recebimentos = value;
                OnPropertyChanged("Recebimentos");
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
        public async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel("SELECIONE UMA MATRIZ"));
        }
        private void CalculaTotais()
        {
            double totalRecebido = 0;
            double totalOperadora = 0;

            foreach (RecebimentoCartaoModel recebimento in Recebimentos)
            {
                totalRecebido += recebimento.Recebido;
                totalOperadora += recebimento.ValorOperadora;
            }

            TotalOperadora = totalOperadora;
            TotalRecebido = totalRecebido;
        }
        private void RecebimentosChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Calcula os totais quando os recebimentos são inseridos no DataGrid pela primeira vez
            CalculaTotais();
        }
    }
}
