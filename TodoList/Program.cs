using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Repositories;

var myAllowedOrigin = "todo origin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowedOrigin,
        policy =>
        {
            policy.WithOrigins("https://kimfom01.github.io/TodoListUI");
        });
});
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

app.UseCors(myAllowedOrigin);
app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();