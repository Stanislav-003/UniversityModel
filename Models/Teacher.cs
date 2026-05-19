using PropertyChanged;
using Newtonsoft.Json;

namespace UniversityModel.Models;

[AddINotifyPropertyChangedInterface]
public class Teacher : Person
{
    private const string TYPE_NAME = "Teacher";
    public override string Type => TYPE_NAME;
    
    [JsonIgnore]
    public List<Course> Courses { get; set; } = new();
    public List<int> CourseIds { get; set; } = new();
}