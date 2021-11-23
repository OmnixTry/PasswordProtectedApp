using AutoMapper;
using AutoMapper.Execution;
using FluentValidation;
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
using Microsoft.OpenApi.Models;
using SecureApi.Database;
using SecureApi.Models;
using SecureApi.Models.WebEntities;
using SecureApi.Models.WebEntities.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi
{
	public class Startup
	{
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

			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<RegistrationWe, User>();
			});

			services.AddAutoMapper(x => x.CreateMap<RegistrationWe, User>());

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
