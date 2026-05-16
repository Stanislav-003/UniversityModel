using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for AddCourseView.xaml
    /// </summary>
    public partial class AddCourseView : Window
    {
        public AddCourseView()
        {
            InitializeComponent();
        }

        public void BindViewModel(AddCourseViewModel vm)
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
