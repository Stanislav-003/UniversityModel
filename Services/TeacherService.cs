using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class TeacherService : ITeacherService
{
    private readonly UniversityStorage storage;
    
    public TeacherService(UniversityStorage storage)
    {
        this.storage = storage;
    }

    public void Create(Teacher teacher)
    {
        storage.Data.Persons.Add(teacher);

        teacher.Courses = storage.Data.Courses.Where(c => teacher.CourseIds.Contains(c.Id)).ToList();
        
        foreach (var course in teacher.Courses)
        {
            course.Teacher = teacher;
        }

        storage.Save();
    }

    public IEnumerable<Teacher> GetAll()
    {
        return storage.Data.Persons.OfType<Teacher>();
    }

    public void Remove(Teacher teacher)
    {
        foreach (var course in teacher.Courses)
        {
            course.Teacher = null;
        }

        storage.Data.Persons.Remove(teacher);
        storage.Save();
    }

    public void Update(Teacher teacher)
    {
        teacher.Courses = storage.Data.Courses.Where(c => teacher.CourseIds.Contains(c.Id)).ToList();

        foreach (var course in storage.Data.Courses)
        {
            if (teacher.CourseIds.Contains(course.Id))
            {
                course.Teacher = teacher;
            }
            else if (course.Teacher == teacher)
            {
                course.Teacher = null;
            }
        }

        storage.Save();
    }
}
