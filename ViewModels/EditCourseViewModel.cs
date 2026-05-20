using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;
using UniversityModel.Services;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class EditCourseViewModel
{
    private readonly ICourseService courseService;
    public Course EditingCourse { get; set; } = new();
    public ObservableCollection<Teacher> AvailableTeachers { get; set; } = new();
    public Teacher? SelectedTeacher { get; set; } = new();

    public event Action<bool?>? RequestClose;
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditCourseViewModel(ICourseService courseService, IEnumerable<Teacher> teachers, Course course)
    {
        this.courseService = courseService;
        PrepareModels(teachers, course);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Teacher> teachers, Course course)
    {
        AvailableTeachers = new ObservableCollection<Teacher>(teachers);
        EditingCourse = course;
        SelectedTeacher = course.Teacher;
    }

    private void Save()
    {
        try
        {
            if (SelectedTeacher == null)
            {
                return;
            }

            EditingCourse.Teacher = SelectedTeacher;
            
            courseService.Update(EditingCourse);

            MessageBox.Show("Course updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch
        {
            MessageBox.Show("Error while updating course", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}