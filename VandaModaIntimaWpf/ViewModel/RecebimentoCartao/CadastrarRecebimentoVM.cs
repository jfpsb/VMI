﻿using FinancerData;
using Microsoft.Win32;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class CadastrarRecebimentoVM : ACadastrarViewModel<Model.RecebimentoCartao>
    {
        private DAO<Model.OperadoraCartao> daoOperadoraCartao;
        private DAO<Model.Banco> daoBanco;
        private DAOLoja daoLoja;
        private int matrizComboBoxIndex;
        public ObservableCollection<Model.Loja> Matrizes { get; set; }
        public ObservableCollection<Model.Banco> Bancos { get; set; }
        private ObservableCollection<Model.RecebimentoCartao> recebimentos = new ObservableCollection<Model.RecebimentoCartao>();
        private double totalRecebido;
        private double totalOperadora;
        private Model.Loja matriz;
        private Model.Banco banco;
        private DateTime dataEscolhida;

        public ICommand AbrirOfxComando { get; set; }
        public CadastrarRecebimentoVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarRecebimentoVMStrategy();
            daoEntidade = new DAORecebimentoCartao(session);
            daoOperadoraCartao = new DAO<Model.OperadoraCartao>(session);
            daoBanco = new DAO<Model.Banco>(session);
            daoLoja = new DAOLoja(session);

            AbrirOfxComando = new RelayCommand(AbrirOfx, ValidaAbrirOfx);

            Recebimentos.CollectionChanged += RecebimentosChanged;

            GetMatrizes();
            GetBancos();

            DataEscolhida = DateTime.Now;

            PropertyChanged += PesquisaRecebimentosExistentes;
        }

        /// <summary>
        /// Pesquisa se já existem recebimentos com os parâmetros (ano, mês, loja, banco) selecionados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PesquisaRecebimentosExistentes(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DataEscolhida") || e.PropertyName.Equals("Matriz") || e.PropertyName.Equals("Banco"))
            {
                if (Matriz.Cnpj != null)
                    Recebimentos = new ObservableCollection<Model.RecebimentoCartao>(await (daoEntidade as DAORecebimentoCartao).ListarPorMesAnoLojaBanco(DataEscolhida.Month, DataEscolhida.Year, Matriz, Banco));
            }
        }

        public override void ResetaPropriedades()
        {
            Recebimentos.Clear();
            Recebimentos = new ObservableCollection<Model.RecebimentoCartao>();
            MatrizComboBoxIndex = 0;
        }

        protected async override Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = await daoEntidade.InserirOuAtualizar(Recebimentos);

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = false,
                MensagemSucesso = viewModelStrategy.MensagemEntidadeSalvaComSucesso(),
                MensagemErro = viewModelStrategy.MensagemEntidadeErroAoSalvar(),
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (MatrizComboBoxIndex == 0 || Recebimentos.Count == 0)
            {
                BtnSalvarToolTip += "Escolha Uma Matriz Ou Insira Recebimentos Na Lista!";
                valido = false;
            }

            return valido;
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
                IList<Model.OperadoraCartao> operadoras = await daoOperadoraCartao.Listar();
                Dictionary<Model.OperadoraCartao, double> recebimentoPorOperadora = new Dictionary<Model.OperadoraCartao, double>();

                foreach (var transacao in doc.Descendants("STMTTRN"))
                {
                    if (transacao.Element("TRNTYPE").Value == "CREDIT")
                    {
                        string memo = transacao.Element("MEMO").Value;

                        foreach (Model.OperadoraCartao operadora in operadoras)
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

                if (Recebimentos.Count > 0)
                {
                    await daoEntidade.Deletar(Recebimentos);
                }

                Recebimentos.Clear();

                foreach (var rpo in recebimentoPorOperadora)
                {
                    Model.RecebimentoCartao recebimento = new Model.RecebimentoCartao();

                    recebimento.Mes = DataEscolhida.Month;
                    recebimento.Ano = DataEscolhida.Year;
                    recebimento.Banco = Banco;
                    recebimento.Loja = Matriz;
                    recebimento.OperadoraCartao = rpo.Key;
                    recebimento.Recebido = rpo.Value;

                    recebimento.PropertyChanged += ChecaPropriedadesRecebimento;

                    Recebimentos.Add(recebimento);
                }
            }
        }
        public double TotalRecebido
        {
            get { return totalRecebido; }
            set
            {
                totalRecebido = value;
                OnPropertyChanged("TotalRecebido");
            }
        }
        public double TotalOperadora
        {
            get { return totalOperadora; }
            set
            {
                totalOperadora = value;
                OnPropertyChanged("TotalOperadora");
            }
        }
        public ObservableCollection<Model.RecebimentoCartao> Recebimentos
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
        public Model.Loja Matriz
        {
            get => matriz;
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
        public Model.Banco Banco
        {
            get => banco;
            set
            {
                banco = value;
                OnPropertyChanged("Banco");
            }
        }
        public DateTime DataEscolhida
        {
            get => dataEscolhida;
            set
            {
                dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }
        public async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<Model.Loja>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new Model.Loja(GetResource.GetString("matriz_nao_selecionada")));
        }
        public async void GetBancos()
        {
            Bancos = new ObservableCollection<Model.Banco>(await daoBanco.Listar());
            Banco = Bancos[0];
        }
        private void CalculaTotais()
        {
            double totalRecebido = 0;
            double totalOperadora = 0;

            foreach (Model.RecebimentoCartao recebimento in Recebimentos)
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
        public void ChecaPropriedadesRecebimento(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ValorOperadora":
                    // Calcula os totais quando mudo o valor na célula do DataGrid
                    CalculaTotais();
                    break;
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
