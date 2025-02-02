using HotelManagement.Application;
using HotelManagement.Infrastructure.Data;
using HotelManagement.Web;
using HotelManagement.Web.Filters;
using Microsoft.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddSerilogLogging();
builder.Services.AddControllers(options =>
{
	options.Filters.Add<ExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
	options.AddPolicy("AllowFrontendApp", policy =>
	{
		policy.WithOrigins("http://localhost:4200")
		.AllowAnyMethod()
		.AllowAnyHeader();
	})
);

builder.Services.AddSwagger();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	await app.Services.InitialiseDbAsync();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelManagement V1");
	});
	app.UseCors("AllowFrontendApp");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler(options => { });

app.Run();