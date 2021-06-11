using Shedule.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shedule.View
{
    /// <summary>
    /// Логика взаимодействия для TeacherEdit.xaml
    /// </summary>
    public partial class TeacherEdit : Window
    {
        public TeacherEdit(TeachersVM currentModel)
        {
            InitializeComponent();
            DataContext = currentModel;
        }
    }
}
