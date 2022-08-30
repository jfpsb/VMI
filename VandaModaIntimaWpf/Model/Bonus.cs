using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Bonus : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private Loja _lojaTrabalho;
        private string _descricao;
        private double _valor;
        private int _mesReferencia;
        private int _anoReferencia;
        private bool _bonusMensal;
        private bool _bonusCancelado;
        private bool _pagoEmFolha = true;

        [JsonIgnore]
        public virtual string GetContextMenuHeader => string.Format("R$ {0}", Valor);

        public virtual Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id.ToString() }
                };
                return dic;
            }
        }

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public virtual string Descricao
        {
            get => _descricao?.ToUpper();
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }
        public virtual double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        [JsonIgnore]
        public virtual string DataString
        {
            get => Data.ToString("G");
        }
        public virtual int MesReferencia
        {
            get => _mesReferencia;
            set
            {
                _mesReferencia = value;
                OnPropertyChanged("MesReferencia");
            }
        }
        public virtual int AnoReferencia
        {
            get => _anoReferencia;
            set
            {
                _anoReferencia = value;
                OnPropertyChanged("AnoReferencia");
            }
        }
        public virtual bool BonusCancelado
        {
            get => _bonusCancelado;
            set
            {
                _bonusCancelado = value;
                OnPropertyChanged("BonusCancelado");
            }
        }

        public virtual bool BonusMensal
        {
            get => _bonusMensal;
            set
            {
                _bonusMensal = value;
                OnPropertyChanged("BonusMensal");
            }
        }

        public virtual Loja LojaTrabalho
        {
            get => _lojaTrabalho;
            set
            {
                _lojaTrabalho = value;
                OnPropertyChanged("LojaTrabalho");
            }
        }

        public virtual bool PagoEmFolha
        {
            get
            {
                return _pagoEmFolha;
            }

            set
            {
                _pagoEmFolha = value;
                OnPropertyChanged("PagoEmFolha");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException("Bonus Não Possui Propriedades Que Usam Lazy Loading");
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
