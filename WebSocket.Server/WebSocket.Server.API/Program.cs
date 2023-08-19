using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseWebSockets();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

        while (true)
        {
            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes($"Hora atual -> {DateTime.Now}"),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);

            await Task.Delay(1000);
        }
    }
});

app.Run();