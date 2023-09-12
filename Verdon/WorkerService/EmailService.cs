using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Verdon.Data;
using Verdon.Models.Auth;

namespace Verdon.WorkerService
{
    public class EmailService:BackgroundService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IServiceScopeFactory _serviceProvider;

        public EmailService(ILogger<EmailService> logger, IServiceScopeFactory serviceProvider)
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

                    var JobQueue = await context.JobQueue.Where(x => x.Type == "EmailService").Where(x => x.Terminated == false).ToListAsync();

                    if (JobQueue != null)
                    {
                        foreach (var item in JobQueue)
                        {
                            //Execute task



                            //Update task

                        }

                        _logger.LogInformation("EmailService terminated at: {time}", DateTimeOffset.Now);
                    }



                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
