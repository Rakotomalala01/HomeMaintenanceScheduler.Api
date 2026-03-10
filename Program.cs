using HomeMaintenanceScheduler.Api.Background;
using HomeMaintenanceScheduler.Api.Data;
using HomeMaintenanceScheduler.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

builder.Services.AddSingleton<RecurrenceService>();
builder.Services.AddScoped<DiscordNotifier>();
builder.Services.AddScoped<ReminderScanService>();

builder.Services.AddHostedService<ReminderWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();