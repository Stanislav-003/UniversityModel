using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for AddStudentView.xaml
    /// </summary>
    public partial class AddStudentView : Window
    {
        public AddStudentView()
        {
            InitializeComponent();
        }

        public void BindViewModel(AddStudentViewModel vm)
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
