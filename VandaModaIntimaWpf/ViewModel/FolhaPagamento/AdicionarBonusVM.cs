using NHibernate;
using System;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusVM : ACadastrarViewModel<Bonus>
    {
        private FolhaModel _folha;
        private string _valor;
        private DateTime _inicioPagamento;
        private string[] _cmbDescricaoHoraExtra;
        private int _descricaoHoraExtra;
        private int _quantHoras;
        private int _quantMinutos;
        private DateTime dataEscolhida;
        private double valorHora;

        public AdicionarBonusVM(ISession session, FolhaModel folha, DateTime dataEscolhida, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOBonus(session);
            viewModelStrategy = new CadastrarBonusVMStrategy();
            Folha = folha;
            CmbDescricaoHoraExtra = Application.Current.Resources["CmbDescricaoHoraExtra"] as string[];
            this.dataEscolhida = dataEscolhida;

            AntesDeInserirNoBancoDeDados += ConfiguraBonus;

            Entidade = new Bonus()
            {
                Funcionario = folha.Funcionario
            };

            PropertyChanged += AdicionarBonusVM_PropertyChanged;

            valorHora = Folha.Funcionario.Salario / 220;
            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);
        }

        private void AdicionarBonusVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "InicioPagamento":
                    Entidade.MesReferencia = InicioPagamento.Month;
                    Entidade.AnoReferencia = InicioPagamento.Year;
                    break;

                case "DescricaoHoraExtra":
                    CalculaValor(CalculaHorasTotal());
                    break;

                case "QuantHoras":
                    CalculaValor(CalculaHorasTotal());
                    break;

                case "QuantMinutos":
                    CalculaValor(CalculaHorasTotal());
                    break;
            }
        }

        private double CalculaHorasTotal()
        {
            if (QuantHoras < 0)
            {
                QuantHoras = 0;
            }
            else if (QuantHoras > 99)
            {
                QuantHoras = 99;
            }

            if (QuantMinutos > 59)
            {
                QuantMinutos = 59;
            }
            else if (QuantMinutos < 0)
            {
                QuantMinutos = 0;
            }

            return Convert.ToDouble(QuantHoras) + (Convert.ToDouble(QuantMinutos) / 60);
        }

        /// <summary>
        /// Calcula o valor da hora extra e configura a descrição do bônus
        /// </summary>
        /// <param name="quantHorasTotal"></param>
        private void CalculaValor(double quantHorasTotal)
        {
            //Hora Extra C 100%
            if (DescricaoHoraExtra == 0)
            {
                Entidade.Descricao = CmbDescricaoHoraExtra[0];
                Valor = Math.Round(quantHorasTotal * valorHora * 2, 2, MidpointRounding.AwayFromZero).ToString();
            }
            //Hora Extra C 55%
            else
            {
                Entidade.Descricao = CmbDescricaoHoraExtra[1];
                Valor = Math.Round(quantHorasTotal * valorHora * 1.55, 2, MidpointRounding.AwayFromZero).ToString();
            }
        }

        private void ConfiguraBonus()
        {
            Entidade.Data = DateTime.Now;
            Entidade.BaseCalculo = Folha.Funcionario.Salario;
        }

        public override void ResetaPropriedades()
        {
            QuantHoras = 0;
            QuantMinutos = 0;

            Entidade = new Bonus
            {
                Funcionario = Folha.Funcionario
            };

            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            double valor;

            if (string.IsNullOrEmpty(Entidade.Descricao) || !double.TryParse(Valor, out valor))
                return false;

            Entidade.Valor = valor;

            return true;
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public string Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public DateTime InicioPagamento
        {
            get => _inicioPagamento;
            set
            {
                _inicioPagamento = value;
                OnPropertyChanged("InicioPagamento");
            }
        }

        public FolhaModel Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
            }
        }

        public string[] CmbDescricaoHoraExtra
        {
            get => _cmbDescricaoHoraExtra;
            set
            {
                _cmbDescricaoHoraExtra = value;
                OnPropertyChanged("CmbDescricaoHoraExtra");
            }
        }
        public int DescricaoHoraExtra
        {
            get => _descricaoHoraExtra;
            set
            {
                _descricaoHoraExtra = value;
                OnPropertyChanged("DescricaoHoraExtra");
            }
        }

        public int QuantHoras
        {
            get => _quantHoras;
            set
            {
                _quantHoras = value;
                OnPropertyChanged("QuantHoras");
            }
        }
        public int QuantMinutos
        {
            get => _quantMinutos;
            set
            {
                _quantMinutos = value;
                OnPropertyChanged("QuantMinutos");
            }
        }
    }
}
