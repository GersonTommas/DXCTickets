using System;
using System.Linq;

using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.IO;

namespace DXCTickets
{
    class Db
    {
        #region Variables

        private static SQLiteConnection Conect() { return new SQLiteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TG\\TGSoftDB.db;foreign keys=true;"); }

        #endregion


        #region Global

        public static DBTicketsContext DBTickets;

        public static void DBConnect() { CreateTables(); DBTicketsConnect(); }
        public static void DBConnect(string _temp) { CreateTables(); DBTicketsConnect(); }

        public static void DBTicketsConnect() { DBTickets = new DBTicketsContext(Conect()); }

        #endregion


        #region Private

        private static void CreateTables()
        {
            //CreateTable("DROP TABLE [SolutionsTable]");
            CreateTable("CREATE TABLE IF NOT EXISTS [SolutionsTable] ([ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT NOT NULL, [EnOne] TEXT, [EnTwo] TEXT, [EnThree] TEXT, [PtOne] TEXT, [PtTwo] TEXT, [PtThree] TEXT, [EsOne] TEXT, [EsTwo] TEXT, [EsThree] TEXT)");
        }
        private static void CreateTable(string _STRtable)
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TG\\");
            using (var sqldb = Conect())
            {
                sqldb.Open();
                var commandTable01 = new SQLiteCommand(_STRtable, sqldb);
                commandTable01.ExecuteNonQuery();
                sqldb.Close();
            }
        }
        private static void DefaultOptions()
        {

        }

        #endregion
    }

    #region Tickets

    #region Database Tickets

    [Database(Name = "MyDataBase")]
    public partial class DBTicketsContext : System.Data.Linq.DataContext
    {
        #region On Created

        private static MappingSource mappingSource = new AttributeMappingSource();

        partial void OnCreated();

        public DBTicketsContext(System.Data.IDbConnection connection) : base(connection, mappingSource) { OnCreated(); }

        #endregion


        #region Search

        // Solutions
        public DBSolutionsClass GlobalNewSolution { get { return new DBSolutionsClass() { Id = FindMaxSolutionsID() + 1 }; } }
        private int FindMaxSolutionsID() { try { return AllSolutionsTable.Select(x => x.Id).Max(); } catch { return 0; } }

        #endregion


        #region Tables

        public System.Data.Linq.Table<DBSolutionsClass> AllSolutionsTable { get { return GetTable<DBSolutionsClass>(); } }

        #endregion
    }

    [Database(Name = "MyDatabase")]

    #endregion


    #region Table Solutions

    [Table(Name = "SolutionsTable")]
    public class DBSolutionsClass : ObservableClass
    {
        #region Private

        private int _Id; private string _Name, _EnOne, _EnTwo, _EnThree, _PtOne, _PtTwo, _PtThree, _EsOne, _EsTwo, _EsThree;

        #endregion


        #region Public

        [Column(IsPrimaryKey = true, Storage = "_Id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int Id { get { return _Id; } set { if (_Id != value) { _Id = value; OnPropertyChanged("Id"); } } }

        [Column(Storage = "_Name")]
        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; OnPropertyChanged("Name"); } } }
        [Column(Storage = "_EnOne")]
        public string EnOne { get { return _EnOne; } set { if (_EnOne != value) { _EnOne = value; OnPropertyChanged("EnOne"); } } }
        [Column(Storage = "_EnTwo")]
        public string EnTwo { get { return _EnTwo; } set { if (_EnTwo != value) { _EnTwo = value; OnPropertyChanged("EnTwo"); } } }
        [Column(Storage = "_EnThree")]
        public string EnThree { get { return _EnThree; } set { if (_EnThree != value) { _EnThree = value; OnPropertyChanged("EnThree"); } } }
        [Column(Storage = "_PtOne")]
        public string PtOne { get { return _PtOne; } set { if (_PtOne != value) { _PtOne = value; OnPropertyChanged("PtOne"); } } }
        [Column(Storage = "_PtTwo")]
        public string PtTwo { get { return _PtTwo; } set { if (_PtTwo != value) { _PtTwo = value; OnPropertyChanged("PtTwo"); } } }
        [Column(Storage = "_PtThree")]
        public string PtThree { get { return _PtThree; } set { if (_PtThree != value) { _PtThree = value; OnPropertyChanged("PtThree"); } } }
        [Column(Storage = "_EsOne")]
        public string EsOne { get { return _EsOne; } set { if (_EsOne != value) { _EsOne = value; OnPropertyChanged("EsOne"); } } }
        [Column(Storage = "_EsTwo")]
        public string EsTwo { get { return _EsTwo; } set { if (_EsTwo != value) { _EsTwo = value; OnPropertyChanged("EsTwo"); } } }
        [Column(Storage = "_EsThree")]
        public string EsThree { get { return _EsThree; } set { if (_EsThree != value) { _EsThree = value; OnPropertyChanged("EsThree"); } } }

        #endregion
    }

    #endregion

    #endregion
}
