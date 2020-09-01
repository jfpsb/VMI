using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbMetaLojaLog : CouchDbLog
    {
        public string MySqlId { get; set; }
        public Loja Loja { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Valor { get; set; }
        public CouchDbMetaLojaLog()
        {
            Tipo = "metaloja";
        }
        public override void AtribuiCampos(object o)
        {
            MetaLoja m = (MetaLoja)o;
            MySqlId = m.Id;
            Loja = m.Loja;
            Mes = m.Mes;
            Ano = m.Ano;
            Valor = m.Valor;
        }
        public override object Clone()
        {
            CouchDbMetaLojaLog log = new CouchDbMetaLojaLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Loja = Loja,
                Mes = Mes,
                Ano = Ano,
                Valor = Valor
            };

            return log;
        }
    }
}
