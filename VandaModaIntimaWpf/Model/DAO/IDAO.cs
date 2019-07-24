﻿using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO
{
    public interface IDAO<T>
    {
        bool Inserir(T objeto);
        bool Atualizar(T objeto);
        bool InserirOuAtualizar(T objeto);
        bool Deletar(T objeto);
        bool Deletar(IList<T> objetos);
        IList<T> Listar(ICriteria criteria);
        ICriteria CriarCriteria();
    }
}
