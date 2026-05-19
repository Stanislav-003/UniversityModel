using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;
using UniversityModel.Views;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class MainViewModel
{
    private readonly ICourseService courseService;
    private readonly ITeacherService teacherService;
    private readonly IStudentService studentService;

    public ICommand OpenAddStudentCommand { get; }
    public ICommand OpenAddCourseCommand { get; }
    public ICommand OpenEditStudentCommand { get; }
    public ICommand DeleteStudentCommand { get; }
    public ICommand OpenAddTeacherCommand { get; }
    public ICommand OpenEditTeacherCommand { get; }
    public ICommand DeleteTeacherCommand { get; }
    public ICommand OpenEditCourseCommand { get; }
    public ICommand DeleteCourseCommand { get; }

    public ObservableCollection<Student> Students { get; } = new();
    public ObservableCollection<Teacher> Teachers { get; } = new();
    public ObservableCollection<Course> Courses { get; } = new();

    public Student? SelectedStudent { get; set; }
    public Teacher? SelectedTeacher { get; set; }
    public Course? SelectedCourse { get; set; }

    public ObservableCollection<string> SelectedStudentCoursesWithTeachers { get; } = new();
    public ObservableCollection<string> SelectedTeacherCourseNames { get; } = new();
    public string SelectedCourseTeacherName { get; set; } = string.Empty;

    private bool CanEditStudent() => SelectedStudent != null;
    private bool CanDeleteStudent() => SelectedStudent != null;

    private bool CanEditTeacher() => SelectedTeacher != null;
    private bool CanDeleteTeacher() => SelectedTeacher != null;

    private bool CanEditCourse() => SelectedCourse != null;
    private bool CanDeleteCourse() => SelectedCourse != null;

    public MainViewModel(
        ICourseService courseService,
        ITeacherService teatcherService,
        IStudentService studentService)
    {
        this.courseService = courseService;
        this.teacherService = teatcherService;
        this.studentService = studentService;

        OpenAddStudentCommand = new RelayCommand(OpenAddStudent);
        OpenEditStudentCommand = new RelayCommand(OpenEditStudent, CanEditStudent);
        DeleteStudentCommand = new RelayCommand(DeleteStudent, CanDeleteStudent);
        OpenAddTeacherCommand = new RelayCommand(OpenAddTeacher);
        OpenEditTeacherCommand = new RelayCommand(OpenEditTeacher, CanEditTeacher);
        DeleteTeacherCommand = new RelayCommand(DeleteTeacher, CanDeleteTeacher);
        OpenAddCourseCommand = new RelayCommand(OpenAddCourse);
        OpenEditCourseCommand = new RelayCommand(OpenEditCourse, CanEditCourse);
        DeleteCourseCommand = new RelayCommand(DeleteCourse, CanDeleteCourse);
    }

    public void OnAppearing()
    {
        UpdateAllData();
    }

    private void UpdateAllData()
    {
        var prevStudent = SelectedStudent;
        var prevTeacher = SelectedTeacher;
        var prevCourseId = SelectedCourse?.Id;

        LoadStudents();
        LoadTeatchers();
        LoadCourses();

        SelectedStudent = prevStudent != null ? Students.FirstOrDefault(s => s.FirstName == prevStudent.FirstName && s.LastName == prevStudent.LastName) : null;
        SelectedTeacher = prevTeacher != null ? Teachers.FirstOrDefault(t => t.FirstName == prevTeacher.FirstName && t.LastName == prevTeacher.LastName) : null;
        SelectedCourse = prevCourseId.HasValue ? Courses.FirstOrDefault(c => c.Id == prevCourseId) : null;
    }

    private void OnSelectedStudentChanged()
    {
        SelectedStudentCoursesWithTeachers.Clear();

        if (SelectedStudent == null)
        { 
            return;
        }

        foreach (var course in SelectedStudent.Courses)
        {
            var teacherName = course.Teacher != null ? $"{course.Teacher.FirstName} {course.Teacher.LastName}" : "Unknown";
            SelectedStudentCoursesWithTeachers.Add($"{course.Name} (Teacher: {teacherName})");
        }
    }
    private void OnSelectedTeacherChanged()
    {
        SelectedTeacherCourseNames.Clear();

        if (SelectedTeacher == null)
        { 
            return;
        }

        foreach (var course in SelectedTeacher.Courses)
        {
            SelectedTeacherCourseNames.Add(course.Name);
        }
    }

    private void OnSelectedCourseChanged()
    {
        if (SelectedCourse == null)
        {
            SelectedCourseTeacherName = string.Empty;
            return;
        }

        if (SelectedCourse.Teacher != null)
        {
            SelectedCourseTeacherName = $"{SelectedCourse.Teacher.FirstName} {SelectedCourse.Teacher.LastName}";
        }
        else
        {
            SelectedCourseTeacherName = "Unknown";
        }
    }

    private void OpenAddStudent()
    {
        var viewModel = new AddStudentViewModel(studentService, Courses);
        var window = new AddStudentView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void OpenAddCourse()
    {
        var viewModel = new AddCourseViewModel(courseService, Teachers);
        var window = new AddCourseView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void OpenEditStudent()
    {
        if (SelectedStudent == null)
        {
            return;
        }

        var viewModel = new EditStudentViewModel(studentService, Courses, SelectedStudent);
        var window = new EditStudentView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void OpenEditCourse()
    {
        if (SelectedCourse == null)
        {
            return;
        }

        var viewModel = new EditCourseViewModel(courseService, Teachers, SelectedCourse);
        var window = new EditCourseView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void DeleteCourse()
    {
        if (SelectedCourse == null)
        {
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to delete course {SelectedCourse.Name}?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        courseService.Remove(SelectedCourse.Id);

        SelectedCourse = null;
        SelectedCourseTeacherName = string.Empty;
        UpdateAllData();
    }

    private void DeleteStudent()
    {
        if (SelectedStudent == null)
        {
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to delete {SelectedStudent.FirstName} {SelectedStudent.LastName}?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        studentService.Remove(SelectedStudent);

        SelectedStudent = null;
        SelectedStudentCoursesWithTeachers.Clear();
        UpdateAllData();
    }

    private void DeleteTeacher()
    {
        if (SelectedTeacher == null)
        {
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to delete {SelectedTeacher.FirstName} {SelectedTeacher.LastName}?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        teacherService.Remove(SelectedTeacher);

        SelectedTeacher = null;
        SelectedTeacherCourseNames.Clear();
        UpdateAllData();
    }

    private void OpenAddTeacher()
    {
        var viewModel = new AddTeacherViewModel(teacherService, Courses);
        var window = new AddTeacherView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void OpenEditTeacher()
    {
        if (SelectedTeacher == null)
        {
            return;
        }

        var viewModel = new EditTeacherViewModel(teacherService, SelectedTeacher);
        var window = new EditTeacherView();
        window.BindViewModel(viewModel);
        window.ShowDialog();
        UpdateAllData();
    }

    private void LoadStudents()
    {
        IEnumerable<Student>? students = studentService.GetAll();

        Students.Clear();

        if (students == null)
        {
            return;
        }

        foreach (var student in students)
        {
            Students.Add(student);
        }
    }

    private void LoadTeatchers()
    {
        Teachers.Clear();

        IEnumerable<Teacher>? teachers = teacherService.GetAll();

        if (teachers == null)
        {
            return;
        }

        foreach (var teacher in teachers)
        {
            Teachers.Add(teacher);
        }
    }

    private void LoadCourses()
    {
        Courses.Clear();

        IEnumerable<Course>? courses = courseService.GetAll();

        if (courses == null)
        {
            return;
        }

        foreach (var course in courses)
        {
            Courses.Add(course);
        }
    }
}