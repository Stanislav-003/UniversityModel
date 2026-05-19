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
public class AddCourseViewModel
{
    private readonly ICourseService courseService;
    public Course NewCourse { get; set; } = new();
    public ObservableCollection<Teacher> Teachers { get; set; } = new();
    public Teacher? SelectedTeacher { get; set; }

    public event Action<bool?>? RequestClose;
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddCourseViewModel(ICourseService courseService, IEnumerable<Teacher> teachers)
    {
        this.courseService = courseService;
        PrepareModels(teachers);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Teacher> teachers)
    {
        Teachers = new ObservableCollection<Teacher>(teachers);
    }

    private void Save()
    {
        try
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show("Please select a teacher", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            courseService.Create(NewCourse);

            if (!SelectedTeacher.CourseIds.Contains(NewCourse.Id))
            {
                SelectedTeacher.CourseIds.Add(NewCourse.Id);
            }
            
            NewCourse.Teacher = SelectedTeacher;
            
            if (!SelectedTeacher.Courses.Contains(NewCourse))
            {
                SelectedTeacher.Courses.Add(NewCourse);
            }

            courseService.Update(NewCourse);

            MessageBox.Show("Course was created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            RequestClose?.Invoke(true);
        }
        catch (Exception)
        {
            MessageBox.Show("An error occurred while creating the course", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}
