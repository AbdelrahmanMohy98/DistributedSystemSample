namespace ServiceB.Services
{
    public interface ITextFileService
    {
        Task WriteAsync(string text);
        Task<string> ReadAsync();
    }
}
