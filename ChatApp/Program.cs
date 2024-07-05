using ChatApp.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseCors("reactApp");
app.MapHub<ChatHub>("/Chat");

app.Run();

// Sample SignalR hub
public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task JoinSpecificChatRoom(string username, string chatroom)
    {
        await Clients.Group(chatroom).SendAsync("JoinSpecificChatRoom", username, $"User {username} has joined the chatroom {chatroom}");
        await Groups.AddToGroupAsync(Context.ConnectionId, chatroom);
    }
}
