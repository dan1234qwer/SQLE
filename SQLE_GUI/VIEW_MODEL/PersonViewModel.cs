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
                PersonList = new ObservableCollection<Person>()
                {
                    new Person() { FirstName = "User1", LastName = "User1_L", Age = 10 },
                    new Person() { FirstName = "User2", LastName = "User2_L", Age = 20 },
                    new Person() { FirstName = "User3", LastName = "User3_L", Age = 30 },
                };
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


        public RelayCommand OpenDBCommand { get { return new RelayCommand(OpenDBCommand_Execute, Can_OpenDBCommand_Execute); } }

        private bool Can_OpenDBCommand_Execute()
        {
            return true;
        }

        private void OpenDBCommand_Execute()
        {
            if (Open_DB_Dialog() != true) { return; }

            //DB_Equipment_2_Item_Settings();

        }

        private bool Open_DB_Dialog()
        {
            bool ret = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQLite file |*.db; *.sqlite; *.sl3";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fi = new FileInfo(openFileDialog.FileName);
                //db_name = fi.FullName;
                //db_path = fi.DirectoryName;
                //projName.Text = fi.Name;
                //projName.Foreground = Brushes.DodgerBlue;
                ret = true;

                // create obj
                //SQLiteHelper sqlite = new SQLiteHelper();

                // set DB name
                //sqlite.DatabaseFile = fi.FullName;  // "racdb.db";
            }
            else
            {
                ret = false;
            }

            return ret;
        }

#if false
        private void DB_Equipment_2_Item_Settings()
        {
            List<ListWithName> eqList = new List<ListWithName>();
            //ITEM.Equipment_class equipment = new ITEM.Equipment_class();
            //ITEM item = new ITEM();
            string key = String.Empty;

            // create DB obj
            SQLiteHelper sqlite = new SQLiteHelper();

            // set DB name
            sqlite.DatabaseFile = db_name;  // "racdb.db";

            // iterate through Equipments table and get ch_ID        
            eqList = sqlite.GetEntries("Equipments");

            //  LIST TYPE
            //  1 = TextBox  2 = ListBox  3 = CheckBox  4 = TextBlock

            foreach (ListWithName eql in eqList)
            {
                var item = new ITEM();
                item.Setting_List = new ObservableCollection<ITEM.Setting>();

                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment ID", Value_string = eql.SubItems[0].ToString(), List_type = 4 });
                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment Name", Value_string = eql.SubItems[1].ToString(), List_type = 4 });
                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment Process", Value_string = eql.SubItems[2].ToString(), List_type = 4 });
                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment ChannelId", Value_string = eql.SubItems[3].ToString(), List_type = 4 });
                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment Description", Value_string = eql.SubItems[4].ToString(), List_type = 1 });
                item.Setting_List.Add(new ITEM.Setting { Field = "Equipment Active", Value_string = eql.SubItems[5].ToString(), List_type = 4 });

                // add key to dictionary
                key = eql.SubItems[1].ToString();
                dictionary.Add(key, item);
            }
        }
#endif 
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

            //Item_Settings_2_DB_Equipment();
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

                Create_Empty_DataBase(fi.Name);
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        private void Create_Empty_DataBase(string DB_name)
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



            //sqlite..DatabaseFile = DB_name;
            //sqlite.CreateDatabase();

            //DataTable channel = new DataTable();
            //channel.TableName = "Channel";
            //channel.PrimaryKey = new DataColumn[] { channel.Columns["ID"] };
            //channel.Columns.Add(new DataColumn { ColumnName = "Type", DataType = typeof(String), AllowDBNull = false });
            //channel.Columns.Add(new DataColumn { ColumnName = "Description", DataType = typeof(String), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "IP", DataType = typeof(String), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "PORT", DataType = typeof(String), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "BAUDRATE", DataType = typeof(Int32), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "DATABITS", DataType = typeof(Int32), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "STOPBITS", DataType = typeof(Int32), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "PARITY", DataType = typeof(String), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "FLOWCTRL", DataType = typeof(String), AllowDBNull = true });
            //channel.Columns.Add(new DataColumn { ColumnName = "RTSCONTROL", DataType = typeof(String), AllowDBNull = true });
            //sqlite.CreateTable(channel);




        }

#if false
        private void Item_Settings_2_DB_Equipment()
        {
            ITEM item = new ITEM();
            EntryList entry = new EntryList();

            string eq_id = String.Empty;
            string eq_name = String.Empty;
            string eq_process = String.Empty;
            string eq_channelid = String.Empty;
            string eq_description = String.Empty;
            string eq_active = String.Empty;

            //ITEM.Equipment_class equipment = new ITEM.Equipment_class();

            SQLiteHelper sqlite = new SQLiteHelper();
            sqlite.DatabaseFile = db_name;

            foreach (string key in dictionary.Keys)
            {
                //item = new ITEM();
                item = dictionary[key];

                foreach (ITEM.Setting sett in item.Setting_List)
                {
                    if (sett.Field.Contains("Equipment ID") == true)
                    {
                        eq_id = sett.Value_string;
                    }

                    if (sett.Field.Contains("Equipment Name") == true)
                    {
                        eq_name = sett.Value_string;
                    }

                    if (sett.Field.Contains("Equipment Process") == true)
                    {
                        eq_process = sett.Value_string;
                    }

                    if (sett.Field.Contains("Equipment ChannelId") == true)
                    {
                        eq_channelid = (sett.Value_string == "") ? "0" : sett.Value_string;
                    }

                    if (sett.Field.Contains("Equipment Description") == true)
                    {
                        eq_description = sett.Value_string;
                    }

                    if (sett.Field.Contains("Equipment Active") == true)
                    {
                        eq_active = sett.Value_string;
                    }
                }

                // create entry in DB for table Equipment
                entry = new EntryList();
                entry.ColumnName = new List<string> { "ID", "Name", "Process", "ChannelId", "Description", "Active" };
                entry.Content = new List<string> { eq_id, eq_name, eq_process, eq_channelid,
                    eq_description, eq_active };
                entry.DbType = new List<DbType>() { DbType.Int32, DbType.String, DbType.String, DbType.Int32,
                    DbType.String, DbType.Int32 };
                sqlite.CreateEntry("Equipments", entry);
            }
        }


#endif
        #endregion



    }
}


