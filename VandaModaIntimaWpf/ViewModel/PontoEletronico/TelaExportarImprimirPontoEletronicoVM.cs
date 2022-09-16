using ACBrLib.PosPrinter;
using Microsoft.Reporting.WinForms;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.View.PontoEletronico;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class TelaExportarImprimirPontoEletronicoVM : ObservableObject
    {
        private ACBrPosPrinter _posPrinter;
        private ISession _session;
        private Model.Funcionario _funcionario;
        private IConfiguraReportViewer configuraReportViewer;
        private DateTime _dataEscolhida;
        private IMessageBoxService _messageBoxService;

        public ICommand ExportarParaPDFComando { get; set; }
        public ICommand ImprimirA4Comando { get; set; }
        public ICommand ImprimirEmCupomComando { get; set; }

        public TelaExportarImprimirPontoEletronicoVM(ISession session, ACBrPosPrinter posPrinter, Model.Funcionario funcionario)
        {
            _session = session;
            _posPrinter = posPrinter;
            Funcionario = funcionario;
            configuraReportViewer = new ConfiguraReportViewerPonto(session);
            _messageBoxService = new MessageBoxService();

            ExportarParaPDFComando = new RelayCommand(ExportarParaPDF);
            ImprimirA4Comando = new RelayCommand(ImprimirA4);
            DataEscolhida = DateTime.Now;
        }

        private void ImprimirA4(object obj)
        {
            TelaRelatorioPontosPorMes telaRelatorioPontoEletronico = new TelaRelatorioPontosPorMes(_session, Funcionario, DataEscolhida);
            telaRelatorioPontoEletronico.ShowDialog();
        }

        private async void ExportarParaPDF(object obj)
        {
            try
            {
                if (obj == null)
                    throw new Exception($"Parâmetro de comando não configurado para exportar pontos para PDF.");

                var folderBrowserDialog = obj as IFolderBrowserDialog;
                string caminhoPasta = folderBrowserDialog.OpenFolderBrowserDialog();

                ReportDataSource reportDataSource = await configuraReportViewer.ConfigurarReportDataSource(Funcionario, DataEscolhida);
                var relatorio = new ReportViewer();
                configuraReportViewer.Configurar(relatorio, reportDataSource, "VandaModaIntimaWpf.View.PontoEletronico.Relatorios.PontoEletronicoRelatorioFuncionario.rdlc");
                byte[] Bytes = relatorio.LocalReport.Render("PDF", "");
                string caminhoCompleto = Path.Combine(caminhoPasta, $"{Funcionario.Nome} - PONTOS ELETRONICOS DE {DataEscolhida.ToString("MMM-yyyy").ToUpper()}.pdf");
                using (FileStream stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                }
                _messageBoxService.Show($"Exportado com sucesso em {caminhoCompleto}");
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message);
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

        public Model.Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
    }
}
