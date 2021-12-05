using SecureApi.Attributes;
using SecureApi.Database;
using SecureApi.Repositories.Contract;
using SecureApi.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Repositories.Impl
{
	public class BaseRepository<T> : IRepository<T> where T : class
	{
		private readonly AuthenticationContext context;
		private readonly IEncryptor encryptor;

		public BaseRepository(AuthenticationContext authenticationContext, IEncryptor encryptor)
		{
			this.context = authenticationContext;
			this.encryptor = encryptor;
		}

		public IEnumerable<T> GetAll()
		{
			var entities = context.Set<T>().ToList();
			return DecryptMany(entities);
		}

		public T Add(T entity)
		{
			var encrypted = Encrypt(entity);

			context.Set<T>().Add(encrypted);

			context.SaveChanges();
			return encrypted;
		}

		protected T Encrypt(T entity)
		{
			var properties = entity.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(EncryptAttribute), false).Any() && x.PropertyType == typeof(string));

			foreach (var item in properties)
			{
				item.SetValue(entity, encryptor.Encrypt((string)item.GetValue(entity)));
			}

			return entity;
		}

		protected T Decrypt(T entity)
		{
			var properties = entity.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(EncryptAttribute), false).Any() && x.PropertyType == typeof(string));

			foreach (var item in properties)
			{
				item.SetValue(entity, encryptor.Decrypt((string)item.GetValue(entity)));
			}

			return entity;
		}

		protected IEnumerable<T> DecryptMany(IEnumerable<T> entities)
		{
			foreach (var item in entities)
			{
				yield return Decrypt(item);
			}
		}
	}
}
