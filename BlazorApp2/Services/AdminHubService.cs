using BlazorApp2.Identity;
using BlazorApp2.Responses;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp2.Services
{
    public class AdminHubService : IAsyncDisposable
    {
        public event Func<VolunteerRegisteredEvent, Task>? OnVolunteerRegistered;
        public event Func<DisasterReportedEvent, Task>? OnDisasterReported;
        public event Func<TeamLocationEvent, Task>? OnTeamLocationUpdated;

        // OnToastNotification removed — HubToastContainer subscribes directly
        // to OnVolunteerRegistered / OnDisasterReported instead.

        private HubConnection? _hub;
        private bool _started;

        private readonly IAccountManagement _accountManagement;
        private readonly IConfiguration _configuration;

        public AdminHubService(IAccountManagement accountManagement, IConfiguration configuration)
        {
            _accountManagement = accountManagement;
            _configuration = configuration;
        }

        public HubConnectionState ConnectionState =>
            _hub?.State ?? HubConnectionState.Disconnected;

        public async Task StartAsync()
        {
            if (_started) return;
            _started = true;

            var baseUrl = "https://localhost:7244";

            // ── Load token BEFORE building the connection ──────────────────
            var tokenResponse = await _accountManagement.LoadAccessTokenFromStorage();

            if (tokenResponse?.ExpiresOnUtc <= DateTime.UtcNow)
                tokenResponse = await _accountManagement.RefreshTokenAsync();

            var token = tokenResponse?.AccessToken;

            Console.WriteLine($"[AdminHub] Token loaded: {(string.IsNullOrEmpty(token) ? "❌ NULL" : "✅ OK")}");

            _hub = new HubConnectionBuilder()
                .WithUrl($"{baseUrl.TrimEnd('/')}/hubs/admin", opts =>
                {
                    opts.AccessTokenProvider = async () =>
                    {
                        
                        var t = await _accountManagement.LoadAccessTokenFromStorage();
                        if (t?.ExpiresOnUtc <= DateTime.UtcNow)
                            t = await _accountManagement.RefreshTokenAsync();
                        return t?.AccessToken;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            // ── Volunteer registered ──────────────────────────────────────
            _hub.On<Guid, string, string, DateTimeOffset>(
                "NewVolunteerRegisterd",
                async (referenceId, message, url, requestedAt) =>
                {
                    var evt = new VolunteerRegisteredEvent(referenceId, message, url, requestedAt);

                    if (OnVolunteerRegistered is not null)
                        await OnVolunteerRegistered(evt);
                });

            _hub.On<Guid, string, double, double, string, DateTimeOffset>(
     "NewDisasterReported",
     async (referenceId, message, lat, lng, url, createdAt) =>
     {
         Console.WriteLine($"[AdminHub] ⚡ NewDisasterReported RECEIVED — {message}");

         var evt = new DisasterReportedEvent(
             referenceId, message, lat, lng, url, createdAt,
             new List<RecommendedTeamsResponse>(),      // server doesn't send these
             new List<RecommendedVolunteerResponse>()   // so we pass empty lists
         );

         if (OnDisasterReported is not null)
             await OnDisasterReported(evt);
     });

            // ── Team location ─────────────────────────────────────────────
            _hub.On<Guid, double, double>(
                "TeamLiveLocationUpdated",
                async (teamId, lat, lng) =>
                {
                    if (OnTeamLocationUpdated is not null)
                        await OnTeamLocationUpdated(new(teamId, lat, lng));
                });

            try
            {
                await _hub.StartAsync();
                Console.WriteLine("[AdminHub] Connected ✅");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AdminHub] Failed to connect: {ex.Message}");
                _started = false; // allow retry
            }

        }


        public async Task StopAsync()
        {
            if (_hub is not null)
                await _hub.StopAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hub is not null)
                await _hub.DisposeAsync();
        }
    }

    // ── Event records ──────────────────────────────────────────────────────

    public record VolunteerRegisteredEvent(
        Guid ReferenceId,
        string Message,
        string Url,
        DateTimeOffset RequestedAt);

    public record DisasterReportedEvent(
        Guid ReferenceId,
        string Message,
        double Latitude,
        double Longitude,
        string Url,
        DateTimeOffset CreatedAt,
        List<RecommendedTeamsResponse> RecommendedTeams,
        List<RecommendedVolunteerResponse> RecommendedVolunteers);

    public record TeamLocationEvent(
        Guid TeamId,
        double Latitude,
        double Longitude);

    public record HubToastNotification(
        string Title,
        string Message,
        string Icon,
        string Color,       // "success" | "critical" | "warning" | "info"
        string NavigateTo);
}