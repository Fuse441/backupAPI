using colab_api.Models;
using colab_api.Repositories;
using colab_api.Services;
using colab_api.Services.MailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SpaceService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<SampleDBContext>(options =>
{
    var DBConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    options.UseMySql(DBConnectionString,ServerVersion.AutoDetect(DBConnectionString));

   
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
