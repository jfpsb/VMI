﻿namespace SincronizacaoBD.Model
{
    public interface IModel
    {
        object GetIdentifier();
        string GetContextMenuHeader { get; }
    }
}
