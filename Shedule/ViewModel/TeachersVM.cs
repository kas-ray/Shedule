using Shedule.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Shedule.ViewModel
{
    public class TeachersVM : INotifyPropertyChanged
    {
        private List<Teachers> teachersList = new List<Teachers>();
        private Teachers selectedTeacher = new Teachers();
        public List<Teachers> TeachersList
        {
            get { return teachersList; }
            set
            {
                teachersList = value;
                OnPropertyChanged("TeachersList");
            }
        }
        public Teachers SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedTeacher = value;
                OnPropertyChanged("SelectedTeacher");
            }
        }
        private bool isAdd = false;
        public TeachersVM()
        {
            TeachersList = ScheduleBaseEntities.GetContext().Teachers.OrderBy(t => t.FIO).ToList();
            
            if (TeachersList.Count != 0 )
                SelectedTeacher = TeachersList[0];
            EditCommand = new DelegateCommand(EditWinShow);
            AddCommandShow = new DelegateCommand(AddTeacherShow);
            DeleteCommand = new DelegateCommand(DeleteTeacher);
            SaveCommand = new DelegateCommand(SaveTeacher);
            CancelCommand = new DelegateCommand(CancelTeacher);
        }
        /// <summary>
        /// Вызывает окно для редактирования информации о выбранном преподавателе
        /// </summary>
        private void EditWinShow(object obj)
        {
            TeacherEdit teacherEditW = new TeacherEdit(this);            
            teacherEditW.ShowDialog();
        }

        /// <summary>
        /// Вызывает окно для добавления нового преподавателя
        /// </summary>
        private void AddTeacherShow(object obj)
        {
            TeacherEdit teacherEditW = new TeacherEdit(this);
            //Teachers newt = new Teachers();

            SelectedTeacher = new Teachers();
            SelectedTeacher.FIO = "ФИО"; 
            //SelectedTeacher.Id = ScheduleBaseEntities.GetContext().Teachers.AsEnumerable().LastOrDefault().Id + 1;
            Teachers newt = ScheduleBaseEntities.GetContext().Teachers.Add(SelectedTeacher); 
            SelectedTeacher = newt;
            ScheduleBaseEntities.GetContext().SaveChanges();
            TeachersList = ScheduleBaseEntities.GetContext().Teachers.OrderBy(t => t.FIO).ToList();
            isAdd = true;

            teacherEditW.ShowDialog();
        }
        /// <summary>
        /// Удаляет выбранного преподавателя
        /// </summary>
        private void DeleteTeacher(object obj)
        {
            if (MessageBox.Show("Вы точно хотите удалить информацию об этом преподавателе?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //MessageBox.Show(SelectedTeacher.Id.ToString());
                ScheduleBaseEntities.GetContext().Teachers.Remove(SelectedTeacher);
                ScheduleBaseEntities.GetContext().Lessons.RemoveRange(ScheduleBaseEntities.GetContext().Lessons.Where(l => l.Teachers_Id == SelectedTeacher.Id));
                ScheduleBaseEntities.GetContext().SaveChanges();
                TeachersList = ScheduleBaseEntities.GetContext().Teachers.OrderBy(t => t.FIO).ToList();
            }
            if (TeachersList.Count != 0)
                SelectedTeacher = TeachersList[0];
        }

        //Функции окна редактирования
        /// <summary>
        /// Сохраняет изменения информации об преподавателе
        /// </summary>
        private void SaveTeacher(object obj)
        {
            StringBuilder error = new StringBuilder();
            Teachers newT = ScheduleBaseEntities.GetContext().Teachers.
                Where(t => t.Id == SelectedTeacher.Id).FirstOrDefault();
            newT = SelectedTeacher;
            if (string.IsNullOrWhiteSpace(newT.FIO))
                error.AppendLine("Укажите ФИО");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }               
            ScheduleBaseEntities.GetContext().SaveChanges();
            isAdd = false;
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinTeacherEdit");
            win.Close();
            //WinTeacherEdit
            if (TeachersList.Count != 0)
                SelectedTeacher = TeachersList[0];
        }
        /// <summary>
        /// Отменяет изменения информации о преподавателе
        /// </summary>
        private void CancelTeacher(object obj)
        {
            if (isAdd)
            {
                ScheduleBaseEntities.GetContext().Teachers.Remove(SelectedTeacher);
                ScheduleBaseEntities.GetContext().SaveChanges();
            } else 
                SelectedTeacher = ScheduleBaseEntities.GetContext().Teachers.
                    Where(t => t.Id == SelectedTeacher.Id).FirstOrDefault();
            isAdd = false;
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinTeacherEdit");
            win.Close();
            TeachersList = ScheduleBaseEntities.GetContext().Teachers.AsNoTracking().OrderBy(t => t.FIO).ToList();
            if (TeachersList.Count != 0)
                SelectedTeacher = TeachersList[0];
        }

        public ICommand EditCommand
        {
            get;set;
        }
        public ICommand AddCommandShow
        {
            get;set;
        }
        public ICommand DeleteCommand
        {
            get; set;
        }
        //Функции окна редактирования
        public ICommand SaveCommand
        {
            get;set;
        }
        public ICommand CancelCommand
        {
            get;set;
        } 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
