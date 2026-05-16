using UniversityModel.Helpers;

namespace UniversityModel.Abstractions.Factories;

public interface IStorageFactory
{
    JsonFileStorage<T> Create<T>();
}
