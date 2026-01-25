namespace TheatreTicketSystem.Service.Interfaces
{
    public interface IChatService
    {
        Task<string> GetResponseAsync(string userMessage);
    }
}
