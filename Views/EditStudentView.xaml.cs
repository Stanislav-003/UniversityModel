using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for EditStudentView.xaml
    /// </summary>
    public partial class EditStudentView : Window
    {
        public EditStudentView()
        {
            InitializeComponent();
        }

        public void BindViewModel(EditStudentViewModel vm)
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
