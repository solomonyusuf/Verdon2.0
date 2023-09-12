using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Verdon.Models.Auth;
using Verdon.Models.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verdon.Controllers
{
     
    public class StreamController  
    {
        public readonly IConfiguration _configuration;
        
        public virtual string Secret { get; set; } 
        
        public virtual string Server { get; set; }
        
        public readonly BigBlueButtonAPIClient client;
       
        public readonly UserManager<VerdonUser> _userManager;

        public virtual VerdonUser VerdonUser { get; set; }

        public StreamController(UserManager<VerdonUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            Secret = _configuration.GetConnectionString("stream_secret");
            Server = _configuration.GetConnectionString("stream_server");
            client = new BigBlueButtonAPIClient(new BigBlueButtonAPISettings { ServerAPIUrl = Server, SharedSecret = Secret }, new HttpClient());
        }
        

        // GET: api/<StreamController>
        [HttpGet]
        public IEnumerable<string> FetchAllMeeting()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StreamController>/5
        [HttpGet("{id}")]
        public string FetchMeeting(int id)
        {
            return "value";
        }


      
        public string TutorJoinMeeting(Meeting meeting, VerdonUser user)
        {
           
            //Join the meeting 
            var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
            {
                meetingID = meeting.Id.ToString(),
                fullName = user.FirstName+" "+user.LastName,
                password = meeting.ModeratorPwd
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
