using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace VandaModaIntimaWpf.Resources
{
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

        public static string[] GetStringArray(string key)
        {
            var resouce = Application.Current.Resources[key];
            return (string[])resouce;
        }
    }
}
