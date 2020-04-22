using System.Windows;

namespace VandaModaIntimaWpf.Resources
{
    /// <summary>
    /// Classe para retornar resources do tipo String
    /// </summary>
    public static class StringResource
    {
        public static string GetString(string key)
        {
            var resource = Application.Current.Resources[key];
            return resource.ToString();
        }
    }
}
