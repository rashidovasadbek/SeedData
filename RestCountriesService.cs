using System.Reflection;
using Microsoft.AspNetCore.Hosting;

using SeedData.models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SeedData;

public static class RestCountriesService
{
  //  private static readonly IEnumerable<Country> Data;
    
    private const string DataSourcePath = "SeedData.json.data.json";
    private static IEnumerable<Country>? Data;
    
    /*public static async ValueTask<IEnumerable<Country>> GetAllCountries(IWebHostEnvironment environment)
    {
        var countries = JsonConvert.DeserializeObject<List<Country>>(await File.ReadAllTextAsync(
            Path.Combine(environment.ContentRootPath, "SeedData","SeedData","ata.json")))!;
        return countries;
    }*/

    public static IEnumerable<Country> GetAllCountries()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames();
        using var stream = assembly.GetManifestResourceStream(DataSourcePath);
        Data = JsonSerializer.Deserialize<IEnumerable<Country>>(new StreamReader(stream).ReadToEnd());
        return Data.OrderBy(c => c.Name).ToList();
    }

}