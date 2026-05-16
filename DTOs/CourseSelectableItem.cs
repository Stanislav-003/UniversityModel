using UniversityModel.Models;

namespace UniversityModel.DTOs;

public class CourseSelectableItem
{
    public Course Course { get; set; } = default!;
    public bool IsSelected { get; set; }
}
