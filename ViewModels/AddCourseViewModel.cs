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

    public event Action<bool?>? RequestClose;
    public Course NewCourse { get; set; } = new();
    public ObservableCollection<Teatcher> Teachers { get; set; } = new();
    public Teatcher? SelectedTeacher { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddCourseViewModel(ICourseService courseService, IEnumerable<Teatcher> teachers)
    {
        this.courseService = courseService;

        PrepareModels(teachers);

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(IEnumerable<Teatcher> teachers)
    {
        Teachers = new ObservableCollection<Teatcher>(teachers);
    }

    private void Save()
    {
        try
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show(
                    "Please select a teacher",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            NewCourse.TeacherId = SelectedTeacher.Id;

            courseService.Create(NewCourse);

            MessageBox.Show(
                "Course was created successfully",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch (Exception)
        {
            MessageBox.Show(
                "An error occurred while creating the course",
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
