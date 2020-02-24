using System;

namespace SincronizacaoBD.Sincronizacao
{
    class DatabaseLogFileInfo
    {
        public string FileName { get; set; }
        public DateTime LastModified { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            DatabaseLogFileInfo databaseLogFileInfo = (DatabaseLogFileInfo)obj;

            if (databaseLogFileInfo.FileName.Equals(FileName))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }
    }
}
