using System.Collections.ObjectModel;

namespace UniversityModel.DTOs;

public class TeacherDetails
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty; public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;

    public ObservableCollection<string> Courses { get; set; } = new();
}