using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLE_GUI.MODEL
{
    public class Person : ObservableObject
    {
        #region Members
        private string _FirstName;
        private string _LastName;
        private Int32 _Age;
        #endregion


        #region Properties
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    RaisePropertyChanged("LastName");
                }
            }
        }

        public Int32 Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        #endregion
    }
}
