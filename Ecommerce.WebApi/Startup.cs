using Ecommerce.Application.Interface;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Interface;
using Ecommerce.Infrastructure.Context;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.DTO.ProductApiDto;
using Ecommerce.WebApi.DTO.UserApiDto;
using Ecommerce.WebApi.Middlewares;
using Ecommerce.WebApi.Validators.DiscountValidator;
using Ecommerce.WebApi.Validators.OrderValidator;
using Ecommerce.WebApi.Validators.ProductValidator;
using Ecommerce.WebApi.Validators.UserValidator;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Ecommerce.WebApi
{
    public class Startup
    {
        public IWebHostEnvironment _env;
        public IConfiguration _configuration;
        private static readonly Assembly StartupAssembly = Assembly.GetExecutingAssembly();
        private static readonly Assembly EcommerceApplicationAssembly = Assembly.Load("Ecommerce.Application");
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        { 
            #region Database Configuration

            services.AddDbContextPool<ApplicationDbContext>(builder =>
            builder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #endregion

            services.AddAutoMapper(new[] { StartupAssembly, EcommerceApplicationAssembly });


            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer {token}'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme, new string[] { }
        }
    });
            });

            services.AddProblemDetails(opt => opt.IncludeExceptionDetails = (ctx, ex) => _env.IsEnvironment("Local") || _env.IsDevelopment())
                .AddProblemDetailsMappingOptions();

            #region Service Interface

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            #endregion


            #region Repository Interface

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

            #region Validators

            services.AddScoped<IValidator<AddProductApiRequestDto>, AddProductDtoValidator>();
            services.AddScoped<IValidator<UpdateProductApiRequestDto>, UpdateProductDtoValidator>();
            services.AddScoped<IValidator<AddDiscountApiRequestDto>, AddDiscountDtoValidator>();
            services.AddScoped<IValidator<UpdateDiscountApiRequestDto>, UpdateDiscountDtoValidator>();
            services.AddScoped<IValidator<AddOrderApiRequestDto>, AddOrderDtoValidator>();
            services.AddScoped<IValidator<AddUserApiRequestDto>, AddUserDtoValidator>();
            services.AddScoped<IValidator<UpdateUserApiRequestDto>, UpdateUserDtoValidator>();
            services.AddScoped<IValidator<AddDiscountsApiRequestDto>, AddDiscountsDtoValidator>();
            services.AddScoped<IValidator<UserProductDiscountApi>, UserProductDiscountApiValidator>();
            services.AddScoped<IValidator<UpdateDiscountsApiRequestDto>, UpdateDiscountsDtoValidator>();
            services.AddScoped<IValidator<DiscountsApi>, DiscountsApiValidator>();

            #endregion

            #region JWT Authentication Configuration
            var secretKey = _configuration["Authentication:Jwt:SecretKey"];
            var issuer = _configuration["Authentication:Jwt:Issuer"];
            var audience = _configuration["Authentication:Jwt:Audience"]; 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;

                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"])),
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:44360",
                    ValidateAudience = true,
                    ValidAudience = "EcommerceAPI",
                    ValidateLifetime = true,
                    RequireExpirationTime = false
                };
                jwt.Authority = _configuration["JwtSettings:Issuer"];
            });
        
            #endregion
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API V1"));

            app.UseProblemDetails();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
            ApplicationDbContext dbContext;
            dbContext = new ApplicationDbContext
                (new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                    .Options
                );
            dbContext.Database.Migrate();
        }
    }
}
