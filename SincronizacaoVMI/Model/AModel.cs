using System;

namespace SincronizacaoVMI.Model
{
    public abstract class AModel : ObservableObject
    {
        private Guid _uuid;
        private DateTime? _criadoEm;
        private DateTime? _modificadoEm;
        private DateTime? _deletadoEm;
        private bool _deletado;

        public virtual void Copiar(object source)
        {
            foreach (var prop in GetType().GetProperties())
            {
                if (prop.Name.Equals("Id")) continue;
                if (prop.GetSetMethod() == null) continue;
                prop.SetValue(this, source.GetType().GetProperty(prop.Name).GetValue(source));
            }
        }

        protected virtual void CopiarDadosSinc(object source)
        {
            AModel a = source as AModel;
            Uuid = a.Uuid;
            CriadoEm = a.CriadoEm;
            ModificadoEm = a.ModificadoEm;
            DeletadoEm = a.DeletadoEm;
            Deletado = a.Deletado;
        }
        public virtual string Tipo => GetType().Name.ToLower();
        public virtual bool Deletado
        {
            get => _deletado;
            set
            {
                _deletado = value;
                OnPropertyChanged("Deletado");
            }
        }

        public virtual DateTime? CriadoEm
        {
            get => _criadoEm;
            set
            {
                _criadoEm = value;
                OnPropertyChanged("CriadoEm");
            }
        }
        public virtual DateTime? ModificadoEm
        {
            get => _modificadoEm;
            set
            {
                _modificadoEm = value;
                OnPropertyChanged("ModificadoEm");
            }
        }
        public virtual DateTime? DeletadoEm
        {
            get => _deletadoEm;
            set
            {
                _deletadoEm = value;
                OnPropertyChanged("DeletadoEm");
            }
        }

        public virtual Guid Uuid
        {
            get
            {
                return _uuid;
            }

            set
            {
                _uuid = value;
                OnPropertyChanged("Uuid");
            }
        }
    }
}
