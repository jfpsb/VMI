using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Loja : ObservableObject, ICloneable, IModel
    {
        private string _cnpj;
        private Loja _matriz;
        private string _nome;
        private string _telefone;
        private string _endereco;
        private string _inscricaoestadual;
        private double _aluguel;
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

        public Dictionary<string, string> DictionaryIdentifier
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

        public string Cnpj
        {
            get => _cnpj;
            set
            {
                _cnpj = value;
                OnPropertyChanged("Cnpj");
            }
        }
        public Loja Matriz
        {
            get => _matriz;
            set
            {
                _matriz = value;
                OnPropertyChanged("Matriz");
            }
        }
        public string Nome
        {
            get => _nome?.ToUpper();
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public string Telefone
        {
            get => _telefone;
            set
            {
                _telefone = value;
                OnPropertyChanged("Telefone");
            }
        }

        internal string[] GetColunas()
        {
            return new[] { "CNPJ", "Matriz", "Nome", "Telefone", "Endereço", "Inscrição Estadual" };
        }

        public string Endereco
        {
            get => _endereco?.ToUpper();
            set
            {
                _endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
        public string InscricaoEstadual
        {
            get => _inscricaoestadual;
            set
            {
                _inscricaoestadual = value;
                OnPropertyChanged("InscricaoEstadual");
            }
        }
        public double Aluguel
        {
            get => _aluguel;
            set
            {
                _aluguel = value;
                OnPropertyChanged("Aluguel");
            }
        }

        public bool IsIdentical(object obj)
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

        public string GetContextMenuHeader => Nome;

        public object Clone()
        {
            Loja loja = new Loja
            {
                Cnpj = Cnpj,
                Nome = Nome,
                Telefone = Telefone,
                Endereco = Endereco,
                InscricaoEstadual = InscricaoEstadual,
                Matriz = Matriz
            };

            return loja;
        }

        public object GetIdentifier()
        {
            return Cnpj;
        }

        public override string ToString()
        {
            return Cnpj?.ToString();
        }
    }
}
