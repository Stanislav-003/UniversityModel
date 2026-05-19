using PropertyChanged;
using Newtonsoft.Json;

namespace UniversityModel.Models;

[AddINotifyPropertyChangedInterface]
public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [JsonIgnore]
    public Teacher Teacher { get; set; } = new();

    [JsonIgnore]
    public List<Student> Students { get; set; } = new();
}