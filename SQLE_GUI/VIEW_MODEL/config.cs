using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLE_GUI.VIEW_MODEL
{
    class config
    {
        public static string DatabaseFile = "";
        public static string DataSource
        {
            get
            {
                return string.Format("data source={0}", DatabaseFile);
            }
        }


        public static string TableName_Users = "Users";
        public static string UsersCol0 = "ID";
        public static string UsersCol1 = "FirstName";
        public static string UsersCol2 = "LastName";
        public static string UsersCol3 = "Age";
    }
}

