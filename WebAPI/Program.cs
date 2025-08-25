using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;
using RepositoryLibraryProject.Interfaces;
using RepositoryLibraryProject;
using ClassLibraryProject.Entities;
using AutoMapper;
using ServiceLibraryProject;
using ServiceLibraryProject.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7124, listenOptions =>
    {
        listenOptions.UseHttps();
    });
}); 

// Add services to the container.
builder.Services.AddControllers();

// Configura el DbContext para SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorio gen�rico para DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryLibraryProject.RepositoryIMP<>));

// Registro de servicios de dominio para todas las entidades principales
builder.Services.AddScoped<IService<District>, DistrictService>();
builder.Services.AddScoped<DistrictService>();
builder.Services.AddScoped<StreetService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<ApartmentService>();
builder.Services.AddScoped<FloorService>();
builder.Services.AddScoped<AdressService>();
builder.Services.AddScoped<BuildingCompanyService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<ZipcodeService>();
builder.Services.AddScoped<StatusService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FQP Web API",
        Version = "v1",
        Description = "API Fondos QuebrantaPrecios"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registro de AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "FQP Web API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
