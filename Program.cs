using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Repositories.Implementation;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Implementation;
using RegistrationManagementAPI.Services.Interface;
using RegistrationManagementAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add DbContext (Entity Framework Core)
builder.Services.AddDbContext<NVHTNQ10DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false, // Không kiểm tra Issuer
        ValidateAudience = false // Không kiểm tra Audience
    };
});

// Add controllers and other services
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging
builder.Logging.AddConsole();

// Add AutoMapper for program assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
