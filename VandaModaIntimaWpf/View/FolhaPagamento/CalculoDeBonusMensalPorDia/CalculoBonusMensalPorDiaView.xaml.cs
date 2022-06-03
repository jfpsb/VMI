using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VandaModaIntimaWpf.View.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    /// <summary>
    /// Interaction logic for CalculoPassagemOnibus.xaml
    /// </summary>
    public partial class CalculoBonusMensalPorDiaView : Window
    {
        public CalculoBonusMensalPorDiaView()
        {
            InitializeComponent();
        }

        private void MenuItemHeader1_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap targetBitmap = new RenderTargetBitmap((int)GridCalendario.ActualWidth, (int)GridCalendario.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            Size size = new Size((int)GridCalendario.ActualWidth, (int)GridCalendario.ActualHeight);
            GridCalendario.Measure(size);
            GridCalendario.Arrange(new Rect(size));
            Rect bounds = VisualTreeHelper.GetDescendantBounds(GridCalendario);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(GridCalendario);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            targetBitmap.Render(dv);

            var bitmap = new TransformedBitmap(targetBitmap, new ScaleTransform(630.0 / targetBitmap.PixelWidth, 470.0 / targetBitmap.PixelHeight));

            PngBitmapEncoder pngEnconder = new PngBitmapEncoder();
            pngEnconder.Frames.Add(BitmapFrame.Create(bitmap));
            using (Stream stream = File.Create(Path.Combine(Path.GetTempPath(), "UltimoCalendario.png")))
            {
                pngEnconder.Save(stream);
            }
        }
    }
}
