﻿using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Reflection;
using TraversalApp.Core.Entites;
using TraversalApp.Core.Repositories;
using TraversalApp.Core.Services;
using TraversalApp.Core.UnitOfWorks;
using TraversalApp.Repository.Context;
using TraversalApp.Repository.Repositories;
using TraversalApp.Repository.UnitOfWorks;
using TraversalApp.Service.Mapping;
using TraversalApp.Service.Services;
using TraversalApp.Core.DTOs;
using TraversalApp.Service.Validations;
using TraversalApp.Service.Container;
using TraversalApp.MVC.CQRS.Handlers.DestinationHandlers;
using MediatR;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Repository Layer Dependency Injection
// Service Layer Dependency Injection

builder.Services.AddHttpClient();
builder.Services.ContainerDependencies();


builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.CustomValidator();

builder.Services.AddControllersWithViews().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

builder.Services.AddScoped<GetAllDestinationQueryHandler>();
builder.Services.AddScoped<GetDestinationByIDQueryHandler>();
builder.Services.AddScoped<CreateDestinationCommandHandler>();
builder.Services.AddScoped<RemoveDestinationCommandHandler>();
builder.Services.AddScoped<UpdateDestinationCommandHandler>();

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LibraryEntrypoint).Assembly));

//builder.Services.AddMediatR(typeof(Program));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddLogging(x=>
{
    x.ClearProviders();
    x.SetMinimumLevel(LogLevel.Debug);      //Debugdan itibaren loglamaya başla.
    x.AddDebug();                           // Output sekmesinde sun.
});

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDbContext"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});


builder.Services.AddIdentity<AppUser, AppRole>().AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider).AddEntityFrameworkStores<AppDbContext>().AddErrorDescriber<CustomIdentityValidator>().AddEntityFrameworkStores<AppDbContext>();



// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation();
builder.Services.AddMvc(config =>
    {
        var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
        config.Filters.Add(new AuthorizeFilter(policy));
       
    });

builder.Services.AddMvc();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Login/SignIn";
});

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var path = Directory.GetCurrentDirectory();
loggerFactory.AddFile($"{path}//Logs//Log1.txt");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Destination}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.Run();