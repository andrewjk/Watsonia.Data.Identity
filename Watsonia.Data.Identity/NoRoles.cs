using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsonia.Data.Identity.Models;

namespace Watsonia.Data.Identity
{
	/// <summary>
	/// Pass this into AddWatsoniaDataStores to indicate that roles are not used.
	/// </summary>
	/// <seealso cref="Watsonia.Data.Identity.IUserClaim{long}" />
	public class NoRoles : IUserRole<long>
	{
		/// <summary>
		/// Gets the primary key value for this role.
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public long Id => throw new NotImplementedException();

		/// <summary>
		/// Gets or sets the user that is linked to this role.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public IUser<long> User
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string RoleName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
