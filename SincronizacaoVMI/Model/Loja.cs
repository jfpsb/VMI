using System.Collections.Generic;

namespace SincronizacaoVMI.Model
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
    }
}
