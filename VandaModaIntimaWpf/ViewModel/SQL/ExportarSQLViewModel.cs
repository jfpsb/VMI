using Microsoft.Win32;
using NHibernate;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.SQL
{
    public abstract class ExportarSQLViewModel<E> : ObservableObject where E : class, IModel
    {
        protected ISession _session;

        public ExportarSQLViewModel(ISession session)
        {
            _session = session;
        }

        protected DAO daoEntidade;
        public ICommand ExportarInsertsComando { get; set; }
        public ICommand ExportarUpdatesComando { get; set; }
        public ICommand InserirInsertComando { get; set; }

        private MySQLAliases aliasSelecionado;
        private ObservableCollection<MySQLAliases> aliases = new ObservableCollection<MySQLAliases>();
        public ExportarSQLViewModel()
        {
            ExportarInsertsComando = new RelayCommand(ExportarInserts);
            ExportarUpdatesComando = new RelayCommand(ExportarUpdates);
            InserirInsertComando = new RelayCommand(InserirSelect);
        }
        protected abstract void ExportarSQLInsert(StreamWriter sw, IList<E> entidades, string fileName);
        protected abstract void ExportarSQLUpdate(StreamWriter sw, IList<E> entidades, string fileName);
        protected virtual ObservableCollection<MySQLAliases> GetAliases(string[] exclusoes)
        {
            ObservableCollection<MySQLAliases> aliases = new ObservableCollection<MySQLAliases>();
            var persister = LocalSessionProvider.MainSessionFactory.GetClassMetadata(typeof(E));

            aliases.Add(new MySQLAliases() { Coluna = persister.IdentifierPropertyName, Alias = persister.IdentifierPropertyName });

            foreach (var columnName in persister.PropertyNames)
            {
                aliases.Add(new MySQLAliases() { Coluna = columnName, Alias = columnName });
            }

            foreach (string ex in exclusoes)
            {
                aliases = new ObservableCollection<MySQLAliases>(aliases.Where(w => !w.Coluna.Equals(ex)));
            }

            return aliases;
        }
        public async void ExportarInserts(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Script SQL (*.sql)|*.sql";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    ExportarSQLInsert(sw, await daoEntidade.Listar<E>(), fileName);
                }
            }
        }
        public async void ExportarUpdates(object paramenter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Script SQL (*.sql)|*.sql";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    ExportarSQLUpdate(sw, await daoEntidade.Listar<E>(), fileName);
                }
            }
        }
        private void InserirSelect(object parameter)
        {
            AliasSelecionado.ValorPadrao = "(SELECT <campo> FROM <tabela> WHERE <campo> = \"{nao_altere_este_campo}\" LIMIT 1)";
            OnPropertyChanged("AliasSelecionado");
            OnPropertyChanged("Aliases");
        }
        public void FechaJanela(object sender, CancelEventArgs e)
        {
            LocalSessionProvider.FechaSession(_session);
        }
        public MySQLAliases AliasSelecionado
        {
            get
            {
                return aliasSelecionado;
            }
            set
            {
                aliasSelecionado = value;
                OnPropertyChanged("AliasSelecionado");
            }
        }
        public ObservableCollection<MySQLAliases> Aliases
        {
            get { return aliases; }
            set
            {
                aliases = value;
                OnPropertyChanged("Aliases");
            }
        }
    }
}
