namespace VandaModaIntima.Model
{
    class CodBarraFornecedor
    {
        public virtual Produto Produto { get; set; }
        public virtual string Codigo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(CodBarraFornecedor))
            {
                CodBarraFornecedor codBarraFornecedor = (CodBarraFornecedor)obj;

                if (Produto.Cod_Barra.Equals(codBarraFornecedor.Produto.Cod_Barra)
                    && Codigo.Equals(codBarraFornecedor.Codigo))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Produto.Cod_Barra.GetHashCode() + Codigo.GetHashCode();
        }
    }
}
