using UniversityModel.Models;

namespace UniversityModel.Abstractions.Services;

public interface IBaseService<T>
{
    IEnumerable<T>? GetAll();
    T? GetById(Guid id);
}