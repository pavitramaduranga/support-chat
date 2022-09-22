namespace Client.API.Services
{
    public interface ISessionQueueService
    {
        public void PublishMessageToSessionQueue(string message);
    }
}
