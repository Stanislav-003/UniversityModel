using UniversityModel.Models;

namespace UniversityModel.Abstractions.Services;

public interface IStudentService : IBaseService<Student>
{
    void Create(Student student);
    void Update(Student student);
    void Remove(Student student);
}
