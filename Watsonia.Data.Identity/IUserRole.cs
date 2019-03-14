using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.Data.Identity
{
	/// <summary>
	/// Represents the link between a user and a role.
	/// </summary>
	/// <typeparam name="TKey">The type of the primary key used for users and roles.</typeparam>
	public interface IUserRole<TKey>
	{
		/// <summary>
		/// Gets the primary key value for this role.
		/// </summary>
		TKey Id { get; }

		/// <summary>
		/// Gets or sets the user that is linked to this role.
		/// </summary>
		IUser<TKey> User { get; set; }

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		string RoleName { get; set; }
	}
}
