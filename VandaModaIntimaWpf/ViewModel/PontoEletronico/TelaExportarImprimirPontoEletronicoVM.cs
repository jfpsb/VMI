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
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
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
        private DAOPontoEletronico daoPonto;

        public ICommand ExportarParaPDFComando { get; set; }
        public ICommand ImprimirA4Comando { get; set; }
        public ICommand ImprimirEmCupomComando { get; set; }

        public TelaExportarImprimirPontoEletronicoVM(ISession session, ACBrPosPrinter posPrinter, Model.Funcionario funcionario)
        {
            _session = session;
            _posPrinter = posPrinter;
            _messageBoxService = new MessageBoxService();
            Funcionario = funcionario;
            configuraReportViewer = new ConfiguraReportViewerPonto(session);
            daoPonto = new DAOPontoEletronico(session);

            ExportarParaPDFComando = new RelayCommand(ExportarParaPDF);
            ImprimirA4Comando = new RelayCommand(ImprimirA4);
            ImprimirEmCupomComando = new RelayCommand(ImprimirEmCupom);
            DataEscolhida = DateTime.Now;

            ConfiguraPosPrinter.Configurar(_posPrinter);
        }

        private async void ImprimirEmCupom(object obj)
        {
            var pontos = await daoPonto.ListarPontosTotaisDoMes(Funcionario, DataEscolhida);
            string texto = "</zera>\n";
            texto += "</ce>\n";
            texto += "</logo>\n";
            texto += "<e>RELATÓRIO MENSAL DE PONTOS</e>\n";
            texto += "</ae>" + "\n";
            texto += "<i><n>EMPRESA</i>\n";
            texto += $"CNPJ</n>: {Funcionario.Loja.Cnpj}\n";
            texto += $"<n>Razão Social</n>: {Funcionario.Loja.RazaoSocial}\n";
            texto += $"<n>Endereço</n>: {Funcionario.Loja.Endereco}\n";
            texto += "<i><n>FUNCIONÁRIO</i>\n";
            texto += $"CPF</n>: {Funcionario.Cpf}\n";
            texto += $"<n>Nome</n>: {Funcionario.Nome}\n";
            texto += "</linha_dupla>\n";
            texto += "</ce><e>LISTAGEM DE REGISTROS</e>\n";

            string dia = "Dia";
            string entrada = "Entrada";
            string saida = "Saída";
            string intervalos = "Intervalos";

            texto += string.Concat("<n>", dia, new string(' ', 3), entrada, new string(' ', 3), saida, new string(' ', 3), intervalos, "</n>", "\n");

            foreach (var ponto in pontos)
            {
                texto += string.Concat(ponto.Dia.ToString("dd/MM/yyyy"), new string(' ', 3));

                if (ponto.Entrada == null)
                {
                    texto += string.Concat("-- : --", new string(' ', 3));
                }
                else
                {
                    texto += string.Concat(ponto.Entrada.Value.ToString("HH:mm"), new string(' ', 3));
                }

                if (ponto.Saida == null)
                {
                    texto += string.Concat("-- : --", new string(' ', 3));
                }
                else
                {
                    texto += string.Concat(ponto.Saida.Value.ToString("HH:mm"), new string(' ', 3));
                }

                texto += ponto.IntervalosEmString + "\n";
            }

            texto += "</linha_dupla>\n";
            texto += "</pular_linhas>\n";
            texto += "</corte_total>" + "\n";

            try
            {
                _posPrinter.Imprimir(texto);
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Erro ao imprimir comprovante. Cheque se a impressora está conectada corretamente e que está ligada.\n\n" + ex.Message, "Impressão De Registro De Pontos Mensal", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
