using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Marca : ObservableObject, ICloneable, IModel
    {
        private string nome { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        private IList<ProdutoModel> produtos = new List<ProdutoModel>();

        public enum Colunas
        {
            Nome = 1
        }
        public Marca() { }

        /// <summary>
        /// Construtor para criar placeholder de Marca para comboboxes
        /// </summary>
        /// <param name="nome">SELECIONE UMA MARCA</param>
        public Marca(string nome)
        {
            this.nome = nome;
        }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
                OnPropertyChanged("Nome");
            }
        }

        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [XmlIgnore]
        public virtual IList<ProdutoModel> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public virtual string GetContextMenuHeader { get => Nome; }

        public virtual object Clone()
        {
            Marca m = new Marca();

            m.Nome = Nome;

            return m;
        }

        public virtual string[] GetColunas()
        {
            return new string[] { "Nome" };
        }

        public virtual object GetId()
        {
            return Nome;
        }
    }
}
