﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Repositories.Contract
{
	public interface IRepository<T> where T : class
	{
		T Add(T entity);
		IEnumerable<T> GetAll();
	}
}
