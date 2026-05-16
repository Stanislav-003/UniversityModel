using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for AddTeacherView.xaml
    /// </summary>
    public partial class AddTeacherView : Window
    {
        public AddTeacherView()
        {
            InitializeComponent();
        }

        public void BindViewModel(AddTeacherViewModel vm)
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
