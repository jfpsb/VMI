using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : AModel, IModel
    {
        private string _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private double _valor;
        private bool _fechada;
        private IList<Bonus> _bonus = new List<Bonus>();
        private TabelasINSS[] TabelasINSS;

        public FolhaPagamento()
        {
            var tabelasJson = File.ReadAllText("Resources/tabelas_inss.json");
            TabelasINSS = JsonConvert.DeserializeObject<TabelasINSS[]>(tabelasJson);
        }

        [JsonIgnore]
        public string GetContextMenuHeader => _mes + "/" + _ano + " - " + _funcionario.Nome;

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
            get => _mes + "/" + _ano;
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
        public double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
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
        public string Id
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
                var parcelas = listaDeParcelas.Where(w => w.MesAPagar == Mes && w.AnoAPagar == Ano);
                return parcelas.ToList();
            }
        }

        [JsonIgnore]
        public double DescontoINSS
        {
            get
            {
                TabelasINSS tabela = null;

                for (int i = TabelasINSS.Length - 1; i >= 0; i--)
                {
                    if (Ano >= TabelasINSS[i].vigencia.Year && Mes >= TabelasINSS[i].vigencia.Month)
                        tabela = TabelasINSS[i];
                }

                double desconto = 0.0;

                for (int i = 0; i < tabela.faixas.Length; i++)
                {
                    if (SalarioComHoraExtra > tabela.faixas[i])
                    {
                        desconto += tabela.faixas[i] * tabela.porcentagens[i];
                    }
                    else
                    {
                        if (i == 0)
                        {
                            desconto += SalarioComHoraExtra * tabela.porcentagens[i];
                        }
                        else
                        {
                            var diferenca = SalarioComHoraExtra - tabela.faixas[i - 1];
                            desconto += diferenca * tabela.porcentagens[i];
                        }

                        break;
                    }
                }

                return Math.Round(desconto, 2, MidpointRounding.AwayFromZero);
            }
        }

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
        private double SalarioComHoraExtra
        {
            get
            {
                var bonusHoraExtra = Bonus.Where(w => w.Descricao.StartsWith("HORA EXTRA")).ToList();
                return bonusHoraExtra.Sum(s => s.Valor) + Funcionario.Salario;
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
