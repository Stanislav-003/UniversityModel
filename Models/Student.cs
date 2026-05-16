using PropertyChanged;

namespace UniversityModel.Models;

[AddINotifyPropertyChangedInterface]
public class Student : Person
{
    public List<Guid> CourseIds { get; set; } = new();
}