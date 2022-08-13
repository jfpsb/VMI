using Newtonsoft.Json.Linq;
using NHibernate;
using System;
using System.IO;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Util
{
    public static class GetLojaAplicacao
    {
        public static readonly string AppDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima");
        public static Loja LojaAplicacao(ISession session)
        {
            var configJson = File.ReadAllText(Path.Combine(AppDocumentsFolder, "Config.json"));
            JObject json = JObject.Parse(configJson);
            var cnpj = json["loja"]["cnpj"].ToString();
            return session.Get<Model.Loja>(cnpj);
        }
    }
}
