
using CommandLine;
using ConsoleApp2.utils;

class Program
{
    private const string BaseCatUrl = "https://cataas.com/cat";
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

            string path = ExtractApiUrl(arguments);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(path);

                if (responseMessage.IsSuccessStatusCode)
                {
                    await WriteResponseToDisk(arguments, responseMessage);
                }
                else
                {
                    throw new Exception(responseMessage.StatusCode.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static async Task WriteResponseToDisk(Arguments arguments, HttpResponseMessage responseMessage)
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

    private static string ExtractApiUrl(Arguments arguments)
    {
        string path = BaseCatUrl;

        if (!string.IsNullOrWhiteSpace(arguments.CatSaysText))
        {
            path += $"/says/{arguments.CatSaysText}";
        }

        return path;
    }
}