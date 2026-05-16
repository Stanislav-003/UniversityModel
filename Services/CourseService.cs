using UniversityModel.Abstractions.Factories;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class CourseService : ICourseService
{
    private readonly JsonFileStorage<Course> storage;

    public CourseService(IStorageFactory factory)
    {
        storage = factory.Create<Course>();
    }

    public void Create(Course course)
    {
        course.Id = Guid.NewGuid();
        storage.Add(course);
    }

    public IEnumerable<Course> GetAll()
    {
        return storage.GetAll();
    }

    public Course? GetById(Guid id)
    {
        return storage.Find(c => c.Id == id);
    }

    public void Update(Course course)
    {
        var existing = storage.Find(x => x.Id == course.Id);

        if (existing == null)
        { 
            return;
        }

        existing.Name = course.Name;
        existing.TeacherId = course.TeacherId;

        storage.Save();
    }

    public void Remove(Guid id)
    {
        storage.Remove(c => c.Id == id);
    }
}
