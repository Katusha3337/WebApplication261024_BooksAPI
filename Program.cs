using Microsoft.EntityFrameworkCore;
using WebApplication261024_BooksAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = "Server=DESKTOP-M2FVLS1\\SQLEXPRESS;Database=bookdbstore;Trusted_Connection=True;TrustServerCertificate=true;";
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();