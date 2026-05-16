using System.Windows;
using UniversityModel.ViewModels;

namespace UniversityModel.Views
{
    /// <summary>
    /// Interaction logic for EditCourseView.xaml
    /// </summary>
    public partial class EditCourseView : Window
    {
        public EditCourseView()
        {
            InitializeComponent();
        }

        public void BindViewModel(EditCourseViewModel vm)
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
