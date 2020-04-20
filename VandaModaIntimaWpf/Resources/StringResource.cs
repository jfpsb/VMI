using System.Windows;

namespace VandaModaIntimaWpf.Resources
{
    public static class StringResource
    {
        public static string GetString(string key)
        {
            var resource = Application.Current.Resources[key];
            return resource.ToString();
        }
    }
}
