using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<TodoDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("TodoDb"));
    });
}
else
{
    builder.Services.AddDbContext<TodoDbContext>(options =>
    {
        options.UseNpgsql(ExternalDbConnectionHelper.GetConnectionString());
    });
}
builder.Services.AddScoped<IRepository, TodoRepository>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    var scope = app.Services.CreateScope();
    await MigrationHelper.MigrateDatabaseAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();