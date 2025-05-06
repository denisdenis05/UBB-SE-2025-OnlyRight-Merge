using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using WinUIApp.Models;
using WinUIApp.WebAPI.Requests;

namespace WinUIApp.Services.DummyServices;

public class ProxyAdminService : IAdminService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyAdminService"/> class.
    /// </summary>
    public ProxyAdminService()
    {
        this.httpClient = new HttpClient
        {
            BaseAddress = new Uri(this.GetApiUrl()),
        };
    }

    /// <summary>
    /// Checks if  the user is an admin
    /// </summary>
    /// <param name="userId"> user id. </param>
    /// <returns> true, if yes, false otherwise. </returns>
    /// <exception cref="Exception"> any issues. </exception>
    public bool IsAdmin(int userId)
    {
        try
        {
            var response = this.httpClient.GetAsync($"Admin/is-admin?userId={userId}").Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<bool>().Result;
        }
        catch (Exception exception)
        {
            throw new Exception($"Error happened while getting if user with ID {userId} is admin:", exception);
        }
    }

    public void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType,
        string userModificationRequestDetails)
    {
        var request = new SendNotificationRequest
        {
            SenderUserId = 0,
            UserModificationRequestType = "string",
            UserModificationRequestDetails = "string",
        };
        try
        {
            var response = this.httpClient.PostAsJsonAsync($"Admin/send-notification", request).Result;
            response.EnsureSuccessStatusCode();
            return;
        }
        catch (Exception exception)
        {
            throw new Exception($"Error happened while sending notification from {senderUserId} ({userModificationRequestDetails})", exception);
        }
    }
    private string GetApiUrl()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        return configuration.GetValue<string>("ApiUrl");
    }
}
