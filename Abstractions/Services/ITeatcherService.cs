using UniversityModel.Models;

namespace UniversityModel.Abstractions.Services;

public interface ITeatcherService : IBaseService<Teatcher>
{
    void Create(Teatcher teatcher);
    void Update(Teatcher teacher);
    void Remove(Guid id);
}
