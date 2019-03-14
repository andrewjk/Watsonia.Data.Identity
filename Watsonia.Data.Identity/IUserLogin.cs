using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.Data.Identity
{
	/// <summary>
	/// Represents a login and its associated provider for a user.
	/// </summary>
	/// <typeparam name="TKey">The type of the primary key of the user associated with this login.</typeparam>
	public interface IUserLogin<TKey>
	{
		/// <summary>
		/// Gets the primary key value for this login.
		/// </summary>
		TKey Id { get; }

		/// <summary>
		/// Gets or sets the user associated with this login..
		/// </summary>
		IUser<TKey> User { get; set; }

		/// <summary>
		/// Gets or sets the login provider for the login (e.g. facebook, google).
		/// </summary>
		string LoginProvider { get; set; }

		/// <summary>
		/// Gets or sets the unique provider identifier for this login.
		/// </summary>
		string ProviderKey { get; set; }

		/// <summary>
		/// Gets or sets the friendly name used in a UI for this login.
		/// </summary>
		string ProviderDisplayName { get; set; }
	}
}
