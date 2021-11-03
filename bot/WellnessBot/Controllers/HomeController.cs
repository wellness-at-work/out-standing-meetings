using Microsoft.AspNetCore.Mvc;
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
using Microsoft.Extensions.Logging;

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
            var result = String.Empty;
            try
            {
                var groupCreated = await CreateGroup(meetingContext);
                if (groupCreated)
                {
                    var groupJoined = await JoinGroup(meetingContext);
                    if (groupJoined)
                    {
                        await UpdateStatus(meetingContext);
                    }
                    result = $"Status Updated to {meetingContext.ActivityType}";
                }
            }
            catch (Exception ex)
            {
               
                result = "unable to update the status";
            }
            return Ok(result);
        }

        private async Task<bool> UpdateStatus(OutstandingMeeting meetingContext)
        {
            var status = new UpdateStatus { ParticipantId = meetingContext.UserId, GroupId = meetingContext.Group, StatusId = (int)meetingContext.ActivityType };
            var statusContent = JsonConvert.SerializeObject(status);
            using (var httpClient = new HttpClient())
            {
                var dataParams = new Dictionary<string, string>();
                dataParams.Add("Payload", statusContent);
                dataParams.Add("Content-Type", "application/x-www-form-urlencoded");
                var url = $"{_configuration["WellnessTrackerUrl"]}/UpdateStatus?code={_configuration["wellnesstrackerupdatecode"]}";
                var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(dataParams));
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> JoinGroup(OutstandingMeeting meetingContext)
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
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CreateGroup(OutstandingMeeting meetingContext)
        {
            var groupId = new Group { Id = meetingContext.Group };
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
    }
}
