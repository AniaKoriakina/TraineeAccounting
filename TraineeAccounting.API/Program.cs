using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Handlers;
using TraineeAccounting.Application.Validators;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Infrastructure.Data;
using TraineeAccounting.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<ITraineeshipRepository, TraineeshipRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IValidator<CreateTraineeCommand>, CreateTraineeValidator>();
builder.Services.AddScoped<IValidator<UpdateTraineeCommand>, UpdateTraineeValidator>();
builder.Services.AddScoped<IValidator<UpdateTraineeshipTraineesCommand>, UpdateTraineeshipTraineesValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectTraineesCommand>, UpdateProjectTraineesValidator>();
builder.Services.AddScoped<IValidator<CreateTraineeshipCommand>, CreateTraineeshipValidator>();
builder.Services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectValidator>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTraineeHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTraineeHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteTraineeHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteTraineeshipHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteProjectHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTraineeshipTraineesHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateProjectTraineesHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTraineeshipHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
        policy.WithOrigins("http://localhost:5229")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();