using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkateFactory4.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("SkateboardCN")!;
connectionString = connectionString.Replace("{APP_DATA_PATH}", builder.Environment.ContentRootPath + "App_Data");

builder.Services.AddDbContext<SkateFactoryContext>(options => { 
    options.UseSqlServer(connectionString);
});


builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SkateFactoryContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
