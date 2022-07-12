using Newtonsoft.Json;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace VandaModaIntimaWpf.Model
{
    public class Loja : AModel, IModel
    {
        private string _cnpj;
        private Loja _matriz;
        private string _nome;
        private string _telefone;
        private string _endereco;
        private string _inscricaoestadual;
        private double _aluguel;
        private string _razaoSocial;
        private IList<AliquotasImposto> _aliquotas = new List<AliquotasImposto>();

        public Loja() { }
        public Loja(string nome)
        {
            _nome = nome;
        }

        public enum Colunas
        {
            Cnpj = 1,
            Matriz = 2,
            Nome = 3,
            Telefone = 4,
            Endereco = 5,
            InscricaoEstadual = 6,
            Aluguel = 7
        }

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Cnpj", Cnpj }
                };

                return dic;
            }
        }

        public virtual string Cnpj
        {
            get => _cnpj;
            set
            {
                _cnpj = value;
                OnPropertyChanged("Cnpj");
            }
        }
        public virtual Loja Matriz
        {
            get => _matriz;
            set
            {
                _matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
        public virtual string Nome
        {
            get => _nome?.ToUpper();
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual string Telefone
        {
            get => _telefone;
            set
            {
                _telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        public virtual string Endereco
        {
            get => _endereco?.ToUpper();
            set
            {
                _endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public virtual string InscricaoEstadual
        {
            get => _inscricaoestadual;
            set
            {
                _inscricaoestadual = value;
                OnPropertyChanged("InscricaoEstadual");
            }
        }
        public virtual double Aluguel
        {
            get => _aluguel;
            set
            {
                _aluguel = value;
                OnPropertyChanged("Aluguel");
            }
        }

        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Loja))
            {
                Loja loja = (Loja)obj;

                return loja.Cnpj.Equals(Cnpj)
                       && loja.Matriz.Equals(Matriz)
                       && loja.Nome.Equals(Nome)
                       && loja.Telefone.Equals(Telefone)
                       && loja.Endereco.Equals(Endereco)
                       && loja.InscricaoEstadual.Equals(InscricaoEstadual);
            }

            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Nome;

        public virtual IList<AliquotasImposto> Aliquotas
        {
            get => _aliquotas;
            set
            {
                _aliquotas = value;
                OnPropertyChanged("Aliquotas");
            }
        }

        public virtual AliquotasImposto UltimaAliquota
        {
            get => Aliquotas.Count > 0 ? Aliquotas.Last() : null;
        }

        public virtual string RazaoSocial
        {
            get
            {
                return _razaoSocial;
            }

            set
            {
                _razaoSocial = value;
                OnPropertyChanged("RazaoSocial");
            }
        }

        public virtual object GetIdentifier()
        {
            return Cnpj;
        }

        public virtual void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Matriz))
            {
                NHibernateUtil.Initialize(Matriz);
            }
            if (!NHibernateUtil.IsInitialized(Aliquotas))
            {
                NHibernateUtil.Initialize(Aliquotas);
            }
        }
    }
}
