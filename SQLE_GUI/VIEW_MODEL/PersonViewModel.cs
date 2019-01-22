using MicroMvvm;
using SQLE_GUI.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SQLE_GUI.VIEW_MODEL
{
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
 
        #endregion
    }
}
