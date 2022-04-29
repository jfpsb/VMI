using NHibernate;
using NHibernate.Type;
using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class TriggerInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (entity is AModel)
            {
                if ((entity as AModel).Uuid == null || (entity as AModel).Uuid == Guid.Empty)
                {
                    (entity as AModel).Uuid = Guid.NewGuid();
                    (entity as AModel).CriadoEm = DateTime.Now;
                    int UuidIndex = GetIndex(propertyNames, "Uuid");
                    int CriadoEmIndex = GetIndex(propertyNames, "CriadoEm");
                    state[UuidIndex] = (entity as AModel).Uuid;
                    state[CriadoEmIndex] = (entity as AModel).CriadoEm;
                }
            }
            return base.OnSave(entity, id, state, propertyNames, types);
        }
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            if (entity is AModel)
            {
                (entity as AModel).ModificadoEm = DateTime.Now;
                int ModificadoEmIndex = GetIndex(propertyNames, "ModificadoEm");
                currentState[ModificadoEmIndex] = (entity as AModel).ModificadoEm;

                if ((entity as AModel).Deletado)
                {
                    (entity as AModel).DeletadoEm = (entity as AModel).ModificadoEm;
                    int DeletadoEmIndex = GetIndex(propertyNames, "DeletadoEm");
                    currentState[DeletadoEmIndex] = (entity as AModel).ModificadoEm;
                }
            }
            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        private int GetIndex(object[] array, string property)
        {
            for (var i = 0; i < array.Length; i++)
                if (array[i].ToString() == property)
                    return i;

            return -1;
        }
    }
}
