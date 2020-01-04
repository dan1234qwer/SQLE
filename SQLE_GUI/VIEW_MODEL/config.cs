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
    }
}

