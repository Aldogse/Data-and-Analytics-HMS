using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;
using Reporting_and_Analytics.Repository;
using System.Text.Json.Serialization;
using Reporting_and_Analytics.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




//using Reporting_and_Analytics.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IParticularRepository, ParticularRepository>();
builder.Services.AddScoped<IIncomeStatementRepository, IncomeStatementRepository>();
builder.Services.AddScoped<IPatientRecordsRepository, PatientRecordRepository>();
builder.Services.AddScoped<IDailyPatientReportRepository, DailyPatientReportRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAppUserCredentials, AppUserCredentialsRepository>();
//builder.Services.AddHostedService<IncomeStatementMonthlyReportGenerator>();
//builder.Services.AddHostedService<DailyPatientReportService>();
//builder.Services.AddHostedService<ParticularTableDataCleanUpService>();
////builder.Services.AddHostedService<MonthlyPatientReportServices>();
//builder.Services.AddHostedService<DailyIncomeReportService>();
//builder.Services.AddHostedService<MonthlyHospitalIncomeReportService>();


builder.Services.AddDbContext<DatabaseContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.Password.RequiredLength = 5;
})
	.AddEntityFrameworkStores<DatabaseContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateActor = true,
		ValidateAudience = true,
		ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
		ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value)),
		ValidateLifetime = true,
		
	};
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();



app.Run();

