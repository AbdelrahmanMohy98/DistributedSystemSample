namespace ServiceB.Services
{
    public class TextFileService : ITextFileService
    {
        private readonly string _filePath;

        public TextFileService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine( env.ContentRootPath, "Data", "messages.txt");

            Directory.CreateDirectory(
                Path.GetDirectoryName(_filePath)!);
        }

        public async Task WriteAsync(string text)
        {
            await File.WriteAllTextAsync(_filePath, text);
        }

        public async Task<string> ReadAsync()
        {
            if (!File.Exists(_filePath))
                return string.Empty;

            return await File.ReadAllTextAsync(_filePath);
        }
    }
}
