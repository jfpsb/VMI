using System.Windows;
using System.Windows.Media.Imaging;

namespace VandaModaIntimaWpf.Resources
{
    /// <summary>
    /// Classe para retornar resources do tipo String
    /// </summary>
    public static class GetResource
    {
        public static string GetString(string key)
        {
            var resource = Application.Current.Resources[key];
            return resource.ToString();
        }

        public static BitmapImage GetBitmapImage(string key)
        {
            var resource = Application.Current.Resources[key];
            return (BitmapImage)resource;
        }
    }
}
