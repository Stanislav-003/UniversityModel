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
public class EditStudentViewModel
{
    private readonly IStudentService studentService;
    public Student EditingStudent { get; set; } = new();
    public ObservableCollection<CourseSelectableItem> AvailableCourses { get; set; } = new();

    public event Action<bool?>? RequestClose;
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditStudentViewModel(IStudentService studentService, IEnumerable<Course> courses, Student student)
    {
        this.studentService = studentService;
        PrepareModels(courses, student);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Course> courses, Student student)
    {
        this.EditingStudent = student;

        AvailableCourses = new ObservableCollection<CourseSelectableItem>(
            courses.Select(c => new CourseSelectableItem { Course = c, IsSelected = EditingStudent.CourseIds.Contains(c.Id) }));
    }

    private void Save()
    {
        try
        {
            EditingStudent.CourseIds = AvailableCourses.Where(x => x.IsSelected).Select(x => x.Course.Id).ToList();

            studentService.Update(EditingStudent);

            MessageBox.Show("Student was edited successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch (Exception)
        {
            MessageBox.Show("An error occurred while editing the student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}
