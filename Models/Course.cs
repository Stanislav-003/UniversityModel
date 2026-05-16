using PropertyChanged;

namespace UniversityModel.Models;

[AddINotifyPropertyChangedInterface]
public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
}