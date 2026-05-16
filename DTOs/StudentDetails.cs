using System.Collections.ObjectModel;
using UniversityModel.ViewModels;

namespace UniversityModel.DTOs;

public class StudentDetails
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public ObservableCollection<CourseWithTeacherStepModel> Courses { get; set; } = new();
}
