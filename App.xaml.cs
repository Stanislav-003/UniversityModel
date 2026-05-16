using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using UniversityModel.Abstractions.Factories;
using UniversityModel.Abstractions.Services;
using UniversityModel.Factories;
using UniversityModel.Helpers;
using UniversityModel.Models;
using UniversityModel.Services;
using UniversityModel.ViewModels;

namespace UniversityModel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    public App()
    {
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<IStudentService, StudentService>();
                services.AddScoped<ITeatcherService, TeatcherService>();
                services.AddScoped<ICourseService, CourseService>();

                services.AddSingleton<IStorageFactory, StorageFactory>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        _host.Start();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }
}
