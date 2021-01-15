using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : AModel, IModel
    {
        private string _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private bool _fechada;
        private IList<Bonus> _bonus = new List<Bonus>();
        private TabelasINSS[] tabelasINSS;
        private TabelasINSS _tabelaINSS;

        public FolhaPagamento()
        {
            var tabelasJson = File.ReadAllText("Resources/tabelas_inss.json");
            tabelasINSS = JsonConvert.DeserializeObject<TabelasINSS[]>(tabelasJson);

            PropertyChanged += FolhaPagamento_PropertyChanged;
        }

        private void FolhaPagamento_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Mes") || e.PropertyName.Equals("Ano"))
            {
                DeterminaTabelaINSS();
            }
        }

        private void DeterminaTabelaINSS()
        {
            for (int i = tabelasINSS.Length - 1; i >= 0; i--)
            {
                if (Ano >= tabelasINSS[i].Vigencia.Year && Mes >= tabelasINSS[i].Vigencia.Month)
                    _tabelaINSS = tabelasINSS[i];
            }
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
        public double ValorATransferir
        {
            get
            {
                var atransferir = Funcionario.Salario + TotalBonus - TotalAdiantamentos - DescontoINSS;
                return Math.Round(atransferir, 2, MidpointRounding.AwayFromZero);
            }
        }
        public double TotalAdiantamentos
        {
            get
            {
                return Math.Round(Parcelas.Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);
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
                var parcelas = listaDeParcelas.Where(w => w.Mes == Mes && w.Ano == Ano);
                return parcelas.ToList();
            }
        }

        [JsonIgnore]
        public double DescontoINSS
        {
            get
            {
                double desconto = 0.0;

                if (Funcionario.DescontoINSS)
                {
                    for (int i = 0; i < TabelaINSS.Faixas.Length; i++)
                    {
                        if (SalarioComHoraExtra > TabelaINSS.Faixas[i])
                        {
                            desconto += TabelaINSS.Faixas[i] * TabelaINSS.Porcentagens[i];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                desconto += SalarioComHoraExtra * TabelaINSS.Porcentagens[i];
                            }
                            else
                            {
                                var diferenca = SalarioComHoraExtra - TabelaINSS.Faixas[i - 1];
                                desconto += diferenca * TabelaINSS.Porcentagens[i];
                            }

                            break;
                        }
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
        public double TotalBonus
        {
            get
            {
                return Math.Round(Bonus.Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);
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

        public TabelasINSS TabelaINSS
        {
            get => _tabelaINSS;
            set
            {
                _tabelaINSS = value;
                OnPropertyChanged("TabelaINSS");
            }
        }

        public DateTime Vencimento
        {
            get
            {
                DateTime mesSeguinteFolha = new DateTime(Ano, Mes, 5).AddMonths(1);
                DateTime quintoDiaUtil = mesSeguinteFolha;

                if (!File.Exists($"Resources/Feriados/{mesSeguinteFolha.Year}.json"))
                {
                    try
                    {
                        //TODO: Colocar essa consulta do arquivo de feriados em uma classe estática
                        string url = string.Format("https://api.calendario.com.br/?json=true&ano={0}&estado=MA&cidade=SAO_LUIS&token=amZwc2JfZmVsaXBlMkBob3RtYWlsLmNvbSZoYXNoPTE1NDcxMDY0NA", mesSeguinteFolha.Year);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            File.WriteAllText($"Resources/Feriados/{mesSeguinteFolha.Year}.json", reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{mesSeguinteFolha.Year}.json");
                var datasFeriados = JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);

                int quintoFlag = 0;

                foreach (var dia in AllDatesInMonth(mesSeguinteFolha.Year, mesSeguinteFolha.Month))
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

        //TODO: Colocar esse método de retornar todos os dias de um mês em classe estática
        private IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
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
