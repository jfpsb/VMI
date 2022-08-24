using ACBrLib.Core.PosPrinter;
using ACBrLib.PosPrinter;
using System;
using System.IO;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.Util
{
    public class ConfiguraPosPrinter
    {
        private static readonly string AppDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Intima");
        static IMessageBoxService messageBoxService;

        static ConfiguraPosPrinter()
        {
            messageBoxService = new MessageBoxService();
        }

        public static void Configurar(ACBrPosPrinter posPrinter)
        {
            try
            {
                Directory.CreateDirectory(AppDocumentsFolder);

                if (File.Exists(Path.Combine(AppDocumentsFolder, "ACBrLib.ini")))
                {
                    posPrinter.ConfigLer(Path.Combine(AppDocumentsFolder, "ACBrLib.ini"));
                }
                else
                {
                    //Se não existir arquivo de configuração da impressora, crio um novo genérico
                    posPrinter.Config.Porta = "USB";
                    posPrinter.Config.Modelo = ACBrPosPrinterModelo.ppEscPosEpson;
                    posPrinter.Config.ColunasFonteNormal = 48;
                    posPrinter.Config.EspacoEntreLinhas = 0;
                    posPrinter.Config.LinhasBuffer = 0;
                    posPrinter.Config.LinhasEntreCupons = 1;
                    posPrinter.Config.ControlePorta = true;
                    posPrinter.Config.CortaPapel = true;
                    posPrinter.Config.TraduzirTags = true;
                    posPrinter.Config.IgnorarTags = false;
                    posPrinter.Config.PaginaDeCodigo = PosPaginaCodigo.pc850;

                    posPrinter.Config.QrCodeConfig.Tipo = 2;
                    posPrinter.Config.QrCodeConfig.LarguraModulo = 6;
                    posPrinter.Config.QrCodeConfig.ErrorLevel = 0;

                    posPrinter.ConfigGravar(Path.Combine(AppDocumentsFolder, "ACBrLib.ini"));
                }
            }
            catch (Exception ex)
            {
                messageBoxService.Show("Erro ao iniciar impressora. Cheque se a impressora está conectada corretamente e que está ligada.\n\n" + ex.Message + $"\n\n{ex.InnerException?.Message}\n\n{ex.StackTrace}", "Impressão De Comprovante Pix", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
