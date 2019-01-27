using System;

namespace VandaModaIntima.Model
{
    class Produto
    {
        public virtual string Cod_Barra { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual string Descricao { get; set; }
        public virtual double Preco { get; set; }
    }
}
