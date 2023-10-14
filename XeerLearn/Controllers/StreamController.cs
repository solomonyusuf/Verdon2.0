using BigBlueButtonAPI.Common;
using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeerLearn.Controllers
{
     
    public class StreamController  
    {
        public readonly IConfiguration _configuration;
        
        public virtual string Secret { get; set; } 
        
        public virtual string Server { get; set; }
        
        public readonly BigBlueButtonAPIClient client;
       
        public readonly UserManager<Models.Auth.XeerLearnUser> _userManager;

        public virtual Models.Auth.XeerLearnUser VerdonUser { get; set; }

        public StreamController(UserManager<Models.Auth.XeerLearnUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            Secret = _configuration.GetConnectionString("stream_secret");
            Server = _configuration.GetConnectionString("stream_server");
            client = new BigBlueButtonAPIClient(new BigBlueButtonAPISettings { ServerAPIUrl = Server, SharedSecret = Secret }, new HttpClient());
        }
        

      
        // GET api/<StreamController>/5
        [HttpGet]
        public dynamic FetchMeeting(Meeting meeting, Models.Auth.XeerLearnUser user)
        {
            var meet = new GetMeetingInfoRequest() { meetingID = meeting.Id.ToString()};
            var query = client.GetMeetingInfoAsync(meet);

            return query;
        }


      
        public string TutorJoinMeeting(Meeting meeting, Models.Auth.XeerLearnUser user)
        {
           
            //Join the meeting 
            var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
            {
                meetingID = meeting.Id.ToString(),
                avatarURL = user.Image,
                fullName = user.FirstName+" "+user.LastName,
                password = meeting.ModeratorPwd
            });

             return url;
        }
        
        public string StudentJoinMeeting(Meeting meeting, Models.Auth.XeerLearnUser user)
        {
           
            //Join the meeting 
            var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
            {
                meetingID = meeting.Id.ToString(),
                avatarURL = user.Image,
                fullName = user.FirstName+" "+user.LastName,
                password = meeting.AttendeePwd
            });

             return url;
        }

        // POST api/<StreamController>
        [HttpPost]
        public async void CreateMeeting(Meeting meeting)
        {
            //Create a meeting
            await client.CreateMeetingAsync(new CreateMeetingRequest
            {
                name = meeting.Name,
                meetingID = meeting.Id.ToString(),
                logoutURL = meeting.LogoutUrl,
                attendeePW = meeting.AttendeePwd,
                moderatorPW = meeting.ModeratorPwd                   
                 
            });
        }

        // DELETE api/<StreamController>/5
        [HttpDelete("{id}")]
        public void DeleteMeeting(int id)
        {
        }
    }
}
