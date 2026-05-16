using System.IO;
using System.Text.Json;

namespace UniversityModel.Helpers;

public class JsonFileStorage<T>
{
    private readonly string filePath;
    private readonly JsonSerializerOptions options;
    private List<T> Cache { get; set; } = new();

    public JsonFileStorage(string filePath)
    {
        this.filePath = filePath;

        this.options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        Cache = Load();
    }

    public List<T> Load()
    {
        if (!File.Exists(filePath))
        { 
            return new List<T>();
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, options) ?? new();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Cache, options);
        File.WriteAllText(filePath, json);
    }

    public IReadOnlyList<T> GetAll()
    { 
        return Cache;
    }

    public void Add(T item)
    {
        Cache.Add(item);
        Save();
    }

    public void Remove(Func<T, bool> predicate)
    {
        Cache.RemoveAll(x => predicate(x));
        Save();
    }

    public T? Find(Func<T, bool> predicate)
    {
        return Cache.FirstOrDefault(predicate);
    }
}
