﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using AdaptiveCards;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SidePanel.Models;
using SidePanel.Model;

namespace SidePanel.Controllers
{
    public class HomeController : Controller
    {
        public static string conversationId;
        public static string serviceUrl;
        //public static List<TaskInfo> taskInfoData = new List<TaskInfo>();

        private readonly IConfiguration _configuration;
        private readonly AppCredentials botCredentials;
        private readonly HttpClient httpClient;

        public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory, AppCredentials botCredentials)
        {
            _configuration = configuration;
            this.botCredentials = botCredentials;
            this.httpClient = httpClientFactory.CreateClient();
        }

        //Configure call from Manifest
        [Route("/Home/Configure")]
        public ActionResult Configure()
        {
            return View("Configure");
        }

        //SidePanel Call from Configure
        [Route("/Home/SidePanel")]
        public ActionResult SidePanel()
        {
            return PartialView("SidePanel");
        }

        [HttpPost]
        [Route("/Home/wellnessstyle")]
        public async Task<IActionResult> AddWellnessStyle (OutstandingMeeting meetingContext)
        {
            var groupCreated = await CreateGroup(meetingContext);
            if (groupCreated)
            {
                var joinGroup = new JoinGroup { Id = meetingContext.UserId, Name = meetingContext.User, GroupId = meetingContext.Group, Duration = 15.0F, Activity = null };
                var joinGroupContent = JsonConvert.SerializeObject(joinGroup);
                using (var httpClient = new HttpClient())
                {
                    var dataParams = new Dictionary<string, string>();
                    dataParams.Add("Payload", joinGroupContent);
                    dataParams.Add("ProcessingType", "2");
                    dataParams.Add("Content-Type", "application/x-www-form-urlencoded");
                    var url = $"{_configuration["WellnessTrackerUrl"]}/process?code={_configuration["wellnesstrackercode"]}";
                    var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(dataParams));
                    if (response.IsSuccessStatusCode)
                    {
                        var test = "hello";
                    }
                }
            }
            string result = "Your status has been updated";

            return Ok(result);
        }



        private async Task<bool> CreateGroup(OutstandingMeeting meetingContext)
        {
            var groupId = new Group { Id = meetingContext.Group };
            //StringContent groupContent = new StringContent(JsonConvert.SerializeObject(groupId));
            var groupContent = JsonConvert.SerializeObject(groupId);
            using (var httpClient = new HttpClient())
            {
                var dataParams = new Dictionary<string, string>();
                dataParams.Add("Payload", groupContent);
                dataParams.Add("ProcessingType", "1");
                dataParams.Add("Content-Type", "application/x-www-form-urlencoded");
                var url = $"{_configuration["WellnessTrackerUrl"]}/process?code={_configuration["wellnesstrackercode"]}";
                var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(dataParams));
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        

        ////Create Adaptive Card with the Agenda List
        //private Attachment AgendaAdaptiveList()
        //{
        //    AdaptiveCard adaptiveCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
        //    adaptiveCard.Body = new List<AdaptiveElement>()
        //    {
        //        new AdaptiveTextBlock(){Text="Here is the Agenda for Today", Weight=AdaptiveTextWeight.Bolder}
        //    };

        //    foreach (var agendaPoint in taskInfoData)
        //    {
        //        var textBlock = new AdaptiveTextBlock() { Text = "- " + agendaPoint.Title + " \r" };
        //        adaptiveCard.Body.Add(textBlock);
        //    }

        //    return new Attachment()
        //    {
        //        ContentType = AdaptiveCard.ContentType,
        //        Content = adaptiveCard
        //    };
        //}

        ////Check if the Participant Role is Organizer
        //[Route("/Home/IsOrganizer")]
        //public async Task<ActionResult<bool>> IsOrganizer(string userId, string meetingId, string tenantId)
        //{
        //    var response = await GetMeetingRoleAsync(meetingId, userId, tenantId);
        //    if (response.meeting.role == "Organizer")
        //        return true;
        //    else
        //        return false;
        //}

        //public async Task<UserMeetingRoleServiceResponse> GetMeetingRoleAsync(string meetingId, string userId, string tenantId)
        //{
        //    if (serviceUrl == null)
        //    {
        //        throw new InvalidOperationException("Service URL is not avaiable for tenant ID " + tenantId);
        //    }

        //    using var getRoleRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(serviceUrl), string.Format("v1/meetings/{0}/participants/{1}?tenantId={2}", meetingId, userId, tenantId)));
        //    getRoleRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await this.botCredentials.GetTokenAsync());

        //    using var getRoleResponse = await this.httpClient.SendAsync(getRoleRequest);
        //    getRoleResponse.EnsureSuccessStatusCode();

        //    var response = JsonConvert.DeserializeObject<UserMeetingRoleServiceResponse>(await getRoleResponse.Content.ReadAsStringAsync());
        //    return response;
        //}
    }
}
