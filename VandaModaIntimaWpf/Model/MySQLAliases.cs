namespace VandaModaIntimaWpf.Model
{
    public class MySQLAliases : ObservableObject
    {
        private string coluna;
        private string alias;
        private string valorPadrao;
        public string Coluna
        {
            get { return coluna; }
            set
            {
                coluna = value;
                OnPropertyChanged("Coluna");
            }
        }
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }
        public string ValorPadrao
        {
            get { return valorPadrao; }
            set
            {
                valorPadrao = value;
                OnPropertyChanged("ValorPadrao");
            }
        }
    }
}
