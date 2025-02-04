using System;
using System.IO;
using Newtonsoft.Json;

public static class Config
{
    public static string ApiBaseUrl { get; private set; }

    static Config()
    {
        LoadConfig();
    }

    public static void LoadConfig()
    {
        try
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                dynamic config = JsonConvert.DeserializeObject(json);
                ApiBaseUrl = config.api_base_url;
            }
            else
            {
                ApiBaseUrl = "http://localhost/mbv/"; // URL padrão caso o arquivo não exista
            }
        }
        catch (Exception ex)
        {
            ApiBaseUrl = "http://localhost/mbv/"; // URL padrão em caso de erro
            Console.WriteLine("Erro ao carregar configuração: " + ex.Message);
        }
    }
}
