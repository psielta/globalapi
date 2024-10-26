using Microsoft.AspNetCore.SignalR;
namespace GlobalAPI_ACBrNFe.Lib
{
    public class ImportProgressHub : Hub
    {
        private readonly ILogger<ImportProgressHub> _logger;

        public ImportProgressHub(ILogger<ImportProgressHub> logger)
        {
            _logger = logger;
        }

        public async Task JoinSessionGroup(string sessionId)
        {
            _logger.LogInformation($"Joining session group {sessionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }
    }

}
