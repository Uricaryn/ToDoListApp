using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoListApp1.Context;
using ToDoListApp1.Models.Enums;

public class ReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EmailService _emailService;

    public ReminderService(IServiceProvider serviceProvider, EmailService emailService)
    {
        _serviceProvider = serviceProvider;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var todosWithReminders = context.ToDoItems
                    .Where(t => t.ReminderDate.HasValue && t.ReminderDate.Value <= DateTime.Now && t.IsCompleted != Status.Completed)
                    .ToList();

                foreach (var todo in todosWithReminders)
                {
                    var user = context.Users.Find(todo.UserId);
                    if (user != null)
                    {
                        var subject = "Reminder: " + todo.Title;
                        var body = $"Don't forget to complete your task: {todo.Title} - {todo.Description}";

                        await _emailService.SendEmailAsync(user.Email, subject, body);

                        // Hatırlatıcı e-posta gönderildikten sonra güncelleme yapabilirsiniz.
                        todo.ReminderDate = null; // Tekrar e-posta gönderilmesini önlemek için null yapılıyor
                    }
                }

                await context.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // 5 dakika aralıklarla çalışır
        }
    }
}
