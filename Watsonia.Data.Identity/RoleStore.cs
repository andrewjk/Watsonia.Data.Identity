using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watsonia.Data;
using Watsonia.Data.Identity.Models;

namespace Watsonia.Data.Identity
{
	public sealed class RoleStore<TRole, TKey> :
		IRoleStore<TRole>
		where TRole : class, IUserRole<TKey>
	{
		private readonly Database _db;

		/// <summary>
		/// Initializes a new instance of the <see cref="RoleStore{TRole, TKey}"/> class.
		/// </summary>
		/// <param name="db">The database.</param>
		public RoleStore(Database db)
		{
			_db = db;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Creates a new role in a store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role to create in the store.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
		/// </returns>
		/// <exception cref="ArgumentNullException">role</exception>
		public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			await _db.SaveAsync(role);

			return IdentityResult.Success;
		}

		/// <summary>
		/// Updates a role in a store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role to update in the store.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
		/// </returns>
		/// <exception cref="ArgumentNullException">role</exception>
		public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			await _db.SaveAsync(role);

			return IdentityResult.Success;
		}

		/// <summary>
		/// Deletes a role from the store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role to delete from the store.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.
		/// </returns>
		/// <exception cref="ArgumentNullException">role</exception>
		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			await _db.DeleteAsync(role);

			return IdentityResult.Success;
		}

		/// <summary>
		/// Finds the role who has the specified ID as an asynchronous operation.
		/// </summary>
		/// <param name="roleId">The role ID to look for.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.
		/// </returns>
		public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			// Convert the roleId from string to the correct key type, so the cache gets checked correctly
			var id = (TKey)Convert.ChangeType(roleId, typeof(TKey));
			return _db.LoadAsync<TRole>(id);
		}

		/// <summary>
		/// Finds the role who has the specified normalized name as an asynchronous operation.
		/// </summary>
		/// <param name="normalizedRoleName">The normalized role name to look for.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.
		/// </returns>
		public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			var role = _db.Query<TRole>().FirstOrDefault(r => r.RoleName == normalizedRoleName);

			return Task.FromResult(role);
		}

		/// <summary>
		/// Gets the ID for a role from the store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role whose ID should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the ID of the role.
		/// </returns>
		public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.Id.ToString());
		}

		/// <summary>
		/// Gets the name of a role from the store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role whose name should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.
		/// </returns>
		public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.RoleName);
		}

		/// <summary>
		/// Get a role's normalized name as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role whose normalized name should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.
		/// </returns>
		public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role.RoleName);
		}

		/// <summary>
		/// Sets the name of a role in the store as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role whose name should be set.</param>
		/// <param name="roleName">The name of the role.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
		{
			role.RoleName = roleName;
			await _db.SaveAsync(role);
		}

		/// <summary>
		/// Set a role's normalized name as an asynchronous operation.
		/// </summary>
		/// <param name="role">The role whose normalized name should be set.</param>
		/// <param name="normalizedName">The normalized name to set</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
		{
			role.RoleName = normalizedName;
			await _db.SaveAsync(role);
		}
	}
}
