using System.IO;
using UniversityModel.Abstractions.Factories;
using UniversityModel.Helpers;

namespace UniversityModel.Factories;

public class StorageFactory : IStorageFactory
{
    private readonly string basePath;

    public StorageFactory()
    {
        basePath = AppDomain.CurrentDomain.BaseDirectory;
    }

    public JsonFileStorage<T> Create<T>()
    {
        var fileName = typeof(T).Name.ToLower() + ".json";

        var path = Path.Combine(basePath, fileName);

        return new JsonFileStorage<T>(path);
    }
}
