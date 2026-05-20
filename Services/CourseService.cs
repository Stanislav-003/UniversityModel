using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class CourseService : ICourseService
{
    private readonly UniversityStorage storage;

    public CourseService(UniversityStorage storage)
    {
        this.storage = storage;
    }

    public void Create(Course course)
    {
        if (storage.Data.Courses.Any())
        {
            course.Id = storage.Data.Courses.Max(c => c.Id) + 1;
        }
        else
        { 
            course.Id = 1;
        }

        storage.Data.Courses.Add(course);
        storage.Save();
    }

    public IEnumerable<Course> GetAll()
    {
        return storage.Data.Courses;
    }

    public Course? GetById(int id)
    {
        return storage.Data.Courses.FirstOrDefault(c => c.Id == id);
    }

    public void Remove(int id)
    {
        var course = GetById(id);

        if (course == null)
        { 
            return;
        }

        if (course.Teacher != null)
        {
            course.Teacher.CourseIds.Remove(id);
            course.Teacher.Courses.Remove(course);
        }

        foreach (var student in course.Students)
        {
            student.CourseIds.Remove(id);
            student.Courses.Remove(course);
        }

        storage.Data.Courses.Remove(course);
        storage.Save();
    }

    public void Update(Course course)
    {
        var existing = GetById(course.Id);

        if (existing == null)
        { 
            return;
        }

        existing.Name = course.Name;

        var newTeacher = course.Teacher;

        var oldTeacher = storage.Data.Persons.OfType<Teacher>().FirstOrDefault(t => t.CourseIds.Contains(existing.Id));

        if (oldTeacher != newTeacher)
        {
            if (oldTeacher != null)
            {
                oldTeacher.CourseIds.Remove(existing.Id);
                oldTeacher.Courses.Remove(existing);
            }

            existing.Teacher = newTeacher;

            if (newTeacher != null)
            {
                if (!newTeacher.CourseIds.Contains(existing.Id))
                {
                    newTeacher.CourseIds.Add(existing.Id);
                }
                if (!newTeacher.Courses.Contains(existing))
                {
                    newTeacher.Courses.Add(existing);
                }
            }
        }

        storage.Save();
    }
}
