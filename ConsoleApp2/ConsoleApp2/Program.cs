
using CommandLine;
using ConsoleApp2.utils;

class Program
{
    private const string BaseCatUrl = "https://cataas.com/cat";
    private readonly static HttpClient _httpClient = new HttpClient();  
    static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Arguments>(args)
                .MapResult(async arguments => await HandleCommand(arguments), _ => Task.FromResult(1));
    }

    private static async Task HandleCommand(Arguments arguments)
    {
        try
        {
            ValidatePath(arguments);
            string path = GetApiUrl(arguments);
            await FetchAndWriteCatImageAsync(arguments, path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static async Task FetchAndWriteCatImageAsync(Arguments arguments, string path)
    {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(path);

            if (responseMessage.IsSuccessStatusCode)
            {
                await WriteResponseToDiskAsync(arguments, responseMessage);
            }
            else
            {
                throw new Exception($"Request to fetch cat image has failed with code: {responseMessage.StatusCode}");
            }
    }

    private static async Task WriteResponseToDiskAsync(Arguments arguments, HttpResponseMessage responseMessage)
    {
        using (Stream contentStream = await responseMessage.Content.ReadAsStreamAsync())
        {
            using (FileStream fileStream = new FileStream(arguments.OutputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await contentStream.CopyToAsync(fileStream);
            }
        }
    }

    private static void ValidatePath(Arguments arguments)
    {
        PathValidationService pathValidationService = new PathValidationService();

        arguments.OutputPath = pathValidationService.ValidatePath(arguments.OutputPath);
    }

    private static string GetApiUrl(Arguments arguments)
    {
        return string.IsNullOrWhiteSpace(arguments.CatSaysText) ? BaseCatUrl : $"{BaseCatUrl}/says/{arguments.CatSaysText}";
    }
}