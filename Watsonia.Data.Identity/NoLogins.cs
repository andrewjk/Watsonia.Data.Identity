using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.Data.Identity
{
	/// <summary>
	/// Pass this into AddWatsoniaDataStores to indicate that external logins are not used.
	/// </summary>
	/// <seealso cref="Watsonia.Data.Identity.IUserClaim{long}" />
	public class NoLogins : IUserLogin<long>
	{
		/// <summary>
		/// Gets the primary key value for this login.
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public long Id => throw new NotImplementedException();

		/// <summary>
		/// Gets or sets the user associated with this login..
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
		/// Gets or sets the login provider for the login (e.g. facebook, google).
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string LoginProvider
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
		/// Gets or sets the unique provider identifier for this login.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string ProviderKey
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
		/// Gets or sets the friendly name used in a UI for this login.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string ProviderDisplayName
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
