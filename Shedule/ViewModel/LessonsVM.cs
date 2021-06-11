using Shedule.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shedule.ViewModel
{
    public class LessonsVM : INotifyPropertyChanged
    {
        private List<Disciplines> disciplinesList = new List<Disciplines>();
        private Disciplines selectedDiscipline;
        private List<Lessons> lessonsList = new List<Lessons>();
        private Lessons selectedLesson = new Lessons();
        private List<Lessons> lecLessons = new List<Lessons>();
        private List<Lessons> semLessons = new List<Lessons>();
        private List<Lessons> labLessons = new List<Lessons>();
        private List<Lessons> rightLessons = new List<Lessons>();
        private Visibility lecVisible = new Visibility();
        private Visibility semVisible = new Visibility();
        private Visibility labVisible = new Visibility();
        public List<Disciplines> DisciplinesList
        {
            get { return disciplinesList; }
            set
            {
                disciplinesList = value;
                OnPropertyChanged("DisciplinesList");
            }
        }
        public Disciplines SelectedDiscipline
        {
            get { return selectedDiscipline; }
            set {
                selectedDiscipline = value;
                //только пары с нужной дисциплиной
                //LecLessons = ScheduleBaseEntities.GetContext().Lessons.
                //    Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 1).ToList();
                //SemLessons = ScheduleBaseEntities.GetContext().Lessons.
                //    Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 2).ToList();
                //LabLessons = ScheduleBaseEntities.GetContext().Lessons.
                //    Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 3).ToList();
                if(SelectedDiscipline != null)
                {
                    LecLessons = LessonsList.Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 1).ToList();
                    SemLessons = LessonsList.Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 2).ToList();
                    LabLessons = LessonsList.Where(l => l.Disciplines_Id == SelectedDiscipline.Id && l.Lessons_types.Id == 3).ToList();
                }
                OnPropertyChanged("SelectedDiscipline");

            }
        }
        public List<Lessons> LessonsList
        {
            get { return lessonsList; }
            set
            {
                lessonsList = value;
                OnPropertyChanged("LessonsList");
            }
        } 
        public Lessons SelectedLesson
        {
            get { return selectedLesson; }
            set
            {
                selectedLesson = value;
                OnPropertyChanged("SelectedLesson");
            }
        } 

        public List<Lessons> LecLessons
        {
            get { return lecLessons; }
            set
            {
                lecLessons = value;
                OnPropertyChanged("LecLessons");
            }
        }
        public List<Lessons> SemLessons
        {
            get { return semLessons; }
            set
            {
                semLessons = value;
                OnPropertyChanged("SemLessons");
            }
        }
        public List<Lessons> LabLessons
        {
            get { return labLessons; }
            set
            {
                labLessons = value;
                OnPropertyChanged("LabLessons");
            }
        }
        public List<Lessons> RightLessons
        {
            get { return rightLessons; }
            set
            {
                rightLessons = value;
                OnPropertyChanged("RightLessons");
            }
        }
        public Visibility LecVisible
        {
            get { return lecVisible; }
            set
            {
                lecVisible = value;
                OnPropertyChanged("LecVisible");
            }
        }
        public Visibility SemVisible
        {
            get { return semVisible; }
            set
            {
                semVisible = value;
                OnPropertyChanged("SemVisible");
            }
        }
        public Visibility LabVisible
        {
            get { return labVisible; }
            set
            {
                labVisible = value;
                OnPropertyChanged("LabVisible");
            }
        }
                                        //пробуем 
        private List<Teachers> teachersList = new List<Teachers>();
        private List<Times> timesList = new List<Times>();
        private List<Week_days> week_DaysList = new List<Week_days>();
        public List<Teachers> TeachersList
        {
            get { return teachersList; }
            set
            {
                teachersList = value;
                OnPropertyChanged("TeachersList");
            }
        }
        public List<Times> TimesList
        {
            get { return timesList; }
            set
            {
                timesList = value;
                OnPropertyChanged("TimesList");
            }
        }
        public List<Week_days> Week_DaysList
        {
            get { return week_DaysList; }
            set
            {
                week_DaysList = value;
                OnPropertyChanged("Week_DaysList");
            }
        }
        private bool isAdd = false;
        public LessonsVM()
        {
            LessonsList = ScheduleBaseEntities.GetContext().Lessons.ToList();
            DisciplinesList = ScheduleBaseEntities.GetContext().Disciplines.OrderBy(t => t.Name).ToList();
            if (DisciplinesList.Count != 0)
                SelectedDiscipline = DisciplinesList[0];     //  если список пустой сделать проверку   

            LecVisible = Visibility.Collapsed;
            SemVisible = Visibility.Collapsed;
            LabVisible = Visibility.Collapsed;

            ShowLecCommand = new DelegateCommand(ShowLec);
            ShowSemCommand = new DelegateCommand(ShowSem);
            ShowLabCommand = new DelegateCommand(ShowLab);

            EditDatesCommand = new DelegateCommand(EditDates);
            AddDateCommand = new DelegateCommand(AddDate);
            DeleteDateCommand = new DelegateCommand(DeleteDate);
            SaveDateCommand = new DelegateCommand(SaveDate);
            CancelDateCommand = new DelegateCommand(CancelDate);

            EditCommand = new DelegateCommand(EditWinShow);
            AddCommandShow = new DelegateCommand(AddDisciplineShow);
            DeleteCommand = new DelegateCommand(DeleteDiscipline);

            SaveCommand = new DelegateCommand(SaveDiscipline);
            CancelCommand = new DelegateCommand(CancelDiscipline);
            
            //пробуем 
            TeachersList = ScheduleBaseEntities.GetContext().Teachers.OrderBy(t => t.FIO).ToList();
            TimesList = ScheduleBaseEntities.GetContext().Times.ToList();
            Week_DaysList = ScheduleBaseEntities.GetContext().Week_days.ToList();
        }
        private void ShowLec(object obj)
        {
            LecVisible = LecVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (LecVisible == Visibility.Visible)
            {
                SemVisible = Visibility.Collapsed;
                LabVisible = Visibility.Collapsed;
                if (LecLessons.Count != 0)
                    SelectedLesson = LecLessons[0];
                else
                    SelectedLesson = null;
            }           
        }
        private void ShowSem(object obj)
        {
            SemVisible = SemVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (SemVisible == Visibility.Visible)
            {
                LecVisible = Visibility.Collapsed;
                LabVisible = Visibility.Collapsed;
                if (SemLessons.Count != 0)
                    SelectedLesson = SemLessons[0];
                else
                    SelectedLesson = null;
            }
        }
        private void ShowLab(object obj)
        {
            LabVisible = LabVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (LabVisible == Visibility.Visible)
            {
                LecVisible = Visibility.Collapsed;
                SemVisible = Visibility.Collapsed;
                if (LabLessons.Count != 0)
                    SelectedLesson = LabLessons[0];
                else
                    SelectedLesson = null;
            }
        }
        /// <summary>
        /// Вызывает окно для редактирования информации о выбранной дате
        /// </summary>
        private void EditDates(object obj) // obj = SelectedLesson
        {
            if (obj != null)
            {
                LessonEdit lessonEdit = new LessonEdit(this);
                lessonEdit.ShowDialog();
                LecVisible = Visibility.Collapsed;
                SemVisible = Visibility.Collapsed;
                LabVisible = Visibility.Collapsed;
            }
            else
                MessageBox.Show("Пока нечего редактировать. Сначала добавьте элементы.");        
        }
        private void AddDate(object obj) // obj empty
        {
            if (TeachersList.Count == 0)
            {
                MessageBox.Show("Сначала заполните список учителей");
                return;
            }
            LessonEdit lessonEdit = new LessonEdit(this);
            SelectedLesson = new Lessons();
            Dates newDate = new Dates();

            SelectedLesson.Disciplines_Id = SelectedDiscipline.Id;
            SelectedLesson.Lessons_types_Id = LecVisible == Visibility.Visible ? 1 : 
                SemVisible == Visibility.Visible ? 2 : 3;
            SelectedLesson.Teachers_Id = TeachersList[0].Id;

            newDate.Start = DateTime.Today;
            newDate.End = DateTime.Today;
            newDate.Week_days_Id = (int) newDate.Start.DayOfWeek;
            newDate.Times_Id = 1;
            newDate = ScheduleBaseEntities.GetContext().Dates.Add(newDate);
            ScheduleBaseEntities.GetContext().SaveChanges();
            SelectedLesson.Dates_Id = newDate.Id;
            Lessons newl = ScheduleBaseEntities.GetContext().Lessons.Add(SelectedLesson);
            SelectedLesson = newl;
            ScheduleBaseEntities.GetContext().SaveChanges();
            LessonsList = ScheduleBaseEntities.GetContext().Lessons.ToList();
            isAdd = true;
            lessonEdit.ShowDialog();
            LecVisible = Visibility.Collapsed;
            SemVisible = Visibility.Collapsed;
            LabVisible = Visibility.Collapsed;
        }
        private void DeleteDate(object obj) // obj = SelectedLesson
        {
            if (MessageBox.Show("Вы точно хотите удалить эту пару?", "Внимание",
                   MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ScheduleBaseEntities.GetContext().Dates.Remove(ScheduleBaseEntities.GetContext().
                    Dates.Where(d => SelectedLesson.Dates_Id == d.Id).FirstOrDefault());
                ScheduleBaseEntities.GetContext().Eventes.RemoveRange(ScheduleBaseEntities.GetContext().Eventes.
                    Where(e => e.Lessons_Id == SelectedLesson.Id));
                ScheduleBaseEntities.GetContext().Lessons.Remove(SelectedLesson);
                
                ScheduleBaseEntities.GetContext().SaveChanges();
                LessonsList = ScheduleBaseEntities.GetContext().Lessons.AsNoTracking().ToList();
            }
            if (LessonsList.Count != 0)
                SelectedLesson = LessonsList[0];
        }
        private void SaveDate(object obj) 
        {
            StringBuilder error = new StringBuilder();
            Dates newDate = ScheduleBaseEntities.GetContext().Dates.
                Where(d => d.Id == SelectedLesson.Dates_Id).FirstOrDefault();
            newDate = SelectedLesson.Dates;
            Lessons newLesson = ScheduleBaseEntities.GetContext().Lessons.
                Where(l => l.Id == SelectedLesson.Id).FirstOrDefault();
            newLesson = SelectedLesson;
            if (string.IsNullOrWhiteSpace(newLesson.Dates.Start.ToString()))
                error.AppendLine("Недопустимое значение даты начала");
            if (string.IsNullOrWhiteSpace(newLesson.Dates.End.ToString()))
                error.AppendLine("Недопустимое значение даты конца");
            if (newLesson.Dates.Start > newLesson.Dates.End)
                error.AppendLine("Дата начала позже даты конца");
            if (string.IsNullOrWhiteSpace(newLesson.Teachers.FIO))
                error.AppendLine("Недопустимое значение ФИО преподавателя");
            if (string.IsNullOrWhiteSpace(newLesson.Dates.Times.ToString()))
                error.AppendLine("Недопустимое значение номера пары");
            if (string.IsNullOrWhiteSpace(newLesson.Dates.Week_days.Name))
                error.AppendLine("Недопустимое значение дня недели");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ScheduleBaseEntities.GetContext().SaveChanges();
            LessonsList = ScheduleBaseEntities.GetContext().Lessons.ToList();
            isAdd = false;
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinLessonEdit");
            win.Close();
        }
        private void CancelDate(object obj) 
        {
            if (isAdd)
            {
                ScheduleBaseEntities.GetContext().Dates.Remove(ScheduleBaseEntities.GetContext().
                    Dates.Where(d => SelectedLesson.Dates_Id == d.Id).FirstOrDefault());
                ScheduleBaseEntities.GetContext().SaveChanges();
            }
            else
                SelectedLesson = ScheduleBaseEntities.GetContext().Lessons.
                    Where(l => l.Id == SelectedLesson.Id).FirstOrDefault();
            isAdd = false;
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinLessonEdit");
            win.Close();
            LessonsList = ScheduleBaseEntities.GetContext().Lessons.AsNoTracking().ToList();
            SelectedLesson = LessonsList[0];
        }
        /// <summary>
        /// Вызывает окно для редактирования информации о выбранной дисциплине
        /// </summary>
        private void EditWinShow(object obj)
        {
            DisciplineEdit disciplineEditW = new DisciplineEdit(this);
            disciplineEditW.ShowDialog();
        }
        /// <summary>
        /// Вызывает окно для добавления новой дисциплины
        /// </summary>
        private void AddDisciplineShow(object obj)
        {
            DisciplineEdit disciplineEditW = new DisciplineEdit(this);
            //Teachers newt = new Teachers();

            SelectedDiscipline = new Disciplines();
            SelectedDiscipline.Name = "Наименование";
            Disciplines newd = ScheduleBaseEntities.GetContext().Disciplines.Add(SelectedDiscipline);
            SelectedDiscipline = newd;
            ScheduleBaseEntities.GetContext().SaveChanges();
            DisciplinesList = ScheduleBaseEntities.GetContext().Disciplines.OrderBy(d => d.Name).ToList();
            isAdd = true;

            disciplineEditW.ShowDialog();
        }
        /// <summary>
        /// Удаляет выбранную дисциплину
        /// </summary>
        private void DeleteDiscipline(object obj)
        {
            if (MessageBox.Show("Вы точно хотите удалить эту дисциплину?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ScheduleBaseEntities.GetContext().Disciplines.Remove(SelectedDiscipline);
                ScheduleBaseEntities.GetContext().Lessons.RemoveRange(ScheduleBaseEntities.GetContext().Lessons.Where(l => l.Disciplines_Id == SelectedDiscipline.Id));
                ScheduleBaseEntities.GetContext().SaveChanges();
                DisciplinesList = ScheduleBaseEntities.GetContext().Disciplines.AsNoTracking().OrderBy(d => d.Name).ToList();
                LessonsList = ScheduleBaseEntities.GetContext().Lessons.AsNoTracking().ToList();
            }
            if (DisciplinesList.Count != 0)
                SelectedDiscipline = DisciplinesList[0];
        }
        /// <summary>
        /// Сохраняет изменения информации о дисциплине
        /// </summary>
        private void SaveDiscipline(object obj)
        {
            StringBuilder error = new StringBuilder();
            Disciplines newD = ScheduleBaseEntities.GetContext().Disciplines.
                Where(d => d.Id == SelectedDiscipline.Id).FirstOrDefault();
            newD = SelectedDiscipline;
            if (string.IsNullOrWhiteSpace(newD.Name))
                error.AppendLine("Укажите наименование предмета");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            isAdd = false;
            ScheduleBaseEntities.GetContext().SaveChanges();
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinDesciplineEdite");
            win.Close();
            if (DisciplinesList.Count != 0)
                SelectedDiscipline = DisciplinesList[0];
        }
        /// <summary>
        /// Отменяет изменения информации о дисциплине
        /// </summary>
        private void CancelDiscipline(object obj)
        {
            if (isAdd)
            {
                ScheduleBaseEntities.GetContext().Disciplines.Remove(SelectedDiscipline);
                ScheduleBaseEntities.GetContext().SaveChanges();
            }
            else
                SelectedDiscipline = ScheduleBaseEntities.GetContext().Disciplines.
                    Where(t => t.Id == SelectedDiscipline.Id).FirstOrDefault();
            isAdd = false;
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "WinDesciplineEdite");
            win.Close();
            DisciplinesList = ScheduleBaseEntities.GetContext().Disciplines.AsNoTracking().OrderBy(d => d.Name).ToList();
            if (DisciplinesList.Count != 0)
                SelectedDiscipline = DisciplinesList[0];
        }

        //Функции представления
        public ICommand ShowLecCommand
        {
            get; set;
        }
        public ICommand ShowSemCommand
        {
            get; set;
        }
        public ICommand ShowLabCommand
        {
            get; set;
        }
        //Основные функции
        //Функции редактирования дат
        public ICommand EditDatesCommand
        {
            get; set;
        }
        public ICommand AddDateCommand
        {
            get; set;
        }
        public ICommand DeleteDateCommand
        {
            get; set;
        }
        //Функции окна редактирования дат
        public ICommand CancelDateCommand
        {
            get; set;
        }
        public ICommand SaveDateCommand
        {
            get; set;
        }
        //Функции работы с предметом
        public ICommand EditCommand
        {
            get; set;
        }
        public ICommand AddCommandShow
        {
            get; set;
        }
        public ICommand DeleteCommand
        {
            get; set;
        }
        //Функции окна редактирования предмета
        public ICommand SaveCommand
        {
            get; set;
        }
        public ICommand CancelCommand
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
