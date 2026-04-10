using System;
using System.Collections.Generic;
using System.Text;

namespace VandaModaIntimaWpf.Model
{
    public class Provisionamento : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private int _ano;
        private int _mes;
        private double _ultimaRemuneracao;
        private double _avisoPrevio;
        private double _decimoTerceiro;
        private double _multaFgts;

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }

        public virtual int Ano
        {
            get
            {
                return _ano;
            }

            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
            }
        }

        public virtual int Mes
        {
            get
            {
                return _mes;
            }

            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }

        public virtual double UltimaRemuneracao
        {
            get
            {
                return _ultimaRemuneracao;
            }

            set
            {
                _ultimaRemuneracao = value;
                OnPropertyChanged("UltimaRemuneracao");
            }
        }

        public virtual double AvisoPrevio
        {
            get
            {
                return _avisoPrevio;
            }

            set
            {
                _avisoPrevio = value;
                OnPropertyChanged("AvisoPrevio");
            }
        }

        public virtual double DecimoTerceiro
        {
            get
            {
                return _decimoTerceiro;
            }

            set
            {
                _decimoTerceiro = value;
                OnPropertyChanged("DecimoTerceiro");
            }
        }

        public virtual double MultaFgts
        {
            get
            {
                return _multaFgts;
            }

            set
            {
                _multaFgts = value;
                OnPropertyChanged("MultaFgts");
            }
        }

        public virtual double ProvisionamentoTotal
        {
            get
            {
                return MultaFgts + DecimoTerceiro + AvisoPrevio;
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}
