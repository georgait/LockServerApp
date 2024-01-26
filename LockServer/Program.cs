using LockServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("RunnersDB");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/runners", async (AppDbContext dbContext, Runner request) =>
{
    dbContext.Runners.Add(request);
    await dbContext.SaveChangesAsync();
    //if (!added)
    //{
    //    return Results.Conflict("Runner exists");
    //}    
    return Results.Created($"/api/runners/{request.Id}", request);
});

app.MapDelete("api/runners/{id}", async (AppDbContext dbContext, string id) =>
{
    var runner = dbContext.Runners.Find(id);
    if (runner is not null)
    {
        dbContext.Runners.Remove(runner);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapGet("api/runners", async (AppDbContext dbContext) => Results.Ok(await dbContext.Runners.ToListAsync()));
app.MapPost("api/runners/purge", async (AppDbContext dbContext) =>
{
    dbContext.Runners.RemoveRange(dbContext.Runners);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
app.MapGet("api/runners/count", async (AppDbContext dbContext) => Results.Ok(await dbContext.Runners.CountAsync()));

app.Run();

