using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MVVMSQLiteApp
{
    public class UserViewModel : INotifyPropertyChanged
    {
        DBContext db;
        DelegateCommand addCommand;
        DelegateCommand editCommand;
        DelegateCommand deleteCommand;
        IEnumerable<UserData> userDatas;

        public IEnumerable<UserData> UserDatas
        {
            get { return userDatas; }
            set
            {
                userDatas = value;
                OnPropertyChanged("UserDatas");
            }
        }

        public UserViewModel()
        {
            db = new DBContext();
            db.UDataCollection.Load();
            UserDatas = db.UDataCollection.Local.ToBindingList();
        }

        public bool IsEnableBtn
        {
            get { return Convert.ToBoolean(db.UDataCollection.Count()); }
        }

        // команда добавления
        public DelegateCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new DelegateCommand((o) =>
                  {
                      UserDataView userDataView = new UserDataView(new UserData());
                      if (userDataView.ShowDialog() == true)
                      {
                          UserData userData = userDataView.UserDataModel;
                          userData.Person = Environment.UserName;
                          userData.Date = DateTime.Now.ToString();
                          db.UDataCollection.Add(userData);
                          db.SaveChanges();

                          userDataView.UpdateButtons(Convert.ToBoolean(db.UDataCollection.Count()));
                      }
                  }));
            }
        }
        // команда редактирования
        public DelegateCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new DelegateCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      UserData userData = selectedItem as UserData;

                      UserData vm = new UserData()
                      {
                          Id = userData.Id,
                          Person = Environment.UserName,
                          Date = DateTime.Now.ToString(),
                          Text = userData.Text
                      };
                      UserDataView phoneWindow = new UserDataView(vm);


                      if (phoneWindow.ShowDialog() == true)
                      {
                          // получаем измененный объект
                          userData = db.UDataCollection.Find(phoneWindow.UserDataModel.Id);
                          if (userData != null)
                          {
                              userData.Person = phoneWindow.UserDataModel.Person;
                              userData.Date = phoneWindow.UserDataModel.Date;
                              userData.Text = phoneWindow.UserDataModel.Text;
                              db.Entry(userData).State = EntityState.Modified;
                              db.SaveChanges();
                          }
                      }
                  }));
            }
        }
        // команда удаления
        public DelegateCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new DelegateCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      UserData userData = selectedItem as UserData;
                      db.UDataCollection.Remove(userData);
                      db.SaveChanges();

                      UserDataView userDataView = new UserDataView(new UserData());
                      userDataView.UpdateButtons(Convert.ToBoolean(db.UDataCollection.Count()));
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}