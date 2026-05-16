using UniversityModel.Models;

namespace UniversityModel.Abstractions.Services;

public interface ICourseService : IBaseService<Course>
{
    void Create(Course course);
    void Update(Course course);
    void Remove(Guid id);
}
