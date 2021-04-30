using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : AModel, IModel
    {
        private long _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private bool _fechada;
        private double _salarioLiquido;
        private string _observacao;
        private IList<Bonus> _bonus = new List<Bonus>();

        public FolhaPagamento()
        {
            var tabelasJson = File.ReadAllText("Resources/tabelas_inss.json");
            //tabelasINSS = JsonConvert.DeserializeObject<TabelasINSS[]>(tabelasJson);

            PropertyChanged += FolhaPagamento_PropertyChanged;
        }

        private void FolhaPagamento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("Mes") || e.PropertyName.Equals("Ano"))
            //{
            //    DeterminaTabelaINSS();
            //}
        }

        //private void DeterminaTabelaINSS()
        //{
        //    for (int i = tabelasINSS.Length - 1; i >= 0; i--)
        //    {
        //        if (Ano >= tabelasINSS[i].Vigencia.Year && Mes >= tabelasINSS[i].Vigencia.Month)
        //            _tabelaINSS = tabelasINSS[i];
        //    }
        //}

        [JsonIgnore]
        public string GetContextMenuHeader => $"{MesReferencia} - {_funcionario.Nome}";

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id.ToString() }
                };

                return dic;
            }
        }

        public int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
            }
        }

        [JsonIgnore]
        public string MesReferencia
        {
            get => new DateTime(Ano, Mes, 1).ToString("MM/yyyy");
        }
        public Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public double ValorATransferir
        {
            get
            {
                var atransferir = SalarioLiquido + TotalBonus - TotalAdiantamentos;
                return atransferir;
            }
        }
        public double TotalAdiantamentos
        {
            get
            {
                return Parcelas.Sum(s => s.Valor);
            }
        }
        public bool Fechada
        {
            get => _fechada;
            set
            {
                _fechada = value;
                OnPropertyChanged("Fechada");
            }
        }

        [JsonProperty(PropertyName = "MySqlId")]
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonIgnore]
        public IList<Parcela> Parcelas
        {
            get
            {
                var listaDeParcelas = Funcionario.Adiantamentos.SelectMany(s => s.Parcelas);
                var parcelas = listaDeParcelas.Where(w => w.Mes == Mes && w.Ano == Ano);
                return parcelas.ToList();
            }
        }

        //[JsonIgnore]
        //public double DescontoINSS
        //{
        //    get
        //    {
        //        double desconto = 0.0;

        //        for (int i = 0; i < TabelaINSS.Faixas.Length; i++)
        //        {
        //            if (SalarioComHoraExtra > TabelaINSS.Faixas[i])
        //            {
        //                desconto += TabelaINSS.Faixas[i] * TabelaINSS.Porcentagens[i];
        //            }
        //            else
        //            {
        //                if (i == 0)
        //                {
        //                    desconto += SalarioComHoraExtra * TabelaINSS.Porcentagens[i];
        //                }
        //                else
        //                {
        //                    var diferenca = SalarioComHoraExtra - TabelaINSS.Faixas[i - 1];
        //                    desconto += diferenca * TabelaINSS.Porcentagens[i];
        //                }

        //                break;
        //            }
        //        }

        //        return desconto;
        //    }
        //}

        [JsonIgnore]
        public IList<Bonus> Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        [JsonIgnore]
        public double TotalBonus
        {
            get
            {
                return Bonus.Sum(s => s.Valor);
            }
        }

        //public TabelasINSS TabelaINSS
        //{
        //    get => _tabelaINSS;
        //    set
        //    {
        //        _tabelaINSS = value;
        //        OnPropertyChanged("TabelaINSS");
        //    }
        //}

        public DateTime Vencimento
        {
            get
            {
                DateTime mesSeguinteFolha = new DateTime(Ano, Mes, 5).AddMonths(1);
                DateTime quintoDiaUtil = mesSeguinteFolha;
                var datasFeriados = FeriadoJsonUtil.RetornaListagemDeFeriados(mesSeguinteFolha.Year);

                int quintoFlag = 0;

                foreach (var dia in DateTimeUtil.RetornaDiasEmMes(mesSeguinteFolha.Year, mesSeguinteFolha.Month))
                {
                    if (dia.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    var feriado = datasFeriados.FirstOrDefault(s => s.Date.Day == dia.Day && s.Date.Month == dia.Month);

                    if (feriado != null)
                    {
                        if (feriado.Type.ToLower().Equals("feriado nacional") || feriado.Type.ToLower().Equals("feriado estadual") || feriado.Type.ToLower().Equals("feriado municipal"))
                            continue;
                    }

                    quintoFlag++;

                    if (quintoFlag == 5)
                    {
                        quintoDiaUtil = dia;
                    }
                }

                return quintoDiaUtil;
            }
        }

        public double SalarioLiquido
        {
            get => _salarioLiquido;
            set
            {
                _salarioLiquido = value;
                OnPropertyChanged("SalarioLiquido");
            }
        }

        public string Observacao
        {
            get => _observacao;
            set
            {
                _observacao = value;
                OnPropertyChanged("Observacao");
            }
        }

        public object GetIdentifier()
        {
            return _id;
        }

        public void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Funcionario))
            {
                NHibernateUtil.Initialize(Funcionario);
            }
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public string CouchDbId()
        {
            return Id.ToString();
        }
    }
}
