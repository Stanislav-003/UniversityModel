using PropertyChanged;

namespace UniversityModel.Models;

[AddINotifyPropertyChangedInterface]
public abstract class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }

    public abstract string Type { get; }
}