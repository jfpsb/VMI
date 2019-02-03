﻿using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Fornecedor
    {
        public virtual string Cnpj { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string Email { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
