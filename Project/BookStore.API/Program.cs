using BookStore.API.Configurations;
using BookStore.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(option =>
{
    option
    .UseSqlServer(builder
    .Configuration.
    GetConnectionString("defaultConnection"));
});

builder.Services.AddAutoMapper(typeof(MapperConfiguration));

builder.Services.AddControllers();

builder.Host.UseSerilog(
    (ctx, lc) => lc
    .WriteTo
    .Console()
    .ReadFrom
    .Configuration(ctx.Configuration)
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
