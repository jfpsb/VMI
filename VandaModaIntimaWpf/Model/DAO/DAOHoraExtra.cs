﻿using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHoraExtra : DAO
    {
        public DAOHoraExtra(ISession session) : base(session)
        {
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.LoadAsync<HoraExtra>(id);
        }
    }
}
