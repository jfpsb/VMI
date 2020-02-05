﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Fornecedor
    {
        private string cnpj { get; set; }
        private string nome { get; set; }
        private string fantasia { get; set; }
        private string email { get; set; }
        private string telefone { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;

        public virtual string Cnpj
        {
            get { return cnpj; }
            set
            {
                if (value != null)
                {
                    cnpj = Regex.Replace(value, @"[-./]", "");
                }
            }
        }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                if (value != null)
                {
                    nome = value.ToUpper();
                }
            }
        }

        public virtual string Fantasia
        {
            get { return fantasia; }
            set
            {
                if (value != null)
                {
                    fantasia = value.ToUpper();
                }
            }
        }

        public virtual string Email
        {
            get { return email; }
            set
            {
                email = value;
            }
        }
        public virtual string Telefone
        {
            get { return telefone; }
            set
            {
                telefone = value;
            }
        }

        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
            }
        }
    }
}
