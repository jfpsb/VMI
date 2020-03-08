using System;

namespace SincronizacaoBD.Util.Sincronizacao
{
    class DatabaseLogFileInfo
    {
        public string FileName { get; set; }
        public DateTime LastModified { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(DatabaseLogFileInfo)) return false;

            DatabaseLogFileInfo databaseLogFileInfo = (DatabaseLogFileInfo)obj;

            if (databaseLogFileInfo.FileName.Equals(FileName) && databaseLogFileInfo.LastModified.Equals(LastModified))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (FileName + LastModified.ToString()).GetHashCode();
        }
    }
}
