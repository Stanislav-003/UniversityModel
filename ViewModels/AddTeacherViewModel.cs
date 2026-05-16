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
public class AddTeacherViewModel
{
    private readonly ITeatcherService teacherService;

    public event Action<bool?>? RequestClose;

    public Teatcher NewTeacher { get; set; } = new();

    public ObservableCollection<CourseSelectableItem> AvailableCourses { get; set; } = new();

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddTeacherViewModel(ITeatcherService teacherService, IEnumerable<Course> courses)
    {
        this.teacherService = teacherService;

        PrepareModels(courses);

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Course> courses)
    {
        AvailableCourses = new ObservableCollection<CourseSelectableItem>(
            courses.Select(c => new CourseSelectableItem
            {
                Course = c,
                IsSelected = false
            }));
    }

    private void Save()
    {
        try
        {
            NewTeacher.Id = Guid.NewGuid();

            MessageBox.Show(
                "Teacher was created successfully",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            teacherService.Create(NewTeacher);

            RequestClose?.Invoke(true);
        }
        catch
        {
            MessageBox.Show(
                "Error while creating teacher",
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