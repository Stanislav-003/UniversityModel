using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for EditTeacherView.xaml
    /// </summary>
    public partial class EditTeacherView : Window
    {
        public EditTeacherView()
        {
            InitializeComponent();
        }

        public void BindViewModel(EditTeacherViewModel vm)
        {
            DataContext = vm;

            vm.RequestClose += result =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
