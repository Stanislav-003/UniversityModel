using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class StudentService : IStudentService
{
    private readonly UniversityStorage storage;
    
    public StudentService(UniversityStorage storage)
    { 
        this.storage = storage;
    }

    public void Create(Student student)
    {
        storage.Data.Persons.Add(student);

        student.Courses = storage.Data.Courses.Where(c => student.CourseIds.Contains(c.Id)).ToList();
        
        foreach (var course in student.Courses)
        {
            if (!course.Students.Contains(student))
            { 
                course.Students.Add(student);
            }    
        }

        storage.Save();
    }

    public IEnumerable<Student> GetAll()
    {
        return storage.Data.Persons.OfType<Student>();
    }

    public void Remove(Student student)
    {
        foreach (var course in student.Courses)
        {
            course.Students.Remove(student);
        }

        storage.Data.Persons.Remove(student);
        storage.Save();
    }

    public void Update(Student student)
    {
        student.Courses = storage.Data.Courses.Where(c => student.CourseIds.Contains(c.Id)).ToList();

        foreach (var course in storage.Data.Courses)
        {
            if (student.CourseIds.Contains(course.Id))
            {
                if (!course.Students.Contains(student))
                { 
                    course.Students.Add(student);
                }
            }
            else
            {
                course.Students.Remove(student);
            }
        }

        storage.Save();
    }
}
