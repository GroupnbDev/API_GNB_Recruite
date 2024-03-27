
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using nbRecruitment.Models;
using DinkToPdf.Contracts;
using DinkToPdf;
using nbRecruitment.ModelsERP;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
//builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(2000));
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serverVersion = new MySqlServerVersion(new Version(8, 0, 19));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString2 = builder.Configuration.GetConnectionString("DefaultConnection2");
builder.Services.AddDbContext<NbRecruitmentContext>(opt => opt.UseMySql(connectionString, serverVersion));
builder.Services.AddDbContext<GnbContext>(opt => opt.UseMySql(connectionString2, serverVersion));
//builder.Services.AddSingleton(connectionFleetDeclartion);


builder.Services.AddCors(
       o =>
        o.AddPolicy(
            "Policy",
            builder =>
            {

                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
         )
    );

var app = builder.Build();
app.UseCors("Policy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseSwagger();
app.UseSwaggerUI();*/

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();