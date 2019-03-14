using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watsonia.Data;

namespace Watsonia.Data.Identity
{
	public class UserStore<TUser, TUserRole, TUserLogin, TUserClaim, TKey> :
		IUserStore<TUser>,
		IUserRoleStore<TUser>,
		IUserLoginStore<TUser>,
		IUserClaimStore<TUser>,
		IUserPasswordStore<TUser>,
		IUserSecurityStampStore<TUser>,
		IUserEmailStore<TUser>,
		IUserLockoutStore<TUser>,
		IUserTwoFactorStore<TUser>,
		IUserPhoneNumberStore<TUser>
		where TUser : class, IUser<TKey>
		where TUserRole : class, IUserRole<TKey>
		where TUserLogin : class, IUserLogin<TKey>
		where TUserClaim : class, IUserClaim<TKey>
	{
		private readonly Database _db;

		private readonly bool _areRolesEnabled;
		private readonly bool _areLoginsEnabled;
		private readonly bool _areClaimsEnabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserStore{TUser, TRole, TClaim, TLogin, TKey}"/> class.
		/// </summary>
		/// <param name="db">The database.</param>
		public UserStore(Database db)
		{
			_db = db;
			_areRolesEnabled = (typeof(TUserRole) != typeof(NoRoles));
			_areLoginsEnabled = (typeof(TUserLogin) != typeof(NoLogins));
			_areClaimsEnabled = (typeof(TUserClaim) != typeof(NoClaims));
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		#region IUserStore

		/// <summary>
		/// Creates the specified <paramref name="user" /> in the user store.
		/// </summary>
		/// <param name="user">The user to create.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the creation operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			_db.Save(user);

			return Task.FromResult(IdentityResult.Success);
		}

		/// <summary>
		/// Deletes the specified <paramref name="user" /> from the user store.
		/// </summary>
		/// <param name="user">The user to delete.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the update operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			// HACK: Was this necessary?
			//var dbuser = _db.Query<TUser>().FirstOrDefault(u => u.UserName == user.UserName);
			//if (dbuser != null)
			//{
			//	_db.Delete(dbuser);
			//}

			_db.Delete(user);

			return Task.FromResult(IdentityResult.Success);
		}

		/// <summary>
		/// Finds and returns a user, if any, who has the specified <paramref name="userId" />.
		/// </summary>
		/// <param name="userId">The user ID to search for.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId" /> if it exists.
		/// </returns>
		public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			var user = _db.Load<TUser>(userId);

			return Task.FromResult(user);
		}

		/// <summary>
		/// Finds and returns a user, if any, who has the specified normalized user name.
		/// </summary>
		/// <param name="normalizedUserName">The normalized user name to search for.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName" /> if it exists.
		/// </returns>
		public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			var user = _db.Query<TUser>().FirstOrDefault(u => u.UserName == normalizedUserName);
			if (user == null)
			{
				return Task.FromResult(default(TUser));
			}

			return Task.FromResult(user);
		}

		/// <summary>
		/// Gets the normalized user name for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose normalized name should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.UserName.ToUpperInvariant());
		}

		/// <summary>
		/// Gets the user identifier for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose identifier should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.Id.ToString());
		}

		/// <summary>
		/// Gets the user name for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose name should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the name for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.UserName);
		}

		/// <summary>
		/// Sets the given normalized name for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose name should be set.</param>
		/// <param name="normalizedName">The normalized name to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
		{
			// TODO: What am I supposed to be doing here?
			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets the given <paramref name="userName" /> for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose name should be set.</param>
		/// <param name="userName">The user name to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.UserName = userName;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Updates the specified <paramref name="user" /> in the user store.
		/// </summary>
		/// <param name="user">The user to update.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the update operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			_db.Save(user);

			return Task.FromResult(IdentityResult.Success);
		}

		#endregion IUserStore

		#region IUserRoleStore

		/// <summary>
		/// Adds to role asynchronous.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="role">The role.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task AddToRoleAsync(TUser user, string role, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			if (!user.Roles.Any(r => r.RoleName.Equals(role, StringComparison.InvariantCultureIgnoreCase)))
			{
				var newRole = (TUserRole)Activator.CreateInstance(typeof(TUserRole));
				newRole.User = user;
				newRole.RoleName = role;
				user.Roles.Add(newRole);
			}

			return Task.FromResult(0);
		}

		/// <summary>
		/// Gets a list of role names the specified <paramref name="user" /> belongs to.
		/// </summary>
		/// <param name="user">The user whose role names to retrieve.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a list of role names.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			IList<string> result;

			if (_areRolesEnabled)
			{
				result = user.Roles.Select(r => r.RoleName).ToList();
			}
			else
			{
				// Return an empty list rather than throwing an exception
				result = new List<string>();
			}

			return Task.FromResult(result);
		}

		/// <summary>
		/// Returns a list of Users who are members of the named role.
		/// </summary>
		/// <param name="roleName">The name of the role whose membership should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a list of users who are in the named role.
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether [is in role asynchronous] [the specified user].
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="role">The role.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.Roles.Any(r => r.RoleName.Equals(role, StringComparison.InvariantCultureIgnoreCase)));
		}

		/// <summary>
		/// Removes from role asynchronous.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="role">The role.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task RemoveFromRoleAsync(TUser user, string role, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			for (var i = user.Roles.Count - 1; i >= 0; i--)
			{
				if (user.Roles[i].RoleName.Equals(role, StringComparison.InvariantCultureIgnoreCase))
				{
					user.Roles.RemoveAt(i);
				}
			}

			return Task.FromResult(0);
		}

		#endregion IUserRoleStore

		#region IUserLoginStore

		/// <summary>
		/// Adds an external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user to add the login to.</param>
		/// <param name="login">The external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to add to the specified <paramref name="user" />.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			var loginExists = user.Logins.Any(l =>
				l.LoginProvider == login.LoginProvider &&
				l.ProviderKey == login.ProviderKey);

			if (!loginExists)
			{
				var newLogin = _db.Create<TUserLogin>();
				newLogin.User = user;
				newLogin.LoginProvider = login.LoginProvider;
				newLogin.ProviderKey = login.ProviderKey;
				_db.Save(newLogin);

				user.Logins.Add(newLogin);
			}

			return Task.FromResult(true);
		}

		/// <summary>
		/// Retrieves the user associated with the specified login provider and login provider key.
		/// </summary>
		/// <param name="loginProvider">The login provider who provided the <paramref name="providerKey" />.</param>
		/// <param name="providerKey">The key provided by the <paramref name="loginProvider" /> to identify a user.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
		/// </returns>
		public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			var dblogin = _db.Query<TUserLogin>().FirstOrDefault(l =>
				l.LoginProvider == loginProvider &&
				l.ProviderKey == providerKey);

			TUser user = null;
			if (dblogin != null)
			{
				user = (TUser)dblogin.User;
			}

			return Task.FromResult(user);
		}

		/// <summary>
		/// Retrieves the associated logins for the specified <param ref="user" />.
		/// </summary>
		/// <param name="user">The user whose associated logins to retrieve.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing a list of <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> for the specified <paramref name="user" />, if any.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			IList<UserLoginInfo> result;

			if (_areLoginsEnabled)
			{
				result = user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.LoginProvider)).ToList();
			}
			else
			{
				// Return an empty list rather than throwing an exception
				result = new List<UserLoginInfo>();
			}

			return Task.FromResult(result);
		}

		/// <summary>
		/// Attempts to remove the provided login information from the specified <paramref name="user" />.
		/// and returns a flag indicating whether the removal succeed or not.
		/// </summary>
		/// <param name="user">The user to remove the login information from.</param>
		/// <param name="loginProvider">The login provide whose information should be removed.</param>
		/// <param name="providerKey">The key given by the external login provider for the specified user.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			for (var i = user.Logins.Count - 1; i >= 0; i--)
			{
				if (user.Logins[i].LoginProvider == loginProvider &&
					user.Logins[i].ProviderKey == providerKey)
				{
					_db.Delete(user.Logins[i]);
					user.Logins.RemoveAt(i);
				}
			}

			return Task.FromResult(0);
		}

		#endregion IUserLoginStore

		#region IUserClaimStore

		/// <summary>
		/// Add claims to a user as an asynchronous operation.
		/// </summary>
		/// <param name="user">The user to add the claim to.</param>
		/// <param name="claims">The collection of <see cref="T:System.Security.Claims.Claim" />s to add.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}


			foreach (var claim in claims)
			{
				var claimExists = user.Claims.Any(c =>
					c.Type == claim.Type &&
					c.Value == claim.Value);

				if (!claimExists)
				{
					var newClaim = _db.Create<TUserClaim>();
					newClaim.User = user;
					newClaim.Type = claim.Type;
					newClaim.Value = claim.Value;
					_db.Save(newClaim);

					user.Claims.Add(new Claim(claim.Type, claim.Value));
				}
			}

			return Task.FromResult(0);
		}

		/// <summary>
		/// Gets a list of <see cref="T:System.Security.Claims.Claim" />s to be belonging to the specified <paramref name="user" /> as an asynchronous operation.
		/// </summary>
		/// <param name="user">The role whose claims to retrieve.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <see cref="T:System.Security.Claims.Claim" />s.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			IList<Claim> result;

			if (_areClaimsEnabled)
			{
				result = user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
			}
			else
			{
				// Return an empty list rather than throwing an exception
				result = new List<Claim>();
			}

			return Task.FromResult(result);
		}

		/// <summary>
		/// Returns a list of users who contain the specified <see cref="T:System.Security.Claims.Claim" />.
		/// </summary>
		/// <param name="claim">The claim to look for.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <typeparamref name="TUser" /> who
		/// contain the specified claim.
		/// </returns>
		public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			var query = (from c in _db.Query<TUserClaim>()
						join u in _db.Query<TUser>() on c.User equals u
						where c.Type == claim.Type &&
							c.Value == claim.Value
						select u).Distinct();

			IList<TUser> result = query.ToList();

			return Task.FromResult(result);
		}

		/// <summary>
		/// Removes the specified <paramref name="claims" /> from the given <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user to remove the specified <paramref name="claims" /> from.</param>
		/// <param name="claims">A collection of <see cref="T:System.Security.Claims.Claim" />s to remove.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			for (var i = user.Claims.Count - 1; i >= 0; i--)
			{
				if (claims.Any(c => c.Type == user.Claims[i].Type && c.Value == user.Claims[i].Value))
				{
					user.Claims.RemoveAt(i);
				}
			}

			return Task.FromResult(0);
		}

		/// <summary>
		/// Replaces the given <paramref name="claim" /> on the specified <paramref name="user" /> with the <paramref name="newClaim" />
		/// </summary>
		/// <param name="user">The user to replace the claim on.</param>
		/// <param name="claim">The claim to replace.</param>
		/// <param name="newClaim">The new claim to replace the existing <paramref name="claim" /> with.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		#endregion IUserClaimStore

		#region IUserPasswordStore

		/// <summary>
		/// Gets the password hash for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose password hash to retrieve.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.PasswordHash);
		}

		/// <summary>
		/// Gets a flag indicating whether the specified <paramref name="user" /> has a password.
		/// </summary>
		/// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a password
		/// otherwise false.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult<bool>(user.PasswordHash != null);
		}

		/// <summary>
		/// Sets the password hash for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose password hash to set.</param>
		/// <param name="passwordHash">The password hash to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.PasswordHash = passwordHash;

			return Task.FromResult(0);
		}

		#endregion IUserPasswordStore

		#region IUserSecurityStampStore

		/// <summary>
		/// Get the security stamp for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose security stamp should be set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.SecurityStamp);
		}

		/// <summary>
		/// Sets the provided security <paramref name="stamp" /> for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose security stamp should be set.</param>
		/// <param name="stamp">The security stamp to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.SecurityStamp = stamp;

			return Task.FromResult(0);
		}

		#endregion IUserSecurityStampStore

		#region IUserEmailStore

		/// <summary>
		/// Finds the by email asynchronous.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">email</exception>
		public Task<TUser> FindByEmailAsync(string email, CancellationToken cancellationToken)
		{
			if (email == null)
			{
				throw new ArgumentNullException("email");
			}

			var user = _db.Query<TUser>().Where(u => u.Email == email).FirstOrDefault();

			return Task.FromResult(user);
		}

		/// <summary>
		/// Gets the email address for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose email should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user" />.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.Email);
		}

		/// <summary>
		/// Gets a flag indicating whether the email address for the specified <paramref name="user" /> has been verified, true if the email address is verified otherwise
		/// false.
		/// </summary>
		/// <param name="user">The user whose email confirmation status should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user" />
		/// has been confirmed or not.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.EmailConfirmed);
		}

		/// <summary>
		/// Returns the normalized email for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose email address to retrieve.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object containing the results of the asynchronous lookup operation, the normalized email address if any associated with the specified user.
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets the <paramref name="email" /> address for a <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose email should be set.</param>
		/// <param name="email">The email to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.Email = email;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets the flag indicating whether the specified <paramref name="user" />'s email address has been confirmed or not.
		/// </summary>
		/// <param name="user">The user whose email confirmation status should be set.</param>
		/// <param name="confirmed">A flag indicating if the email address has been confirmed, true if the address is confirmed otherwise false.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.EmailConfirmed = confirmed;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets the normalized email for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose email address to set.</param>
		/// <param name="normalizedEmail">The normalized email to set for the specified <paramref name="user" />.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The task object representing the asynchronous operation.
		/// </returns>
		public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			// TODO: What am I supposed to be doing here?
			return Task.FromResult(0);
		}

		#endregion IUserEmailStore

		#region IUserLockoutStore

		/// <summary>
		/// Retrieves the current failed access count for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose failed access count should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the failed access count.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.AccessFailedCount);
		}

		/// <summary>
		/// Retrieves a flag indicating whether user lockout can enabled for the specified user.
		/// </summary>
		/// <param name="user">The user whose ability to be locked out should be returned.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.LockoutEnabled);
		}

		/// <summary>
		/// Gets the last <see cref="T:System.DateTimeOffset" /> a user's last lockout expired, if any.
		/// Any time in the past should be indicates a user is not locked out.
		/// </summary>
		/// <param name="user">The user whose lockout date should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a <see cref="T:System.DateTimeOffset" /> containing the last time
		/// a user's lockout expired, if any.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.LockoutEnd);
		}

		/// <summary>
		/// Records that a failed access has occurred, incrementing the failed access count.
		/// </summary>
		/// <param name="user">The user whose cancellation count should be incremented.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the incremented failed access count.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.AccessFailedCount += 1;

			return Task.FromResult(user.AccessFailedCount);
		}

		/// <summary>
		/// Resets a user's failed access count.
		/// </summary>
		/// <param name="user">The user whose failed access count should be reset.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		/// <remarks>
		/// This is typically called after the account is successfully accessed.
		/// </remarks>
		public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.AccessFailedCount = 0;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Set the flag indicating if the specified <paramref name="user" /> can be locked out.
		/// </summary>
		/// <param name="user">The user whose ability to be locked out should be set.</param>
		/// <param name="enabled">A flag indicating if lock out can be enabled for the specified <paramref name="user" />.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.LockoutEnabled = enabled;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
		/// </summary>
		/// <param name="user">The user whose lockout date should be set.</param>
		/// <param name="lockoutEnd">The <see cref="T:System.DateTimeOffset" /> after which the <paramref name="user" />'s lockout should end.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.LockoutEnd = lockoutEnd;

			return Task.FromResult(0);
		}

		#endregion IUserLockoutStore

		#region IUserTwoFactorStore

		/// <summary>
		/// Returns a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
		/// as an asynchronous operation.
		/// </summary>
		/// <param name="user">The user whose two factor authentication enabled status should be set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a flag indicating whether the specified
		/// <paramref name="user" /> has two factor authentication enabled or not.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.TwoFactorEnabled);
		}

		/// <summary>
		/// Sets a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
		/// as an asynchronous operation.
		/// </summary>
		/// <param name="user">The user whose two factor authentication enabled status should be set.</param>
		/// <param name="enabled">A flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.TwoFactorEnabled = enabled;

			return Task.FromResult(0);
		}

		#endregion IUserTwoFactorStore

		#region IUserPhoneNumberStore

		/// <summary>
		/// Gets the telephone number, if any, for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose telephone number should be retrieved.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user's telephone number, if any.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.PhoneNumber);
		}

		/// <summary>
		/// Gets a flag indicating whether the specified <paramref name="user" />'s telephone number has been confirmed.
		/// </summary>
		/// <param name="user">The user to return a flag for, indicating whether their telephone number is confirmed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a confirmed
		/// telephone number otherwise false.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			return Task.FromResult(user.PhoneNumberConfirmed);
		}

		/// <summary>
		/// Sets the telephone number for the specified <paramref name="user" />.
		/// </summary>
		/// <param name="user">The user whose telephone number should be set.</param>
		/// <param name="phoneNumber">The telephone number to set.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.PhoneNumber = phoneNumber;

			return Task.FromResult(0);
		}

		/// <summary>
		/// Sets a flag indicating if the specified <paramref name="user" />'s phone number has been confirmed.
		/// </summary>
		/// <param name="user">The user whose telephone number confirmation status should be set.</param>
		/// <param name="confirmed">A flag indicating whether the user's telephone number has been confirmed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
		/// <returns>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
		/// </returns>
		/// <exception cref="ArgumentNullException">user</exception>
		public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

			user.PhoneNumberConfirmed = confirmed;

			return Task.FromResult(0);
		}

		#endregion IUserPhoneNumberStore
	}
}
