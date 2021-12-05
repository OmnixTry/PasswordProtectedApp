using Microsoft.AspNetCore.Mvc;
using SecureApi.Models.Entities;
using SecureApi.Repositories.Contract;
using SecureApi.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecureApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MysteriesController : ControllerBase
	{
		private readonly IRepository<GreatMystery> mysteryRepo;

		public MysteriesController(IRepository<GreatMystery> mysteryRepo)
		{
			this.mysteryRepo = mysteryRepo;
		}

		// GET: api/<MysteriesController>
		[HttpGet]
		public IEnumerable<GreatMystery> Get()
		{
			return mysteryRepo.GetAll();
		}

		// GET api/<MysteriesController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<MysteriesController>
		[HttpPost]
		public void Post([FromBody] GreatMystery value)
		{
			mysteryRepo.Add(value);
		}

		// PUT api/<MysteriesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<MysteriesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
