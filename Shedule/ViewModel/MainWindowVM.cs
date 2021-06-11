using Shedule.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shedule.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private Page Disciplines;
        private Page Main;
        private Page Teachers;

        private Page currentPage;

        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public MainWindowVM()
        {
            Disciplines = new LessonsV();
            Main = new MainV();
            Teachers = new TeachersV();
            DisciplinesCommand = new DelegateCommand(DisciplinesShow);
            MainCommand = new DelegateCommand(MainShow);
            TeachersCommand = new DelegateCommand(TeachersShow);
            CurrentPage = Main;
        }
        private void DisciplinesShow(object obj)
        {
            CurrentPage = Disciplines;
        }

        private void MainShow(object obj)
        {
            CurrentPage = Main;
        }
        private void TeachersShow(object ob)
        {
            CurrentPage = Teachers;
        }
        public ICommand DisciplinesCommand
        {
            get; set;
        }
        public ICommand MainCommand
        {
            get; set;
        }
        public ICommand TeachersCommand
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
