﻿using ACBrLib.PosPrinter;
using Gerencianet.NETCore.SDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate;
using System;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.Pix;
using VandaModaIntimaWpf.Model.Pix;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class ApresentaQRCodePixVM : ObservableObject, IViewModelClosing
    {
        private string _fantasia;
        private string _razao;
        private string _cnpj;
        private string _instituicao;
        private double _valor;
        private string _nomeLoja;
        private ImageSource _imagemQrCode;
        private Cobranca _cobranca;
        private IMessageBoxService _messageBox;
        private DAOCobranca daoCobranca;
        private double _segundosDesdeCriacao;
        private string _segundosAteExpiracaoEmString;
        private Timer timerExpiracaoQrCode;
        private Timer timerConsultaCobranca;
        private DateTime expiraEm;
        private ACBrPosPrinter posPrinter;
        private ISession _session;

        private bool _isCobrancaExpirado;
        private bool _isPagamentoEfetuado;

        public ICommand ImprimirQRCodeComando { get; set; }

        public ApresentaQRCodePixVM(ISession session, int cobrancaId, IMessageBoxService messageBoxService)
        {
            daoCobranca = new DAOCobranca(session);
            GetCobranca(cobrancaId);
            _messageBox = messageBoxService;
            _session = session;

            ImprimirQRCodeComando = new RelayCommand(ImprimirQRCode, IsQRCodeValido);

            expiraEm = Cobranca.Calendario.CriacaoLocalTime.AddSeconds(Cobranca.Calendario.Expiracao);

            Cobranca.PropertyChanged += Cobranca_PropertyChanged;

            switch (Cobranca.Status)
            {
                case "CONCLUIDA":
                    IsPagamentoEfetuado = true;
                    IsCobrancaExpirado = false;
                    PopulaDados();
                    break;
                case "ATIVA":
                    if (DateTime.Now >= expiraEm)
                    {
                        IsCobrancaExpirado = true;
                        IsPagamentoEfetuado = false;
                        PopulaDados();
                    }
                    else
                    {
                        IsPagamentoEfetuado = false;
                        IsCobrancaExpirado = false;

                        daoCobranca = new DAOCobranca(session);

                        timerExpiracaoQrCode = new Timer(1000);
                        timerExpiracaoQrCode.Elapsed += TimerExpiracaoQrCode_Elapsed;
                        timerExpiracaoQrCode.AutoReset = true;
                        timerExpiracaoQrCode.Enabled = true;

                        timerConsultaCobranca = new Timer(5000);
                        timerConsultaCobranca.Elapsed += TimerConsultaCobranca_Elapsed;
                        timerConsultaCobranca.AutoReset = true;
                        timerConsultaCobranca.Enabled = true;

                        GeraESalvaQrCode();
                    }
                    break;
            };

            posPrinter = new ACBrPosPrinter();
            ConfiguraPosPrinter.Configurar(posPrinter);
        }

        private async void GetCobranca(int cobrancaId)
        {
            try
            {
                Cobranca = await daoCobranca.ListarPorId(cobrancaId);
            }
            catch (Exception ex)
            {
                _messageBox.Show(ex.Message, "QRCode E Dados De Cobrança Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Cobranca_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Status"))
            {
                if (Cobranca.Status.Equals("CONCLUIDA"))
                {
                    IsPagamentoEfetuado = true;
                    Cobranca.PagoEm = Cobranca.Pix[0].Horario;
                    SegundosAteExpiracaoEmString = "PAGAMENTO EFETUADO";
                    timerConsultaCobranca.Stop();
                    timerConsultaCobranca.Dispose();
                    timerExpiracaoQrCode.Stop();
                    timerExpiracaoQrCode.Dispose();

                    try
                    {
                        await daoCobranca.Atualizar(Cobranca);
                        Cobranca.PropertyChanged -= Cobranca_PropertyChanged;
                        _session.Refresh(Cobranca);
                    }
                    catch (Exception ex)
                    {
                        _messageBox.Show(ex.Message, "QRCode E Dados De Cobrança Pix", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void TimerConsultaCobranca_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (e.SignalTime <= expiraEm)
            {
                timerConsultaCobranca.Stop();

                var gnEndPoints = Config.GNEndpoints(_session);

                if (gnEndPoints == null)
                {
                    _messageBox.Show($"Erro ao recuperar credenciais da GerenciaNet.\nAcesse {Log.LogCredenciais} para mais detalhes.", "Erro em Credenciais GerenciaNet", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dynamic endpoints = new Endpoints(gnEndPoints);

                var param = new
                {
                    txid = Cobranca.Txid
                };

                try
                {
                    var response = endpoints.PixDetailCharge(param);
                    Cobranca cobranca = JsonConvert.DeserializeObject<Cobranca>(response);

                    Cobranca.Revisao = cobranca.Revisao;
                    Cobranca.Location = cobranca.Location;

                    Cobranca.Pix.Clear();
                    foreach (var p in cobranca.Pix)
                    {
                        p.Cobranca = Cobranca;
                        Cobranca.Pix.Add(p);
                    }

                    Cobranca.Status = cobranca.Status;
                }
                catch (GnException gne)
                {
                    Log.EscreveLogGn(gne);
                    _messageBox.Show($"Erro ao consultar cobrança Pix na GerenciaNet.\nAcesse {Log.LogGn} para mais detalhes.", "Erro ao consultar cobrança", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    Log.EscreveExceptionGenerica(ex);
                    _messageBox.Show($"Erro ao consultar situação de cobrança Pix. Cheque sua conexão com a internet.\nAcesse {Log.LogExceptionGenerica} para mais detalhes.", "Erro ao consultar cobrança", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                try
                {
                    timerConsultaCobranca.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Timer De Consulta Foi Parado. " + ex.Message);
                }
            }
            else
            {
                IsCobrancaExpirado = true;
                IsPagamentoEfetuado = false;
                timerConsultaCobranca.Stop();
                timerConsultaCobranca.Dispose();
            }
        }

        private void TimerExpiracaoQrCode_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (e.SignalTime > expiraEm)
            {
                IsCobrancaExpirado = true;
                IsPagamentoEfetuado = false;
                SegundosDesdeCriacao = Cobranca.Calendario.Expiracao;
                SegundosAteExpiracaoEmString = "EXPIRADO";
                timerExpiracaoQrCode.Stop();
                timerExpiracaoQrCode.Dispose();
            }
            else
            {
                var horaAtual = DateTime.Now.TimeOfDay;
                var timeSpan1 = new TimeSpan(Cobranca.Calendario.CriacaoLocalTime.Hour, Cobranca.Calendario.CriacaoLocalTime.Minute, Cobranca.Calendario.CriacaoLocalTime.Second);
                var timeSpan2 = new TimeSpan(expiraEm.Hour, expiraEm.Minute, expiraEm.Second);

                if (horaAtual >= timeSpan1 && horaAtual <= timeSpan2)
                {
                    var timeSpanRestante = timeSpan2.Subtract(horaAtual);
                    SegundosAteExpiracaoEmString = timeSpanRestante.ToString(@"mm\:ss");
                    SegundosDesdeCriacao = horaAtual.Subtract(timeSpan1).TotalSeconds;
                }
            }
        }

        private bool IsQRCodeValido(object arg)
        {
            return !IsCobrancaExpirado && !IsPagamentoEfetuado;
        }

        private void ImprimirQRCode(object obj)
        {
            string s = "</zera>" + "\n";
            s += "</ce>" + "\n";
            s += "</logo>" + "\n";
            s += "<e>PAGAMENTO PIX</e>" + "\n";
            s += "</ae>" + "\n";
            s += "<n>RECEBEDOR<n>" + "\n";
            s += "<n>Nome Fantasia:</n>" + "\n";
            s += $"</fn>{Fantasia}" + "\n";
            s += "<n>Razão Social:</n>" + "\n";
            s += $"</fn>{Razao}" + "\n";
            s += "<n>CNPJ:</n>" + "\n";
            s += $"</fn>{Cnpj}" + "\n";
            s += "<n>Instituição:</n>" + "\n";
            s += $"</fn>{Instituicao}" + "\n";
            s += "<n>Loja:</n>" + "\n";
            s += $"</fn>{NomeLoja}" + "\n";
            s += $"<a><n><in>VALOR: {Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"))}</in></n></a>" + "\n";
            s += "</linha_dupla>" + "\n";
            s += "</ce>" + "\n";
            s += "<a>ESCANEIE O QR CODE COM O APLICATIVO DO SEU BANCO" + "\n";
            s += "PARA EFETUAR O PAGAMENTO</a>" + "\n";
            s += $"</fn>QR Code válido até {expiraEm.ToString(CultureInfo.CurrentCulture)}" + "\n";
            s += $"<qrcode>{Cobranca.QrCode.Qrcode}</qrcode>" + "\n";
            s += "</pular_linhas>\n";
            s += "</corte_total>" + "\n";

            try
            {
                posPrinter.Imprimir(s);
            }
            catch (Exception ex)
            {
                _messageBox.Show("Erro ao imprimir QR Code. Cheque se a impressora está conectada corretamente e que está ligada.\n\n" + ex.Message, "Impressão De QR Code Pix", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void GeraESalvaQrCode()
        {
            var gnEndPoints = Config.GNEndpoints(_session);

            if (gnEndPoints == null)
            {
                _messageBox.Show($"Erro ao recuperar credenciais da GerenciaNet.\nAcesse {Log.LogCredenciais} para mais detalhes.", "Erro em Credenciais GerenciaNet", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dynamic endpoints = new Endpoints(gnEndPoints);

            var paramQRCode = new
            {
                id = Cobranca.Loc.Id
            };

            if (Cobranca.QrCode == null)
            {
                try
                {
                    var qrCodeGn = endpoints.PixGenerateQRCode(paramQRCode);
                    QRCode qrCode = new QRCode();
                    JObject qrCodeJson = JObject.Parse(qrCodeGn);
                    qrCode.Qrcode = (string)qrCodeJson["qrcode"];
                    qrCode.ImagemQrcode = ((string)qrCodeJson["imagemQrcode"]).Replace("data:image/png;base64,", "");
                    Cobranca.QrCode = qrCode;

                    try
                    {
                        await daoCobranca.Atualizar(Cobranca);
                        PopulaDados();
                        await daoCobranca.RefreshEntidade(Cobranca);
                    }
                    catch (Exception ex)
                    {
                        _messageBox.Show(ex.Message, "QRCode E Dados De Cobrança Pix", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (GnException e)
                {
                    Log.EscreveLogGn(e);
                    _messageBox.Show($"Erro ao gerar QR Code na GerenciaNet.\nAcesse {Log.LogGn} para mais detalhes.", "Erro ao gerar QR Code", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _messageBox.Show($"Não Foi Possível Mostrar Informações De Cobrança Pix. Cheque Sua Conexão Com A Internet.\n\n{ex.Message}", "Dados De Cobrança Pix", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            else
            {
                PopulaDados();
            }
        }

        private void PopulaDados()
        {
            var dados = Config.GetDadosRecebedor(Config.LojaAplicacao(_session));

            if (!IsPagamentoEfetuado && !IsCobrancaExpirado)
                ImagemQrCode = Cobranca.QrCode.QrCodeBitmap;

            Fantasia = (string)dados["fantasia"];
            Razao = (string)dados["razaosocial"];
            Cnpj = (string)dados["cnpj"];
            Instituicao = (string)dados["instituicao"];
            NomeLoja = Config.NomeLoja();
            Valor = Cobranca.Valor.Original;
        }

        public string Fantasia
        {
            get
            {
                return _fantasia;
            }

            set
            {
                _fantasia = value;
                OnPropertyChanged("Fantasia");
            }
        }

        public string Razao
        {
            get
            {
                return _razao;
            }

            set
            {
                _razao = value;
                OnPropertyChanged("Razao");
            }
        }

        public string Cnpj
        {
            get
            {
                return _cnpj;
            }

            set
            {
                _cnpj = value;
                OnPropertyChanged("Cnpj");
            }
        }

        public string Instituicao
        {
            get
            {
                return _instituicao;
            }

            set
            {
                _instituicao = value;
                OnPropertyChanged("Instituicao");
            }
        }

        public ImageSource ImagemQrCode
        {
            get
            {
                return _imagemQrCode;
            }

            set
            {
                _imagemQrCode = value;
                OnPropertyChanged("ImagemQrCode");
            }
        }

        public double Valor
        {
            get
            {
                return _valor;
            }

            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public Cobranca Cobranca
        {
            get
            {
                return _cobranca;
            }

            set
            {
                _cobranca = value;
                OnPropertyChanged("Cobranca");
            }
        }

        public double SegundosDesdeCriacao
        {
            get
            {
                return _segundosDesdeCriacao;
            }

            set
            {
                _segundosDesdeCriacao = value;
                OnPropertyChanged("SegundosDesdeCriacao");
            }
        }

        public string SegundosAteExpiracaoEmString
        {
            get
            {
                return _segundosAteExpiracaoEmString;
            }

            set
            {
                _segundosAteExpiracaoEmString = value;
                OnPropertyChanged("SegundosAteExpiracaoEmString");
            }
        }

        public bool IsCobrancaExpirado
        {
            get
            {
                return _isCobrancaExpirado;
            }

            set
            {
                _isCobrancaExpirado = value;
                OnPropertyChanged("IsCobrancaExpirado");
            }
        }

        public bool IsPagamentoEfetuado
        {
            get
            {
                return _isPagamentoEfetuado;
            }

            set
            {
                _isPagamentoEfetuado = value;
                OnPropertyChanged("IsPagamentoEfetuado");
            }
        }

        public string NomeLoja
        {
            get
            {
                return _nomeLoja;
            }

            set
            {
                _nomeLoja = value;
                OnPropertyChanged("NomeLoja");
            }
        }

        public void OnClosing()
        {
            if (posPrinter != null)
                posPrinter.Dispose();

            if (timerConsultaCobranca != null)
            {
                timerConsultaCobranca.Stop();
                timerConsultaCobranca.Dispose();
            }
            if (timerExpiracaoQrCode != null)
            {
                timerExpiracaoQrCode.Stop();
                timerExpiracaoQrCode.Dispose();
            }
        }
    }
}
