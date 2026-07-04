using BlazorApp2.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp2.Services
{
     // ═══════════════════════════════════════════════════════════════
        // Events fired to subscribers (UI components)
        // ═══════════════════════════════════════════════════════════════

        public record TeamAssignedEvent(
            Guid TeamId,
            string ReferenceId,
            string Title,
            string City,
            string Message);

        // ═══════════════════════════════════════════════════════════════
        // TeamHubService  –  manages the SignalR connection to /hubs/team
        // Register as a scoped service in Program.cs alongside AdminHubService
        // ═══════════════════════════════════════════════════════════════
        public class TeamHubService : IAsyncDisposable
        {
            private readonly NavigationManager _nav;
           
          private readonly IAccountManagement _accountManagement;


           private HubConnection? _hub;
           private bool _started;

        // ── Typed events ──────────────────────────────────────────
        public event Func<TeamAssignedEvent, Task>? OnTeamAssignedToDisaster;

        // ── Connection state ──────────────────────────────────────
        public HubConnectionState ConnectionState =>
                   _hub?.State ?? HubConnectionState.Disconnected;

  

        public TeamHubService(NavigationManager nav, IAccountManagement accountManagement)
        {
            _nav = nav;
            _accountManagement = accountManagement;
        }

        // ── Start ─────────────────────────────────────────────────
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

            Console.WriteLine($"Token loaded: {(string.IsNullOrEmpty(token) ? "❌ NULL" : "✅ OK")}");


            _hub = new HubConnectionBuilder()
                .WithUrl($"{baseUrl.TrimEnd('/')}/hubs/team", opts =>
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
            // Map the server → client method declared in ITeamClient


            _hub.On<Guid, string, string, string, string>(
                    "TeamAssignedToDisaster",
                    async (teamId, refernceId, title, city, message) =>
                    {
                        Console.WriteLine($"[TeamHubService] TeamAssignedToDisaster — {title} in {city}");
                        if (OnTeamAssignedToDisaster is not null)
                            await OnTeamAssignedToDisaster(new TeamAssignedEvent(teamId, refernceId, title, city, message));
                    });

                try
                {
                await _hub.StartAsync();
                Console.WriteLine("[TeamHubService] ✅ Connected to /hubs/team");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[TeamHubService] ❌ Connection failed: {ex.Message}");
                }
            }

            // ── Stop ──────────────────────────────────────────────────
            public async ValueTask DisposeAsync()
            {
            if (_hub is not null)
                await _hub.DisposeAsync();
        }
        }
    }

