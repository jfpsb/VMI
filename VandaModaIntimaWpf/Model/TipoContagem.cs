﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoContagem : AModel, IModel
    {
        private int _id;
        private string _nome;
        public virtual bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(TipoContagem))
            {
                TipoContagem tipoContagem = (TipoContagem)obj;
                return tipoContagem.Id.Equals(Id)
                       && tipoContagem.Nome.Equals(Nome);
            }
            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Nome;

        [JsonIgnore]
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

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException("TipoContagem Não Possui Propriedades Com Lazy Loading");
        }
    }
}
