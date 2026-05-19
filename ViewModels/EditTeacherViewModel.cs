using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class EditTeacherViewModel
{
    private readonly ITeacherService teacherService;
    public Teacher EditingTeacher { get; set; } = new();

    public event Action<bool?>? RequestClose;
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditTeacherViewModel(ITeacherService teacherService, Teacher teacher)
    {
        this.teacherService = teacherService;
        PrepareModels(teacher);
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void PrepareModels(Teacher teacher)
    {
        EditingTeacher = new Teacher
        {
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Age = teacher.Age,
            Gender = teacher.Gender,
        };
    }

    private void Save()
    {
        try
        {
            teacherService.Update(EditingTeacher);

            MessageBox.Show("Teacher was edited successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(true);
        }
        catch
        {
            MessageBox.Show("Error while editing teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}