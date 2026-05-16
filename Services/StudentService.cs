using UniversityModel.Abstractions.Factories;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class StudentService : IStudentService
{
    private readonly JsonFileStorage<Student> storage;

    public StudentService(IStorageFactory factory)
    {
        storage = factory.Create<Student>();
    }

    public void Create(Student student)
    {
        student.Id = Guid.NewGuid();
        storage.Add(student);
    }

    public IEnumerable<Student> GetAll()
    {
        var students = storage.GetAll();
        return students;
    }

    public Student? GetById(Guid id)
    {
        var student = storage.Find(s => s.Id == id);
        return student;
    }

    public void Remove(Guid id)
    {
        storage.Remove(s => s.Id == id);
    }

    public void Update(Student student)
    {
        var existing = storage.Find(x => x.Id == student.Id);

        if (existing == null)
        { 
            return;
        }

        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.Age = student.Age;
        existing.Gender = student.Gender;
        existing.CourseIds = student.CourseIds;

        storage.Save();
    }
}
