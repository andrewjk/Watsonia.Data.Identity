using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.Data.Identity.Models
{
	/// <summary>
	/// Represents a claim that a user possesses.
	/// </summary>
	/// <typeparam name="TKey">The type used for the primary key for the user that possesses this claim.</typeparam>
	public interface IUserClaim<TKey>
	{
		/// <summary>
		/// Gets the primary key value for this claim.
		/// </summary>
		TKey Id { get; }

		/// <summary>
		/// Gets or sets the user associated with this claim.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		IUser<TKey> User { get; set; }

		/// <summary>
		/// Gets or sets the claim type for this claim.
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// Gets or sets the claim value for this claim.
		/// </summary>
		/// <value>
		/// The claim value.
		/// </value>
		string Value { get; set; }
	}
}
