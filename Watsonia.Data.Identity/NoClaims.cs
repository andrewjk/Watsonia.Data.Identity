using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsonia.Data.Identity.Models;

namespace Watsonia.Data.Identity
{
	/// <summary>
	/// Pass this into AddWatsoniaDataStores to indicate that claims are not used.
	/// </summary>
	/// <seealso cref="Watsonia.Data.Identity.IUserClaim{long}" />
	public class NoClaims : IUserClaim<long>
	{
		/// <summary>
		/// Gets the primary key value for this claim.
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public long Id => throw new NotImplementedException();

		/// <summary>
		/// Gets or sets the user associated with this claim.
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
		/// Gets or sets the claim type for this claim.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string Type
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
		/// Gets or sets the claim value for this claim.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		public string Value
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
