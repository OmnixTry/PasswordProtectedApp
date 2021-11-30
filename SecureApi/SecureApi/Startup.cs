using AutoMapper;
using AutoMapper.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecureApi.Database;
using SecureApi.Identity.Jwt;
using SecureApi.Identity.PasswordHashers;
using SecureApi.Models;
using SecureApi.Models.Mapping;
using SecureApi.Models.WebEntities;
using SecureApi.Models.WebEntities.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureApi
{
	public class Startup
	{
		private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; //TODO: Move to secure source
		private readonly SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddDbContext<AuthenticationContext>(options => 
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
					builder => builder.MigrationsAssembly("SecureApi")));

			services.AddTransient<IValidator<RegistrationWe>, RegistrationWeValidator>();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SecureApi", Version = "v1" });
			});

			services.AddIdentity<User, IdentityRole>
				(o =>
				{
					// configure identity options
					o.Password.RequireDigit = false;
					o.Password.RequireLowercase = false;
					o.Password.RequireUppercase = false;
					o.Password.RequireNonAlphanumeric = false;
					o.Password.RequiredLength = 6;
				})
				.AddEntityFrameworkStores<AuthenticationContext>()
				.AddDefaultTokenProviders();
			services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
			services.AddSingleton<IJwtFactory, JwtFactory>();

			services.AddAutoMapper(c => c.AddProfile<WeToEntityProfile>());

			var jwtAppSettingConfigurationOptions = Configuration.GetSection(nameof(JwtIssuerConfig));

			// Configure JwtIssuerOptions
			services.Configure<JwtIssuerConfig>(options =>
			{
				options.Issuer = jwtAppSettingConfigurationOptions[nameof(JwtIssuerConfig.Issuer)];
				options.Audience = jwtAppSettingConfigurationOptions[nameof(JwtIssuerConfig.Audience)];
				options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.Aes256CbcHmacSha512);
			});

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options =>
					{
						options.RequireHttpsMetadata = false;
						options.TokenValidationParameters = new TokenValidationParameters
						{							
							ValidateIssuer = true,							
							ValidIssuer = jwtAppSettingConfigurationOptions[nameof(JwtIssuerConfig.Issuer)],							
							ValidateAudience = true,							
							ValidAudience = jwtAppSettingConfigurationOptions[nameof(JwtIssuerConfig.Audience)],							
							ValidateLifetime = true,							
							IssuerSigningKey = signingKey,							
							ValidateIssuerSigningKey = true,
						};
					});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecureApi v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerConfig));
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerConfig.Issuer)],

				ValidateAudience = true,
				ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerConfig.Audience)],

				ValidateIssuerSigningKey = true,
				IssuerSigningKey = signingKey,

				RequireExpirationTime = false,
				ValidateLifetime = false,
				ClockSkew = TimeSpan.Zero
			};

			
		}

		class diProfile : Profile
		{
			public diProfile()
			{
				CreateMap<RegistrationWe, User>();
			}
		}

	}
}
