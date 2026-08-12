using BlazorApp2.Identity;
using BlazorApp2.Models;
using BlazorApp2.Requests;
using BlazorApp2.Responses;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace BlazorApp2.Services
{
    public class ServiceApi(IHttpClientFactory httpClientFactory)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Eghatha");

        // ═══════════════════════════════════════════════════════
        // IDENTITY
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<LoginResponse>> LoginAsync(LoginRqeuest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/identity/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
                return ApiResult<LoginResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<LoginResponse>(response);
        }

        public async Task<ApiResult<string>> RequestPasswordResetAsync(RequestResetModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/identity/request-password-reset", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/identity/reset-password", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<UserInfo>> GetCurrentUserAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/identity/me");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<UserInfo>(JsonOptions);
                return ApiResult<UserInfo>.Success(data!);
            }
            return await HandleErrorResponseAsync<UserInfo>(response);
        }

        public async Task<ApiResult<string>> ConfirmEmailAsync(ConfirmEmailRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/identity/confirm-email", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ResendConfirmEmailCodeAsync(ResendEmailCodeRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/identity/resend-email-code", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        // ═══════════════════════════════════════════════════════
        // ACCOUNTS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<PagedResponse<AccountResponse>>> GetAccountsAsync(
            string? searchTerm = null, string? role = null, bool? isActive = null,
            int page = 1, int pageSize = 10)
        {
            var url = BuildUrl("api/v1/accounts",
                ("SearchTerm", searchTerm), ("Role", role),
                ("IsActive", isActive?.ToString()), ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<AccountResponse>>(JsonOptions);
                return ApiResult<PagedResponse<AccountResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<AccountResponse>>(response);
        }

        public async Task<ApiResult<string>> ActivateAccountAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"api/v1/accounts/{id}/activate", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DeactivateAccountAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"api/v1/accounts/{id}/deactivate", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<AccountResponse>> GetMyAccount()
        {
            var response = await _httpClient.GetAsync("api/v1/accounts/my-account");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AccountResponse>(JsonOptions);
                return ApiResult<AccountResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<AccountResponse>(response);
        }

        // ═══════════════════════════════════════════════════════
        // DASHBOARDS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<AccountStatisticsResponse>> GetAccountStatisticsAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/dashboards/account-statistics");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AccountStatisticsResponse>(JsonOptions);
                return ApiResult<AccountStatisticsResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<AccountStatisticsResponse>(response);
        }

        public async Task<ApiResult<DisasterStatisticsResponse>> GetDisasterStatisticsAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/dashboards/disaster-statistics");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<DisasterStatisticsResponse>(JsonOptions);
                return ApiResult<DisasterStatisticsResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<DisasterStatisticsResponse>(response);
        }


        // ═══════════════════════════════════════════════════════
        // AiAssistnat
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<AiAssistantResponse>> GetAiAssistantResponseAsync(AiAssistantRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/ai-assistant/ask", model);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AiAssistantResponse>(JsonOptions);
                return ApiResult<AiAssistantResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<AiAssistantResponse>(response);
        }
        // ═══════════════════════════════════════════════════════
        // DISASTERS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<string>> DispatchResourceAsync(
    Guid disasterId,
    DispatchResourceToDisasterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/v1/disasters/{disasterId}/resources", request);

            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");

            return await HandleErrorResponseAsync<string>(response);
        }
        public async Task<ApiResult<CreateDisasterResponse>> CreateDisasterAsync(CreateDisasterRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/disasters", model);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<CreateDisasterResponse>(JsonOptions);
                return ApiResult<CreateDisasterResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<CreateDisasterResponse>(response);
        }

        public async Task<ApiResult<PagedResponse<DisasterResponse>>> GetDisastersAsync(
            string? city = null, string? province = null, string? type = null, string? status = null,
            DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 10)
        {
            var url = BuildUrl("api/v1/disasters",
                ("City", city), ("Province", province), ("Type", type), ("Status", status),
                ("From", from?.ToString("o")), ("To", to?.ToString("o")),
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<DisasterResponse>>(JsonOptions);
                return ApiResult<PagedResponse<DisasterResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<DisasterResponse>>(response);
        }

        public async Task<ApiResult<PagedResponse<DisasterTimelineEvent>>> GetDisasterTimelineAsync(Guid disasterId )
        {
            var response = await _httpClient.GetAsync($"api/v1/disasters/{disasterId}/timeline");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<DisasterTimelineEvent>>(JsonOptions);
                return ApiResult<PagedResponse<DisasterTimelineEvent>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<DisasterTimelineEvent>>(response);

        }

        public async Task<ApiResult<DisasterDetailResponse>> GetDisasterByIdAsync(Guid disasterId)
        {
            var response = await _httpClient.GetAsync($"api/v1/disasters/{disasterId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<DisasterDetailResponse>(JsonOptions);
                return ApiResult<DisasterDetailResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<DisasterDetailResponse>(response);
        }

        public async Task<ApiResult<string>> ResolveDisasterAsync(Guid disasterId)
        {
            var response = await _httpClient.PostAsync($"api/v1/disasters/{disasterId}/resolve", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> CloseDisasterAsync(Guid disasterId)
        {
            var response = await _httpClient.PostAsync($"api/v1/disasters/{disasterId}/close", null);
            if (response.IsSuccessStatusCode)  
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ArchiveDisasterAsync(Guid disasterId)
        {
            var response = await _httpClient.PostAsync($"api/v1/disasters/{disasterId}/archive", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DispatchTeamsAsync(Guid disasterId, DispathTeamsRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/assign-teams" , request);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DispatchVolunteersAsync(Guid disasterId, DispathVolunteersRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/assign-volunteers", request);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> AddAffectedPersonsAsync(Guid disasterId, AddAffectedPersonsRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/affected-persons", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> UpdateAffectedPersonAsync(Guid disasterId, Guid personId, UpdateAffectedPersonRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/disasters/{disasterId}/affected-persons/{personId}", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ConsumeDisasterResourceAsync(Guid disasterId, Guid resourceId, ConsumeDisasterResourceRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/resources/{resourceId}/consume", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ReturnDisasterResourceAsync(Guid disasterId, Guid resourceId, ReturnDisasterResourceRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/resources/{resourceId}/return", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> MarkDisasterResourceDamagedAsync(Guid disasterId, Guid resourceId, MarkDisasterResourceDamagedRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/resources/{resourceId}/damage", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<List<RecommendedTeamsResponse>>> GetRecommendedTeams(Guid disasterId )
        {
            var response = await _httpClient.GetAsync($"api/v1/disasters/{disasterId}/recommended-teams");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<RecommendedTeamsResponse>>(JsonOptions);
                return ApiResult<List<RecommendedTeamsResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<List<RecommendedTeamsResponse>>(response);
        }

        public async Task<ApiResult<List<RecommendedVolunteerResponse>>> GetRecommendedVolunteers(Guid disasterId)
        {
            var response = await _httpClient.GetAsync($"api/v1/disasters/{disasterId}/recommended-volunteers");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<RecommendedVolunteerResponse>>(JsonOptions);
                return ApiResult<List<RecommendedVolunteerResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<List<RecommendedVolunteerResponse>>(response);
        }

        public async Task<ApiResult<PagedResponse<DisasterVolunteerResponse>>> GetDisasterVolunteers(Guid disasterId, int page, int pageSize)
        {
            var url = BuildUrl($"api/v1/disasters/{disasterId}/volunteers",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<DisasterVolunteerResponse>>(JsonOptions);
                return ApiResult<PagedResponse<DisasterVolunteerResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<DisasterVolunteerResponse>>(response);
        }
       
        
        public async Task<ApiResult<GenerateDisasterReportDto>> GenerateReportAsync(Guid disasterid)
        {
            var response = await _httpClient.PostAsync($"api/v1/disasters/{disasterid}/report",null);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<GenerateDisasterReportDto>(JsonOptions);
                return ApiResult<GenerateDisasterReportDto>.Success(data!);
            }
                
            return await HandleErrorResponseAsync<GenerateDisasterReportDto>(response);
        }
             
        // ═══════════════════════════════════════════════════════
        // TEAMS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<TeamLocation>> GetCurrentTeamLocation(Guid teamid )
        {
            var response = await _httpClient.GetAsync($"api/v1/teams/{teamid}/location");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TeamLocation>(JsonOptions);
                return ApiResult<TeamLocation>.Success(data!);
            }
            return await HandleErrorResponseAsync<TeamLocation>(response);

        }
        
         
        public async Task<ApiResult<TeamDisasterResponse>> GetCurrentTeamDisaster(Guid teamid )
        {
            var response = await _httpClient.GetAsync($"api/v1/teams/{teamid}/current-disaster");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TeamDisasterResponse>(JsonOptions);
                return ApiResult<TeamDisasterResponse>.Success(data!);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // 404 means no current disaster assigned to the team
                return ApiResult<TeamDisasterResponse>.Success(null!);
            }
            return await HandleErrorResponseAsync<TeamDisasterResponse>(response);
        }
        
        public async Task<ApiResult<TeamResponse>> CreateTeamAsync(CreateTeamRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/teams", model);
            if (response.IsSuccessStatusCode)
            {
                // API returns 201 with null body — don't deserialize
                return ApiResult<TeamResponse>.Success(null!);
            }
            return await HandleErrorResponseAsync<TeamResponse>(response);
        }

        public async Task<ApiResult<PagedResponse<TeamResponse>>> GetTeamsAsync(
            string? search = null, string? status = null, string? speciality = null,
            string? province = null, int page = 1, int pageSize = 10)
        {
            var url = BuildUrl("api/v1/teams",
                ("SearchTerm", search), ("Status", status), ("Speciality", speciality),
                ("Province", province), ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<TeamResponse>>(JsonOptions);
                return ApiResult<PagedResponse<TeamResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<TeamResponse>>(response);
        }

        public async Task<ApiResult<TeamDetailResponse>> GetTeamByIdAsync(Guid teamId)
        {
            var response = await _httpClient.GetAsync($"api/v1/teams/{teamId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TeamDetailResponse>(JsonOptions);
                return ApiResult<TeamDetailResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<TeamDetailResponse>(response);
        }

        public async Task<ApiResult<string>> UpdateTeamAsync(Guid teamId, UpdateTeamRequest model)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/v1/teams/{teamId}", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> UpdateTeamLocationAsync(Guid teamId, UpdateTeamLocation model)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/v1/teams/{teamId}/live-location", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> AddTeamMemberMultipartAsync(
            Guid teamId,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string jobTitle,
            bool isLeader,
            IBrowserFile photo)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(firstName), "FirstName");
            form.Add(new StringContent(lastName), "LastName");
            form.Add(new StringContent(email), "Email");
            form.Add(new StringContent(phoneNumber), "PhoneNumber");
            form.Add(new StringContent(jobTitle), "JobTitle");
            form.Add(new StringContent(isLeader.ToString()), "IsLeader");

            var streamContent = new StreamContent(photo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            streamContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);

            form.Add(streamContent, "Photo", photo.Name);

            var response = await _httpClient.PostAsync(
                $"api/v1/teams/{teamId}/members",
                form);

            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");

            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<PagedResponse<TeamMemberResponse>>> GetTeamMembersAsync(Guid teamId, int page = 1, int pageSize = 10)
        {
            var url = BuildUrl($"api/v1/teams/{teamId}/members",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<TeamMemberResponse>>(JsonOptions);
                return ApiResult<PagedResponse<TeamMemberResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<TeamMemberResponse>>(response);
        }

        public async Task<ApiResult<PagedResponse<TeamDisasterResponse>>> GetTeamDisasters (Guid disasterId ,int page , int pageSize)
        {
            var url = BuildUrl($"api/v1/teams/{disasterId}/disasters",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<TeamDisasterResponse>>(JsonOptions);
                return ApiResult<PagedResponse<TeamDisasterResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<TeamDisasterResponse>>(response);
        }

        public async Task<ApiResult<string>> RemoveTeamMemberAsync(Guid teamId, Guid memberId)
        {
            var response = await _httpClient.DeleteAsync($"api/v1/teams/{teamId}/members/{memberId}");
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DeActivateTeamMemberAsync(Guid teamId, Guid memberId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/members/{memberId}/deactivate" , null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ActivateTeamMemberAsync(Guid teamId, Guid memberId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/members/{memberId}/activate", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ActivateTeamAsync(Guid teamId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/activate", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DeActivateTeamAsync(Guid teamId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/deactivate", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> ReturningTeamAsync(Guid teamId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/returning", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> SetTeamLeaderAsync(Guid teamId, Guid memberId)
        {
            var response = await _httpClient.PostAsync($"api/v1/teams/{teamId}/leader/{memberId}", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> AddTeamResourceAsync(Guid teamId, AddTeamResourceRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/teams/{teamId}/resources", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<PagedResponse<TeamResourceResponse>>> GetTeamResourcesAsync(Guid teamId, int page = 1, int pageSize = 10, string? type = null)
        {
            var url = BuildUrl($"api/v1/teams/{teamId}/resources",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()), ("Type", type));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<TeamResourceResponse>>(JsonOptions);
                return ApiResult<PagedResponse<TeamResourceResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<TeamResourceResponse>>(response);
        }

        public async Task<ApiResult<string>> IncreaseResourceQuantityAsync(Guid teamId, Guid resourceId, int quantity)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/teams/{teamId}/resources/{resourceId}/increase", quantity);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DecreaseResourceQuantityAsync(Guid teamId, Guid resourceId, int quantity)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/teams/{teamId}/resources/{resourceId}/decrease", quantity);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<TeamMemberInfoResponse>> GetCurrentTeamMemberInfoAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/teams/me");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TeamMemberInfoResponse>(JsonOptions);
                return ApiResult<TeamMemberInfoResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<TeamMemberInfoResponse>(response);
        }

        public async Task<ApiResult<List<TeamMapDto>>> GetMapTeamsAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/teams/map");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<TeamMapDto>>(JsonOptions);
                return ApiResult<List<TeamMapDto>>.Success(data!);
            }
            return await HandleErrorResponseAsync<List<TeamMapDto>>(response);
        }




        // ═══════════════════════════════════════════════════════
        // VOLUNTEER REGISTRATIONS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<PagedResponse<VolunteerRegisterationResponse>>> GetRegistrationsAsync(
            string? searchTerm = null, string? status = null, int page = 1, int pageSize = 10)
        {
            var url = BuildUrl("api/v1/volunteer-registerations",
                ("SearchTerm", searchTerm), ("Status", status),
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<VolunteerRegisterationResponse>>(JsonOptions);
                return ApiResult<PagedResponse<VolunteerRegisterationResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<VolunteerRegisterationResponse>>(response);
        }

        public async Task<ApiResult<string>> ApproveRegistrationAsync(Guid registrationId)
        {
            var response = await _httpClient.PostAsync($"api/v1/volunteer-registerations/{registrationId}/approve", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> RejectRegistrationAsync(Guid registrationId, RejectVolunteerRegiserationRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/volunteer-registerations/{registrationId}/reject", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

       
        public async Task<ApiResult<VolunteerRegisterationResponse>> GetRegisterationByIdAsync(Guid registerationid)
        {
            var response = await _httpClient.GetAsync($"api/v1/volunteer-registerations/{registerationid}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<VolunteerRegisterationResponse>(JsonOptions);
                return ApiResult<VolunteerRegisterationResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<VolunteerRegisterationResponse>(response);
        }

        // ═══════════════════════════════════════════════════════
        // VOLUNTEERS
        // ═══════════════════════════════════════════════════════


        public async Task<ApiResult<CreateVolunteerResponse>> CreateVolunteerAsync(
         string firstName,
         string lastName,
         string email,
         string phoneNumber,
         string password,
         string speciality,
         double latitude,
         double longitude,
         int yearsOfExperience,
       byte[] photoBytes,
string photoName,
string photoContentType,

byte[] cvBytes,
string cvName,
string cvContentType)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(firstName), "FirstName");
            form.Add(new StringContent(lastName), "LastName");
            form.Add(new StringContent(email), "Email");
            form.Add(new StringContent(phoneNumber), "PhoneNumber");
            form.Add(new StringContent(password), "Password");
            form.Add(new StringContent(speciality), "Speciality");
            form.Add(new StringContent(latitude.ToString()), "Latitude");
            form.Add(new StringContent(longitude.ToString()), "Longitude");
            form.Add(new StringContent(yearsOfExperience.ToString()), "YearsOfExperience");

            // ── Photo ──────────────────────────────────────────────────────────
            var photoContent = new ByteArrayContent(photoBytes);

            photoContent.Headers.ContentType =
                new MediaTypeHeaderValue(photoContentType);

            form.Add(photoContent, "Photo", photoName);

            // ── CV ─────────────────────────────────────────────────────────────
            var cvContent = new ByteArrayContent(cvBytes);

            cvContent.Headers.ContentType =
                new MediaTypeHeaderValue(cvContentType);

            form.Add(cvContent, "Cv", cvName);

            var response = await _httpClient.PostAsync("api/v1/volunteers", form);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<CreateVolunteerResponse>(JsonOptions);
                return ApiResult<CreateVolunteerResponse>.Success(data!);
            }

            return await HandleErrorResponseAsync<CreateVolunteerResponse>(response);
        }
    
        public async Task<ApiResult<PagedResponse<VolunteerResponse>>> GetVolunteersAsync(
            string? search = null, string? status = null, string? speciality = null,
            string? province = null, int page = 1, int pageSize = 10)
        {
            var url = BuildUrl("api/v1/volunteers",
                ("SearchTerm", search), ("Status", status), ("Speciality", speciality),
                ("Province", province), ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<VolunteerResponse>>(JsonOptions);
                return ApiResult<PagedResponse<VolunteerResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<VolunteerResponse>>(response);
        }

        public async Task<ApiResult<VolunteerResponse>> GetVolunteerById(Guid volunteerId)
        {
            var response = await _httpClient.GetAsync($"api/v1/volunteers/{volunteerId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<VolunteerResponse>(JsonOptions);
                return ApiResult<VolunteerResponse>.Success(data!);
            }
            return await HandleErrorResponseAsync<VolunteerResponse>(response);
        }
        public async Task<ApiResult<PagedResponse<VolunteerRankingResponse>>> GetTopVolunteersAsync(
            int page = 1, int pageSize = 10, string? province = null, string? city = null,
            string? speciality = null, double? minAverageScore = null, string? sortBy = null, bool descending = true)
        {
            var url = BuildUrl("api/v1/volunteers/top",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()),
                ("Province", province), ("City", city), ("Speciality", speciality),
                ("MinAverageScore", minAverageScore?.ToString()), ("SortBy", sortBy),
                ("Descending", descending.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<VolunteerRankingResponse>>(JsonOptions);
                return ApiResult<PagedResponse<VolunteerRankingResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<VolunteerRankingResponse>>(response);
        }

        // location
        public async Task<ApiResult<string>> UpdateVolunteerLocationAsync(Guid volunteerId, UpdateVolunteerLocationRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/volunteers/{volunteerId}/location", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }


        // status 
        public async Task<ApiResult<string>> SetVolunteerStatusToBusy(Guid volunteerId )
        {
            var response = await _httpClient.PutAsync($"api/v1/volunteers/{volunteerId}/busy", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> SetVolunteerStatusToAvailable(Guid volunteerId)
        {
            var response = await _httpClient.PutAsync($"api/v1/volunteers/{volunteerId}/available", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> SetVolunteerStatusToUnAvailable(Guid volunteerId)
        {
            var response = await _httpClient.PutAsync($"api/v1/volunteers/{volunteerId}/unavailable", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }


        //equipments 

        public async Task<ApiResult<string>> IncreaseVolunteerEquipmentQuantity(Guid volunteerId , Guid equipmentId , ChangeEquipmentQuantityRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/volunteers/{volunteerId}/equipments/{equipmentId}/increase", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> DecreaseVolunteerEquipmentQuantity(Guid volunteerId, Guid equipmentId, ChangeEquipmentQuantityRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/volunteers/{volunteerId}/equipments/{equipmentId}/decrease", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> CreateVolunteerEquipment(Guid volunteerId , CreateVolunteerEquipmentRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/volunteers/{volunteerId}/equipments", model);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> RemoveVolunteerEquipment(Guid volunteerId, Guid equipmentId)
        {
            var response = await _httpClient.DeleteAsync($"api/v1/volunteers/{volunteerId}/equipments/{equipmentId}");
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }
        public async Task<ApiResult<string>> SetEquipmentValidAsync(Guid volunteerId, Guid equipmentId)
        {
            var response = await _httpClient.PutAsync($"api/v1/volunteers/{volunteerId}/equipments/{equipmentId}/valid", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<string>> SetEquipmentInvalidAsync(Guid volunteerId, Guid equipmentId)
        {
            var response = await _httpClient.PutAsync($"api/v1/volunteers/{volunteerId}/equipments/{equipmentId}/invalid", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }
        public async Task<ApiResult<PagedResponse<VolunteerEquipmentResponse>>> GetVolunteerEquipmentsAsync(Guid volunteerId, int page = 1, int pageSize = 10, string? type = null)
        {
            var url = BuildUrl($"api/v1/volunteers/{volunteerId}/equipments",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()), ("Type", type));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<VolunteerEquipmentResponse>>(JsonOptions);
                return ApiResult<PagedResponse<VolunteerEquipmentResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<VolunteerEquipmentResponse>>(response);
        }

        //evaluation
        public async Task<ApiResult<string>> EvaluateVolunteerAsync(Guid disasterId, Guid volunteerid, EvaluateVolunteerRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/disasters/{disasterId}/volunteers/{volunteerid}/evaluate", request);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        // disaster 
        public async Task<ApiResult<VolunteerDisasterResponse>> GetCurrentVolunteerDisaster(Guid volunteerid)
        {
            var response = await _httpClient.GetAsync($"api/v1/volunteers/{volunteerid}/current-disaster");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<VolunteerDisasterResponse>(JsonOptions);
                return ApiResult<VolunteerDisasterResponse>.Success(data!);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // 404 means no current disaster assigned to the team
                return ApiResult<VolunteerDisasterResponse>.Success(null!);
            }
            return await HandleErrorResponseAsync<VolunteerDisasterResponse>(response);
        }



        // ═══════════════════════════════════════════════════════
        // NOTIFICATIONS
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<PagedResponse<NotificationResponse>>> GetNotificationsAsync(int page = 1, int pageSize = 20)
        {
            var url = BuildUrl("api/v1/notifications",
                ("Page", page.ToString()), ("PageSize", pageSize.ToString()));

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PagedResponse<NotificationResponse>>(JsonOptions);
                return ApiResult<PagedResponse<NotificationResponse>>.Success(data!);
            }
            return await HandleErrorResponseAsync<PagedResponse<NotificationResponse>>(response);
        }

        public async Task<ApiResult<string>> MarkNotificationReadAsync(Guid notificationId)
        {
            var response = await _httpClient.PostAsync($"api/v1/notifications/{notificationId}/read", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }

        public async Task<ApiResult<int>> GetUnreadCountAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/notifications/unread-count");

            if (response.IsSuccessStatusCode)
            {
                var count = await response.Content.ReadFromJsonAsync<int>();
                return ApiResult<int>.Success(count);
            }

            return await HandleErrorResponseAsync<int>(response);
        }

        public async Task<ApiResult<string>> MarkAllNotificationsReadAsync()
        {
            var response = await _httpClient.PostAsync("api/v1/notifications/read", null);
            if (response.IsSuccessStatusCode)
                return ApiResult<string>.Success("");
            return await HandleErrorResponseAsync<string>(response);
        }



        // ═══════════════════════════════════════════════════════
        // REFERENCE DATA
        // ═══════════════════════════════════════════════════════

        public async Task<ApiResult<List<ReferenceItem>>> GetDisasterTypesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/disaster-types");

        public async Task<ApiResult<List<ReferenceItem>>> GetDisasterStatusesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/disaster-statuses");

        public async Task<ApiResult<List<ReferenceItem>>> GetTeamStatusesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/team-statuses");

        public async Task<ApiResult<List<ReferenceItem>>> GetTeamSpecialitiesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/team-specialities");

        public async Task<ApiResult<List<ReferenceItem>>> GetResourceTypesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/resource-types");

        public async Task<ApiResult<List<ReferenceItem>>> GetRegistrationStatusesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/registration-statuses");

        public async Task<ApiResult<List<ReferenceItem>>> GetVolunteerStatusesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/volunteer-statuses");

        public async Task<ApiResult<List<ReferenceItem>>> GetVolunteerSpecialitiesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/volunteer-specialities");

        public async Task<ApiResult<List<ReferenceItem>>> GetEquipmentCategoriesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/equipment-categories");

        public async Task<ApiResult<List<ReferenceItem>>> GetEquipmentStatusesAsync()
            => await GetReferenceListAsync<ReferenceItem>("api/v1/reference-data/equipment-statuses");

        private async Task<ApiResult<List<T>>> GetReferenceListAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<T>>(JsonOptions);
                return ApiResult<List<T>>.Success(data!);
            }
            return await HandleErrorResponseAsync<List<T>>(response);
        }

        // ═══════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static string BuildUrl(string baseUrl, params (string Key, string? Value)[] queryParams)
        {
            var filtered = queryParams.Where(p => p.Value != null);
            var query = string.Join("&", filtered.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value!)}"));
            return string.IsNullOrEmpty(query) ? baseUrl : $"{baseUrl}?{query}";
        }

        private static async Task<ApiResult<T>> HandleErrorResponseAsync<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            try
            {
                var problem = JsonSerializer.Deserialize<ProblemDetails>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (problem is not null)
                    return ApiResult<T>.Failure(problem.Title ?? "An error occurred", problem.Detail ?? "", problem.Status ?? (int)response.StatusCode, problem.Errors);
            }
            catch (JsonException) { }

            return ApiResult<T>.Failure(GetFriendlyErrorMessage(response.StatusCode), content, (int)response.StatusCode);
        }

        private static string GetFriendlyErrorMessage(HttpStatusCode statusCode) => statusCode switch
        {
            HttpStatusCode.BadRequest => "Invalid request. Please check your input and try again.",
            HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
            HttpStatusCode.Forbidden => "You don't have permission to perform this action.",
            HttpStatusCode.NotFound => "The requested resource was not found.",
            HttpStatusCode.Conflict => "The operation conflicts with the current state of the resource.",
            HttpStatusCode.UnprocessableEntity => "The request contains invalid data.",
            HttpStatusCode.InternalServerError => "A server error occurred. Please try again later.",
            HttpStatusCode.BadGateway => "Service temporarily unavailable. Please try again later.",
            HttpStatusCode.ServiceUnavailable => "Service temporarily unavailable. Please try again later.",
            HttpStatusCode.GatewayTimeout => "The request timed out. Please try again.",
            _ => "An error occurred while processing your request."
        };
    }
}


