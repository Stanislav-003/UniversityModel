using UniversityModel.Abstractions.Factories;
using UniversityModel.Abstractions.Services;
using UniversityModel.Helpers;
using UniversityModel.Models;

namespace UniversityModel.Services;

public class TeatcherService : ITeatcherService
{
    private readonly JsonFileStorage<Teatcher> storage;

    public TeatcherService(IStorageFactory factory)
    {
        storage = factory.Create<Teatcher>();
    }

    public void Create(Teatcher teatcher)
    {
        teatcher.Id = Guid.NewGuid();
        storage.Add(teatcher);
    }

    public IEnumerable<Teatcher> GetAll()
    {
        return storage.GetAll();
    }

    public Teatcher? GetById(Guid id)
    {
        return storage.Find(t => t.Id == id);
    }

    public void Remove(Guid id)
    {
        storage.Remove(t => t.Id == id);
    }

    public void Update(Teatcher teacher)
    {
        var existing = storage.Find(x => x.Id == teacher.Id);

        if (existing == null)
        {
            return;
        }

        existing.FirstName = teacher.FirstName;
        existing.LastName = teacher.LastName;
        existing.Age = teacher.Age;
        existing.Gender = teacher.Gender;

        storage.Save();
    }
}
