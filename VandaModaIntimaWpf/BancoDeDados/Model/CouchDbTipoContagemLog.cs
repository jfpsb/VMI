using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbTipoContagemLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public string Nome { get; set; }
        public CouchDbTipoContagemLog()
        {
            Tipo = "tipocontagem";
        }
        public override void AtribuiCampos(object o)
        {
            TipoContagem t = (TipoContagem)o;
            MySqlId = t.Id;
            Nome = t.Nome;
        }
        public override object Clone()
        {
            CouchDbTipoContagemLog log = new CouchDbTipoContagemLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Nome = Nome
            };

            return log;
        }
    }
}
