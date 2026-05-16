using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class EditCourseViewModel
{
    private readonly ICourseService courseService;

    public event Action<bool?>? RequestClose;

    public Course EditingCourse { get; set; } = new();

    public ObservableCollection<Teatcher> AvailableTeachers { get; set; } = new();

    public Teatcher? SelectedTeacher { get; set; } = new();

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditCourseViewModel(ICourseService courseService, IEnumerable<Teatcher> teachers, Course course)
    {
        this.courseService = courseService;

        PrepareModels(teachers, course);

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Teatcher> teachers, Course course)
    {
        EditingCourse = new Course
        {
            Id = course.Id,
            Name = course.Name,
            TeacherId = course.TeacherId
        };

        AvailableTeachers = new ObservableCollection<Teatcher>(teachers);

        SelectedTeacher = teachers.FirstOrDefault(t => t.Id == course.TeacherId);
    }

    private void Save()
    {
        try
        {
            if (SelectedTeacher != null)
            {
                EditingCourse.TeacherId = SelectedTeacher.Id;
            }

            courseService.Update(EditingCourse);

            MessageBox.Show(
                "Course updated successfully",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch
        {
            MessageBox.Show(
                "Error while updating course",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}