using MicroMvvm;
using Microsoft.Win32;
using SQLE_GUI.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Data.SQLite;
using System.Windows;
using System.IO;
using System.Reflection;
using System.Data;

namespace SQLE_GUI.VIEW_MODEL
{
    public class ListWithName
    {
        public ListWithName()
        {
            SubItems = new List<string>();
        }
        public string Text { set; get; }
        public List<string> SubItems { set; get; }
    }


    public class PersonViewModel
    {
        #region Properties
        public ObservableCollection<Person> PersonList { get; set; }

        public Person SelectedPerson { get; set; }

        #endregion

        #region Construction
        public PersonViewModel()
        {
            if (PersonList == null)
            {
                if (System.Diagnostics.Debugger.IsAttached == true)
                // code or timeout value when running tests in debug mode
                {
                    PersonList = new ObservableCollection<Person>()
                    {
                        new Person() { FirstName = "User1", LastName = "User1_L", Age = 10 },
                        new Person() { FirstName = "User2", LastName = "User2_L", Age = 20 },
                        new Person() { FirstName = "User3", LastName = "User3_L", Age = 30 },
                    };
                }
                else
                // non debug mode
                {
                    PersonList = new ObservableCollection<Person>()
                    {
                        //new Person() { FirstName = "User1", LastName = "User1_L", Age = 10 },
                        //new Person() { FirstName = "User2", LastName = "User2_L", Age = 20 },
                        //new Person() { FirstName = "User3", LastName = "User3_L", Age = 30 },
                    };
                }

                
            }
        }
        #endregion

        #region Commands

        public RelayCommand CreateCommand { get { return new RelayCommand(CreateCommand_Execute, Can_CreateCommand_Execute); } }

        private bool Can_CreateCommand_Execute()
        {
            return true;
        }

        private void CreateCommand_Execute()
        {
            if (PersonList == null)
                return;

            PersonList.Add(new Person { FirstName = "newFirstN", LastName = "newLastN", Age = 22 });
        }


        public RelayCommand ReadCommand { get { return new RelayCommand(ReadCommand_Execute, Can_ReadCommand_Execute); } }

        private bool Can_ReadCommand_Execute()
        {
            return true;
        }

        private void ReadCommand_Execute()
        {
            if (PersonList == null)
                return;

            PersonList.Add(new Person { FirstName = "newFirstNdd", LastName = "newLastNdd", Age = 232 });
        }


        public RelayCommand UpdateCommand { get { return new RelayCommand(UpdateCommand_Execute, Can_UpdateCommand_Execute); } }

        private bool Can_UpdateCommand_Execute()
        {
            return true;
        }

        private void UpdateCommand_Execute()
        {
            if (PersonList == null)
                return;
            if (SelectedPerson == null)
                return;

            //PersonList[PersonList.IndexOf(PersonList.First(person => person.FirstName == SelectedPerson.FirstName))].FirstName = SelectedPerson.FirstName;
            //PersonList[PersonList.IndexOf(PersonList.First(person => person.FirstName == SelectedPerson.FirstName))].LastName = SelectedPerson.LastName;
            //PersonList[PersonList.IndexOf(PersonList.First(person => person.FirstName == SelectedPerson.FirstName))].Age = SelectedPerson.Age;
        }


        public RelayCommand DeleteCommand { get { return new RelayCommand(DeleteCommand_Execute, Can_DeleteCommand_Execute); } }

        private bool Can_DeleteCommand_Execute()
        {
            return true;
        }

        private void DeleteCommand_Execute()
        {
            if (PersonList == null)
                return;
            if (SelectedPerson == null)
                return;

            PersonList.Remove(SelectedPerson);
        }


        public RelayCommand NewDBCommand { get { return new RelayCommand(NewDBCommand_Execute, Can_NewDBCommand_Execute); } }

        private bool Can_NewDBCommand_Execute()
        {
            return true;
        }

        private void NewDBCommand_Execute()
        {
            if (New_DB_Dialog() != true) { return; }

            PersonList.Clear();

            Create_Empty_DataBase();

        }

        private bool New_DB_Dialog()
        {
            bool ret = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SQLite file |*.db; *.sqlite; *.sl3";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(saveFileDialog.FileName);

                //db_name = fi.FullName;
                config.DatabaseFile = fi.FullName;

                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        private void Create_Empty_DataBase()
        {
            // creating Users table
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sqlite = new SQLiteHelper(cmd);
                    SQLiteTable table = new SQLiteTable(config.TableName_Users);

                    // delete table
                    sqlite.DropTable(config.TableName_Users);


                    SQLiteColumn column_ID = new SQLiteColumn(config.UsersCol0, ColType.Integer, true, true, true, "");
                    SQLiteColumn column_FirstName = new SQLiteColumn(config.UsersCol1, ColType.Text);
                    SQLiteColumn column_LastName = new SQLiteColumn(config.UsersCol2, ColType.Text);
                    SQLiteColumn column_Age = new SQLiteColumn(config.UsersCol3, ColType.Integer);
                    table.Columns.Add(column_ID);
                    table.Columns.Add(column_FirstName);
                    table.Columns.Add(column_LastName);
                    table.Columns.Add(column_Age);

                    sqlite.CreateTable(table);

                    conn.Close();
                }
            }

        }

        public RelayCommand OpenDBCommand { get { return new RelayCommand(OpenDBCommand_Execute, Can_OpenDBCommand_Execute); } }

        private bool Can_OpenDBCommand_Execute()
        {
            return true;
        }

        private void OpenDBCommand_Execute()
        {
            if (Open_DB_Dialog() != true) { return; }

            PersonList.Clear();

            Read_From_DataBase();

        }

        private bool Open_DB_Dialog()
        {
            bool ret = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQLite file |*.db; *.sqlite; *.sl3";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(openFileDialog.FileName);

                config.DatabaseFile = fi.FullName;
                ret = true;

            }
            else
            {
                ret = false;
            }

            return ret;
        }


        private bool TestConnection()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public RelayCommand SaveDBCommand { get { return new RelayCommand(SaveDBCommand_Execute, Can_SaveDBCommand_Execute); } }

        private bool Can_SaveDBCommand_Execute()
        {
            return true;
        }

        private void SaveDBCommand_Execute()
        {
            if (Save_DB_Dialog() != true) { return; }

            Write_To_DataBase();
        }

        private bool Save_DB_Dialog()
        {
            bool ret = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SQLite file |*.db; *.sqlite; *.sl3";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(saveFileDialog.FileName);

                //db_name = fi.FullName;
                config.DatabaseFile = fi.FullName;

                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        private void Create_Empty_DataBase_old()
        {
            //SQLiteHelper sqlite = new SQLiteHelper(new SQLiteCommand(config.DataSource));

            //using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            //{
            //    conn.Open();
            //    SQLiteTable table = new SQLiteTable(config.TableName_Users);
            //    sqlite.CreateTable(table);
            //    conn.Close();
            //}


            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sqlite = new SQLiteHelper(cmd);
                    SQLiteTable table = new SQLiteTable(config.TableName_Users);
                    SQLiteColumn column_ID = new SQLiteColumn("ID", ColType.Integer, true, true, true, "");
                    SQLiteColumn column_FirstName = new SQLiteColumn("FirstName", ColType.Text);
                    SQLiteColumn column_LastName = new SQLiteColumn("LastName", ColType.Text);
                    SQLiteColumn column_Age = new SQLiteColumn("Age", ColType.Integer);
                    table.Columns.Add(column_ID);
                    table.Columns.Add(column_FirstName);
                    table.Columns.Add(column_LastName);
                    table.Columns.Add(column_Age);

                    sqlite.CreateTable(table);

                    conn.Close();
                }
            }

        }

        private void Write_To_DataBase()
        {
            // creating Users table
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sqlite = new SQLiteHelper(cmd);
                    SQLiteTable table = new SQLiteTable(config.TableName_Users);

                    // delete table
                    sqlite.DropTable(config.TableName_Users);


                    SQLiteColumn column_ID = new SQLiteColumn(config.UsersCol0, ColType.Integer, true, true, true, "");
                    SQLiteColumn column_FirstName = new SQLiteColumn(config.UsersCol1, ColType.Text);
                    SQLiteColumn column_LastName = new SQLiteColumn(config.UsersCol2, ColType.Text);
                    SQLiteColumn column_Age = new SQLiteColumn(config.UsersCol3, ColType.Integer);
                    table.Columns.Add(column_ID);
                    table.Columns.Add(column_FirstName);
                    table.Columns.Add(column_LastName);
                    table.Columns.Add(column_Age);

                    sqlite.CreateTable(table);

                    conn.Close();
                }
            }

            // populating Users table
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sqlite = new SQLiteHelper(cmd);

                    Dictionary<string, object> sqlite_dic = new Dictionary<string, object>();

                    foreach (Person person in PersonList)
                    {
                        sqlite_dic.Add(config.UsersCol1, person.FirstName);
                        sqlite_dic.Add(config.UsersCol2, person.LastName);
                        sqlite_dic.Add(config.UsersCol3, person.Age);

                        sqlite.Insert(config.TableName_Users, sqlite_dic);
                        sqlite_dic.Clear();
                    }

                   conn.Close();
                }
            }

        }

        private void Read_From_DataBase()
        {
            // read Users table
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sqlite = new SQLiteHelper(cmd);

                    DataTable personTable = new DataTable();
                    personTable.Columns.Add(new DataColumn { ColumnName = "ID", DataType = typeof(Int32) });
                    personTable.Columns.Add(new DataColumn { ColumnName = "FirstName", DataType = typeof(String) } );
                    personTable.Columns.Add(new DataColumn { ColumnName = "LastName", DataType = typeof(String) });
                    personTable.Columns.Add(new DataColumn { ColumnName = "Age", DataType = typeof(Int32) });

                    string cmd_get_table = "SELECT * FROM " + config.TableName_Users;

                    personTable = sqlite.Select(cmd_get_table);


                    foreach(DataRow row in personTable.Rows)
                    {
                        Person person = new Person();
                        person.FirstName = row.Field<string>("FirstName");
                        person.LastName = row.Field<string>("LastName");
                        person.Age = Convert.ToInt32(row.Field<Int64>("Age"));
                        PersonList.Add(person);
                    }

                    conn.Close();
                }
            }

        }


        #endregion



    }
}


