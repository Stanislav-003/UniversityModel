using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.DTOs;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class AddStudentViewModel
{
    private readonly IStudentService studentService;
    public Student NewStudent { get; set; } = new();
    public ObservableCollection<CourseSelectableItem> Courses { get; set; } = new();

    public event Action<bool?>? RequestClose;
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddStudentViewModel(IStudentService studentService, IEnumerable<Course> courses)
    {
        this.studentService = studentService;
        PrepareModels(courses);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Course> courses)
    {
        Courses = new ObservableCollection<CourseSelectableItem>(courses.Select(c => new CourseSelectableItem { Course = c, IsSelected = false }));
    }

    private void Save()
    {
        try
        {
            NewStudent.CourseIds = Courses.Where(x => x.IsSelected).Select(x => x.Course.Id).ToList();
            
            studentService.Create(NewStudent);

            MessageBox.Show("Student was created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch (Exception)
        {
            MessageBox.Show("An error occurred while creating the student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}