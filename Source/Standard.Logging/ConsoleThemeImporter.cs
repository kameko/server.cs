
namespace Standard.Logging
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    
    public static class ConsoleThemeImporter
    {
        public static async Task<IConsoleTheme> Import(string path)
        {
            try
            {
                var file  = await File.ReadAllTextAsync(path);
                var theme = JsonSerializer.Deserialize<ConsoleTheme>(file);
                if (theme is null)
                {
                    throw new InvalidOperationException("Deserialization failed.");
                }
                return theme;
            }
            catch
            {
                throw;
            }
        }
        
        public static async Task Export(IConsoleTheme theme, string path)
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented       = true,
                    AllowTrailingCommas = true,
                };
                var json = JsonSerializer.Serialize<IConsoleTheme>(theme, options);
                await File.WriteAllTextAsync(path, json);
            }
            catch
            {
                throw;
            }
        }
    }
}
