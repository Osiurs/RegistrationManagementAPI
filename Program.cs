using RegistrationManagementAPI.Services;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Repositories;  // Make sure this is included if the repo is in a separate namespace
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DbContext (assuming you're using Entity Framework)
builder.Services.AddDbContext<NVHTNQ10DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services with dependency injection
builder.Services.AddScoped<IStudentRepository, StudentRepository>(); // Register repository
builder.Services.AddScoped<IStudentService, StudentService>();      // Register service
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Add controllers and other services
builder.Services.AddControllers();

// Add Swagger if needed (for API documentation)
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
