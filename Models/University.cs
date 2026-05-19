namespace UniversityModel.Models;

public class University
{
    public string Name { get; set; } = string.Empty;
    public List<Person> Persons { get; set; } = new();
    public List<Course> Courses { get; set; } = new();

    public void PostDeserialize()
    {
        foreach (var person in Persons)
        {
            if (person is Student student)
            {
                student.Courses = Courses.Where(c => student.CourseIds.Contains(c.Id)).ToList();
            }
            else if (person is Teacher teacher)
            {
                teacher.Courses = Courses.Where(c => teacher.CourseIds.Contains(c.Id)).ToList();
            }
        }

        foreach (var course in Courses)
        {
            course.Teacher = Persons.OfType<Teacher>().FirstOrDefault(t => t.CourseIds.Contains(course.Id))!;
            course.Students = Persons.OfType<Student>().Where(s => s.CourseIds.Contains(course.Id)).ToList();
        }
    }
}