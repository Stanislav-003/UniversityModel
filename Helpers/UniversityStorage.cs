using Newtonsoft.Json;
using System.IO;
using UniversityModel.Converters;
using UniversityModel.Models;

namespace UniversityModel.Helpers;

public class UniversityStorage
{
    private readonly string filePath;
    private readonly JsonSerializerSettings settings;

    public University Data { get; private set; }

    public UniversityStorage()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        this.filePath = Path.Combine(basePath, "university.json");

        this.settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        
        this.settings.Converters.Add(new PersonConverter());

        Data = Load();
    }

    private University Load()
    {
        if (!File.Exists(filePath))
        {
            return new University();
        }

        try
        {
            var json = File.ReadAllText(filePath);
            var university = JsonConvert.DeserializeObject<University>(json, settings) ?? new University();

            university.PostDeserialize();

            return university;
        }
        catch
        {
            return new University();
        }
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(Data, settings);
        File.WriteAllText(filePath, json);
    }
}
