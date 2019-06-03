using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Watsonia.Data.SQLite;
using Watsonia.Data.Identity.Tests.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Watsonia.Data.Identity.Tests
{
	[TestClass]
	public class CreateUser
	{
		[TestMethod]
		public async Task TestCreateUser()
		{
			var provider = new SQLiteDataAccessProvider();
			var connectionString = @"Data Source=Data\Entities.sqlite";
			var db = new EntitiesDb(provider, connectionString, "Watsonia.Data.Identity.Tests.Entities");

			await db.EnsureDatabaseDeletedAsync();
			await db.EnsureDatabaseCreatedAsync();

			var store = new UserStore<User, UserRole, NoLogins, NoClaims, long>(db);

			var user = db.Create<User>();
			user.Email = "test@example.com";
			user.UserName = "test";

			var result = await store.CreateAsync(user, new CancellationToken());

			Assert.AreEqual(true, result.Succeeded);
			Assert.IsTrue(db.Query<User>().Any(u => u.Email == "test@example.com"));
		}
	}
}
