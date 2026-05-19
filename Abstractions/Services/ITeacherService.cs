using UniversityModel.Models;

namespace UniversityModel.Abstractions.Services;

public interface ITeacherService : IBaseService<Teacher>
{
    void Create(Teacher teatcher);
    void Update(Teacher teacher);
    void Remove(Teacher teacher);
}