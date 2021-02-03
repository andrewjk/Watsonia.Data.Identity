using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.Data.Identity.Models
{
	/// <summary>
	/// Represents a user in the identity system.
	/// </summary>
	/// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
	public interface IUser<TKey>
	{
		/// <summary>
		/// Gets the primary key value for this user.
		/// </summary>
		TKey Id { get; }

		/// <summary>
		/// Gets or sets the user name for this user.
		/// </summary>
		string UserName { get; set; }

		/// <summary>
		/// Gets or sets a salted and hashed representation of the password for this user.
		/// </summary>
		string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets the email address for this user.
		/// </summary>
		string Email { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if a user has confirmed their email address.
		/// </summary>
		bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the normalized email address for this user.
		/// </summary>
		string NormalizedEmail { get; set; }

		/// <summary>
		/// Gets or sets a telephone number for the user.
		/// </summary>
		string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if a user has confirmed their telephone address.
		/// </summary>
		bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the number of failed login attempts for the current user.
		/// </summary>
		/// <value>
		/// The access failed count.
		/// </value>
		int AccessFailedCount { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if the user could be locked out.
		/// </summary>
		bool LockoutEnabled { get; set; }

		/// <summary>
		/// Gets or sets the date and time, in UTC, when any user lockout ends.
		/// </summary>
		/// <remarks>
		/// A value in the past means the user is not locked out.
		/// </remarks>
		DateTimeOffset? LockoutEnd { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating if two factor authentication is enabled for this
		/// </summary>
		bool TwoFactorEnabled { get; set; }

		/// <summary>
		/// A random value that must change whenever a user is persisted to the store
		/// </summary>
		string ConcurrencyStamp { get; set; }

		/// <summary>
		/// A random value that must change whenever a users credentials change (password changed, login removed)
		/// </summary>
		string SecurityStamp { get; set; }

		/// <summary>
		/// Gets or sets the user's roles.
		/// </summary>
		IList<IUserRole<TKey>> Roles { get; set; }

		/// <summary>
		/// Gets or sets the user's claims.
		/// </summary>
		IList<IUserClaim<TKey>> Claims { get; set; }

		/// <summary>
		/// Gets or sets the user's external logins.
		/// </summary>
		IList<IUserLogin<TKey>> Logins { get; set; }
	}
}
