using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TodoList.Models;
using TodoList.Services.UserService;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;
using TodoList.Services.ToDo;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<TodoDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DB"))
        );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<BCryptPasswordHash>();
        builder.Services.AddSingleton<AccessTokenGenerator>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IToDoService, ToDoService>();
        builder.Services.AddAutoMapper(typeof(Program));
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                         .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });
        builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();

        app.Run();
    }
}
