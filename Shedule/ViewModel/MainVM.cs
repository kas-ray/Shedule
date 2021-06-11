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
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel; //псевдоним
using System.Drawing;

namespace Shedule.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private List<Lessons> lessonsList = new List<Lessons>();
        private List<Lessons> rightLessons = new List<Lessons>();
        private Lessons selectedLesson = new Lessons();
        private DateTime displayedDate = new DateTime();
        private List<Times> timesList = new List<Times>();
        private List<Eventes> eventesList = new List<Eventes>();
        private Eventes selectedEvent = new Eventes();
        private List<Disciplines> disciplinesList = new List<Disciplines>();

        private Visibility infoVisible = new Visibility();
        private Visibility visibleAdd = new Visibility();
        private Visibility visibleEditDelete = new Visibility();

        /// <summary>
        /// Список пар
        /// </summary>
        public List<Lessons> LessonsList
        {
            get { return lessonsList; }
            set
            {
                lessonsList = value;
                OnPropertyChanged("LessonsList");
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
        public Lessons SelectedLesson
        {
            get { return selectedLesson; }
            set
            {
                selectedLesson = value;
                if (selectedLesson != null)
                {
                    SelectedEvent = EventesList.SingleOrDefault(e => e.Lessons_Id == selectedLesson.Id && e.Date == displayedDate);         
                    if (SelectedEvent != null)
                    {
                        VisibleEditDelete = Visibility.Visible;
                        VisibleAdd = Visibility.Collapsed;
                    } else
                    {
                        VisibleEditDelete = Visibility.Collapsed;
                        VisibleAdd = Visibility.Visible;
                    }
                    InfoVisible = Visibility.Visible;
                }
                else
                {
                    SelectedEvent = null;
                    InfoVisible = Visibility.Collapsed;
                }
                OnPropertyChanged("SelectedLesson");
            }
        }
        /// <summary>
        /// Выбранная дата
        /// </summary>
        public DateTime DisplayedDate
        {
            get { return displayedDate; }
            set
            {
                displayedDate = value;
                RightLessons = ScheduleBaseEntities.GetContext().Lessons.
                    Where(l => l.Dates.Week_days.Id == (int)DisplayedDate.DayOfWeek && 
                    l.Dates.End >= DisplayedDate && l.Dates.Start <= DisplayedDate).
                    OrderBy( l=> l.Dates.Times_Id).ToList();

                if (RightLessons.Count != 0)
                    for (int i = 0; i < RightLessons.Count; i++)                       
                        if (RightLessons[i].Dates.Alternation)
                        {
                            bool delete = true;
                            DateTime altDate = RightLessons[i].Dates.Start;
                            while (altDate <= RightLessons[i].Dates.End)
                            {
                                if (altDate == displayedDate)
                                    delete = false;
                                altDate = altDate.AddDays(14);
                            }
                            if (delete)
                            {
                                RightLessons.Remove(RightLessons[i]);
                                i--;
                            }
                        }                               
                SelectedLesson = RightLessons.Count != 0 ? RightLessons[0] : null;               
                OnPropertyChanged("DisplayedDate");
               
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
        public Eventes SelectedEvent
        {
            get { return selectedEvent; }
            set
            {
                selectedEvent = value;
                OnPropertyChanged("SelectedEvent");
            }
        }
        public List<Eventes> EventesList
        {
            get { return eventesList; }
            set
            {
                eventesList = value;
                OnPropertyChanged("EventesList");
            }
        }

        public Visibility InfoVisible
        {
            get
            {
                return infoVisible;
            }
            set
            {
                infoVisible = value;
                OnPropertyChanged("InfoVisible");
            }
        }
        public Visibility VisibleAdd
        {
            get
            {
                return visibleAdd;
            }
            set
            {
                visibleAdd = value;
                OnPropertyChanged("VisibleAdd");
            }
        }
        public Visibility VisibleEditDelete
        {
            get
            {
                return visibleEditDelete;
            }
            set
            {
                visibleEditDelete = value;
                OnPropertyChanged("VisibleEditDelete");
            }
        }

        //для экселя
        public List<Disciplines> DisciplinesList
        {
            get { return disciplinesList; }
            set
            {
                disciplinesList = value;
                OnPropertyChanged("DisciplinesList");
            }
        }
        public MainVM()
        {
            PlusCommand = new DelegateCommand(DoPlus);
            MinusCommand = new DelegateCommand(DoMinus);

            ExportExelCommand = new DelegateCommand(ExportExel);

            EditEventCommand = new DelegateCommand(EditEvent);
            AddEventCommand = new DelegateCommand(AddEvent);
            DeleteEventCommand = new DelegateCommand(DeleteEvent);

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);

            LessonsList = ScheduleBaseEntities.GetContext().Lessons.OrderBy(l => l.Disciplines_Id).ToList();
            TimesList = ScheduleBaseEntities.GetContext().Times.ToList();
            DisciplinesList = ScheduleBaseEntities.GetContext().Disciplines.ToList();
            EventesList = ScheduleBaseEntities.GetContext().Eventes.ToList();
            DisplayedDate = DateTime.Today;          
        }
        private bool isAdd = false;
        private void EditEvent(object obj)
        {
            if (obj != null)
            {
                EventEdit eventEdit = new EventEdit(this);
                eventEdit.ShowDialog();
            }
            else
                MessageBox.Show("Пока нечего редактировать. Сначала добавьте событие.");
        }
        private void AddEvent(object obj)
        {
            if (obj != null)
            {
                MessageBox.Show("Событие созданно. Вы можете написать новое событие через запятую в существующем событии");
                return;
            }
            EventEdit eventEditW = new EventEdit(this);

            SelectedEvent = new Eventes { 
                Description = "Событие",
                Date = DisplayedDate,
                Lessons_Id = SelectedLesson.Id
            };
            
            Eventes newE = ScheduleBaseEntities.GetContext().Eventes.Add(SelectedEvent);
            SelectedEvent = newE;
            ScheduleBaseEntities.GetContext().SaveChanges();
            EventesList = ScheduleBaseEntities.GetContext().Eventes.ToList();
            isAdd = true;

            eventEditW.ShowDialog();
        }
        private void DeleteEvent(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("Пока нечего удалять");
                return;
            }
            if (MessageBox.Show("Вы точно хотите удалить это событие?", "Внимание",
       MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ScheduleBaseEntities.GetContext().Eventes.Remove(ScheduleBaseEntities.GetContext().
                    Eventes.Where(e => SelectedEvent.Id == e.Id).FirstOrDefault());
                ScheduleBaseEntities.GetContext().SaveChanges();
                EventesList = ScheduleBaseEntities.GetContext().Eventes.AsNoTracking().ToList();
                SelectedEvent = null;
            }           
        }
        private void Cancel(object obj)
        {
            if (isAdd)
            {
                ScheduleBaseEntities.GetContext().Eventes.Remove(SelectedEvent);
                ScheduleBaseEntities.GetContext().SaveChanges();
            }
            else
                SelectedEvent = ScheduleBaseEntities.GetContext().Eventes.
                    Where(e => e.Id == SelectedEvent.Id).FirstOrDefault();
            isAdd = false;
            System.Windows.Window win = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>()
                .SingleOrDefault(w => w.Name == "WinEventEdit");
            win.Close();
            EventesList = ScheduleBaseEntities.GetContext().Eventes.AsNoTracking().ToList();            
        }

        private void Save(object obj)
        {
            StringBuilder error = new StringBuilder();
            Eventes newE = ScheduleBaseEntities.GetContext().Eventes.
                Where(e => e.Id == SelectedEvent.Id).FirstOrDefault();
            newE = SelectedEvent;
            if (string.IsNullOrWhiteSpace(newE.Description))
                error.AppendLine("Укажите описание события");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            isAdd = false;
            ScheduleBaseEntities.GetContext().SaveChanges();
            System.Windows.Window win = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>()
                .SingleOrDefault(w => w.Name == "WinEventEdit");
            win.Close();
        }

        private void DoPlus(object obj)
        {
            DisplayedDate = DisplayedDate.AddDays(1);
        }
        private void DoMinus(object obj)
        {
            DisplayedDate = DisplayedDate.AddDays(-1);
        }
        private void ExportExel(object obj)
        {
            //Объявляем приложение
            Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            //Количество листов в рабочей книге
            ex.SheetsInNewWorkbook = 3;
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            //Отключить отображение окон с сообщениями
            ex.DisplayAlerts = false;
            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet1 = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            Excel.Worksheet sheet2 = (Excel.Worksheet)ex.Worksheets.get_Item(2);
            Excel.Worksheet sheet3 = (Excel.Worksheet)ex.Worksheets.get_Item(3);
            doSheet(1, "Лекции", sheet1);
            doSheet(2, "Семинары", sheet2);
            doSheet(3, "Лабораторные работы", sheet3);



            ex.Visible = true;
            ex.UserControl = true;

        }
        /// <summary>
        /// Задает лист в таблице Excel
        /// </summary>
        /// <param name="typeLesson">Тип пары (лекция = 1, семинар = 2, лабораторная работа = 3)</param>
        /// <param name="nameOfSheet">Название листа, которое подписано снизу</param>
        /// <param name="sheet">Лсист Excel</param>
        private void doSheet(int typeLesson, string nameOfSheet, Excel.Worksheet sheet)
        {           
            //Название листа (вкладки снизу)
            sheet.Name = nameOfSheet;
            for (int i = 1; i <= DisciplinesList.Count; i++) 
            {
                sheet.Cells[i, 1] = DisciplinesList[i - 1].Name;
                int countPassed = 0; 
                int countLeft = 0; 
                List<Lessons> correctLessons = LessonsList.Where
                    (l => l.Disciplines_Id == DisciplinesList[i - 1].Id && l.Lessons_types_Id == typeLesson).ToList();
                for (int j = 0; j < correctLessons.Count; j++) 
                {
                    int interval = correctLessons[j].Dates.Alternation ? 14 : 7;
                    DateTime myDay = correctLessons[j].Dates.Start;
  
                    while (myDay <= correctLessons[j].Dates.End)
                    {
                        if (myDay <= DateTime.Today)
                            countPassed++;
                        else
                            countLeft++;
                        myDay = myDay.AddDays(interval);
                    }
                }
                if (correctLessons.Count > 0)
                {
                    Excel.Range rangePassed = sheet.Range[sheet.Cells[i, 2], sheet.Cells[i, countPassed + 1]];
                    Excel.Range rangeLeft;
                    Excel.Range rangeFull;
                    if (countLeft != 0)
                    {
                        rangeLeft = sheet.Range[sheet.Cells[i, countPassed + 2], sheet.Cells[i, countLeft + countPassed + 1]];
                        rangeLeft.Cells.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                        rangeFull = sheet.Range[sheet.Cells[i, 2], sheet.Cells[i, countLeft + countPassed + 1]];

                    } 
                    else
                        rangeFull = rangePassed;
                    rangePassed.Cells.Interior.Color = ColorTranslator.ToOle(Color.LightGreen);
                    rangeFull.ColumnWidth = 4;
                    MakeBorders(rangeFull);
                    sheet.Cells[i, countLeft + countPassed + 2] = countPassed.ToString() + " из " + (countPassed + countLeft).ToString();
                    sheet.Cells[i, countLeft + countPassed + 2].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
            }

            Excel.Range range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[DisciplinesList.Count, 1]];
            range.ColumnWidth = 30;
            MakeBorders(range);
            //ex.Columns.EntireColumn.AutoFit();
            //ex.Rows.EntireRow.AutoFit();
            range.WrapText = true;
        }
        private void MakeBorders (Excel.Range range)
        {
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        public ICommand PlusCommand
        {
            get; set;
        }
        public ICommand MinusCommand
        {
            get; set;
        }
        public ICommand ExportExelCommand
        {
            get; set;
        }
        public ICommand EditEventCommand
        {
            get; set;
        }
        public ICommand AddEventCommand
        {
            get; set;
        }
        public ICommand DeleteEventCommand
        {
            get; set;
        }
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
