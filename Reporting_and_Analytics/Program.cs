using Microsoft.EntityFrameworkCore;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;
using Reporting_and_Analytics.Repository;
using Reporting_and_Analytics.Services;
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
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.MapControllers();
app.UseHttpsRedirection();



app.Run();

