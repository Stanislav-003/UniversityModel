using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UniversityModel.Abstractions.Services;
using UniversityModel.DTOs;
using UniversityModel.Helpers;
using UniversityModel.Models;
using UniversityModel.Views;

namespace UniversityModel.ViewModels;

[AddINotifyPropertyChangedInterface]
public class MainViewModel
{
    private readonly ICourseService courseService;
    private readonly ITeatcherService teatcherService;
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
    public ObservableCollection<Teatcher> Teachers { get; } = new();
    public ObservableCollection<Course> Courses { get; } = new();

    public Student? SelectedStudent { get; set; }
    public Teatcher? SelectedTeacher { get; set; }
    public Course? SelectedCourse { get; set; }

    public StudentDetails? SelectedStudentDetails { get; set; }
    public TeacherDetails? SelectedTeacherDetails { get; set; }
    public CourseDetails? SelectedCourseDetails { get; set; }

    private bool CanEditStudent() => SelectedStudent != null;
    private bool CanDeleteStudent() => SelectedStudent != null;

    private bool CanEditTeacher() => SelectedTeacher != null;
    private bool CanDeleteTeacher() => SelectedTeacher != null;

    private bool CanEditCourse() => SelectedCourse != null;
    private bool CanDeleteCourse() => SelectedCourse != null;

    public MainViewModel(
        ICourseService courseService,
        ITeatcherService teatcherService,
        IStudentService studentService)
    {
        this.courseService = courseService;
        this.teatcherService = teatcherService;
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
        var selectedStudentId = SelectedStudent?.Id;
        var selectedTeacherId = SelectedTeacher?.Id;
        var selectedCourseId = SelectedCourse?.Id;

        LoadStudents();
        LoadTeatchers();
        LoadCourses();

        SelectedStudent = selectedStudentId.HasValue ? Students.FirstOrDefault(s => s.Id == selectedStudentId) : null;
        SelectedTeacher = selectedTeacherId.HasValue ? Teachers.FirstOrDefault(t => t.Id == selectedTeacherId) : null;
        SelectedCourse = selectedCourseId.HasValue ? Courses.FirstOrDefault(c => c.Id == selectedCourseId) : null;
    }

    private void OnSelectedStudentChanged()
    {
        if (SelectedStudent == null)
        {
            return;
        }

        var details = new StudentDetails
        {
            FirstName = SelectedStudent.FirstName,
            LastName = SelectedStudent.LastName,
            Age = SelectedStudent.Age,
            Gender = SelectedStudent.Gender
        };

        foreach (var courseId in SelectedStudent.CourseIds)
        {
            var course = Courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                continue;
            }

            var teacher = Teachers.FirstOrDefault(t => t.Id == course.TeacherId);

            var teacherName = "No Teacher";

            if (teacher != null && teacher.FirstName != null && teacher.LastName != null)
            {
                teacherName = $"{teacher.FirstName} {teacher.LastName}";
            }

            details.Courses.Add(new CourseWithTeacherStepModel
            {
                CourseName = course.Name,
                TeacherName = teacherName
            });
        }

        SelectedStudentDetails = details;
    }
    private void OnSelectedTeacherChanged()
    {
        if (SelectedTeacher == null)
        {
            return;
        }

        var details = new TeacherDetails
        {
            FirstName = SelectedTeacher.FirstName,
            LastName = SelectedTeacher.LastName,
            Age = SelectedTeacher.Age,
            Gender = SelectedTeacher.Gender
        };

        var teacherCourses = Courses.Where(c => c.TeacherId == SelectedTeacher.Id);

        foreach (var course in teacherCourses)
        {
            details.Courses.Add(course.Name);
        }

        SelectedTeacherDetails = details;
    }
    private void OnSelectedCourseChanged()
    {
        if (SelectedCourse == null)
        {
            return;
        }

        var teacher = Teachers.FirstOrDefault(t => t.Id == SelectedCourse.TeacherId);

        var teacherName = "No Teacher";

        if (teacher != null && teacher.FirstName != null && teacher.LastName != null)
        {
            teacherName = $"{teacher.FirstName} {teacher.LastName}";
        }

        SelectedCourseDetails = new CourseDetails
        {
            CourseName = SelectedCourse.Name,
            TeacherName = teacherName
        };
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

        var viewModel = new EditCourseViewModel(
            courseService,
            Teachers,
            SelectedCourse);

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

        var result = MessageBox.Show(
            $"Are you sure you want to delete course {SelectedCourse.Name}?",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        courseService.Remove(SelectedCourse.Id);

        SelectedCourse = null;
        SelectedCourseDetails = null;

        UpdateAllData();
    }

    private void DeleteStudent()
    {
        if (SelectedStudent == null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete {SelectedStudent.FirstName} {SelectedStudent.LastName}?",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        studentService.Remove(SelectedStudent.Id);

        SelectedStudent = null;
        SelectedStudentDetails = null;

        UpdateAllData();
    }

    private void OpenAddTeacher()
    {
        var viewModel = new AddTeacherViewModel(teatcherService, Courses);

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

        var viewModel = new EditTeacherViewModel(
            teatcherService,
            SelectedTeacher);

        var window = new EditTeacherView();

        window.BindViewModel(viewModel);

        window.ShowDialog();

        UpdateAllData();
    }

    private void DeleteTeacher()
    {
        if (SelectedTeacher == null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete {SelectedTeacher.FirstName} {SelectedTeacher.LastName}?",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        teatcherService.Remove(SelectedTeacher.Id);

        SelectedTeacher = null;
        SelectedTeacherDetails = null;

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

        IEnumerable<Teatcher>? teachers = teatcherService.GetAll();

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