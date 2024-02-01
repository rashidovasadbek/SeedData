using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var countries =
    JsonConvert.DeserializeObject<List<JObject>>(
        await File.ReadAllTextAsync(@"D:\SeedData\SeedData\json\data.json"));

var languages =
    JsonConvert.DeserializeObject<List<JObject>>(
        await File.ReadAllTextAsync(@"D:\SeedData\SeedData\json\language.json"));

var resultCountries = countries!.Where(country =>
        country["currencies"] is not null
        && country["currencies"]!.First!.First!["symbol"] is not null
        && country["currencies"]?.First is { First: not null }
        && country["name"]!["nativeName"] is not null
    ).Select(country =>
    {
        // Set location
        var locationValue = country["latlng"]!;
        var latitude = locationValue[0]!.Value<decimal>();
        var longitude = locationValue[1]!.Value<decimal>();

        // Set currency
        var firstCurrency = country["currencies"] ?? throw new InvalidOperationException(country.ToString());
        var currency = firstCurrency.First!.First!.Value<JObject>()!;

        //set country names
        var countryNames = country["name"]!["nativeName"]!.Value<JObject>()!.Properties();

        var countryName = countryNames
            .Select(countryName => new KeyValuePair<string, string>(countryName.Name,
                countryName.Value.Value<JObject>()!["common"]!.Value<string>()!))
            .FirstOrDefault(countryName => countryName.Key == "eng" || countryName.Key == "rus");

        var language = countryName.Value is not null
            ? languages!.FirstOrDefault(language => language["country"]!.Value<string>() == countryName.Value)
            : null;

        var countryLanguage = language is not null
            ? new
            {
                name = countryName.Value,
                code = language["locale"]!.Value<string>()!.Split('-')[0],
                locale = language["locale"]!.Value<string>()
            }
            : null;

        return (dynamic)new
        {
            id = Guid.NewGuid(),
            name = country["name"]!["official"]!.Value<string>(),
            location = new { latitude, longitude },
            currency = new
            {
                name = currency["name"]!.Value<string>()!,
                symbol = currency["symbol"]!.Value<string>()!,
                code = firstCurrency.First.Path
            },
            language = countryLanguage
        };
    })
    .Where(country => country.language is not null);

await File.WriteAllTextAsync(@"D:\SeedData\SeedData\json\result.json",
    JsonConvert.SerializeObject(resultCountries));