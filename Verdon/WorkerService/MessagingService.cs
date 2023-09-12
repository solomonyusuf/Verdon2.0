using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Utility;
using WhatsAppApi;

namespace Verdon.WorkerService
{
    public class MessagingService: BackgroundService
    {

        private readonly ILogger<MessagingService> _logger;
        private readonly IServiceScopeFactory _serviceProvider;

        public MessagingService(ILogger<MessagingService> logger, IServiceScopeFactory serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var manager = scope.ServiceProvider.GetRequiredService<UserManager<VerdonUser>>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var JobQueue = await context.JobQueue.Where(x => x.Type == "MessagingService").Where(x=> x.Terminated == false).ToListAsync();
                    
                    if(JobQueue != null)
                    {
                        foreach (var item in JobQueue)
                        {
                            var tasks = await context.UserAccess.Where(x => x.AccessKeyId == item.AccessKeyId).ToListAsync();

                            foreach (var task in tasks)
                            {
                                _logger.LogInformation("MessagingService running at: {time}", DateTimeOffset.Now);
                                var user = await manager.FindByIdAsync(task.VerdonUserId);
                                var phone_number = user.PhoneNumber;
                                var message = await context.Announcements.Where(x => x.Id == item.ConstrainId).FirstAsync();
                                //whatapp sender here
                            }

                        }

                        _logger.LogInformation("MessagingService terminated at: {time}", DateTimeOffset.Now);
                    }
                    


                }
               
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
