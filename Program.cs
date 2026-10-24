using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

string Data = "00000000";

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapPost("/data", ([FromBody] RequestPayload payload) =>
{
    if (payload == null || string.IsNullOrWhiteSpace(payload.Data))
    {
        return Results.BadRequest(new { error = "Daten dürfen nicht leer sein." });
    }

    Data = payload.Data;

    return Results.Ok(new { message = "Variable überschrieben!", current = Data });
});

app.MapGet("/data", () =>
{
    return Results.Ok(new { value = Data });
});

app.Run();

public record RequestPayload(string Data);
