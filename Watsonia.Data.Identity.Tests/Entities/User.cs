using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Watsonia.Data.Identity.Tests.Entities
{
	public class User : IUser<long>
	{
		public virtual long Id { get; set; }

		public virtual string UserName { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual string Email { get; set; }
		public virtual bool EmailConfirmed { get; set; }
		public virtual string NormalizedEmail { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual bool PhoneNumberConfirmed { get; set; }
		public virtual int AccessFailedCount { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual DateTimeOffset? LockoutEnd { get; set; }
		public virtual bool TwoFactorEnabled { get; set; }
		public virtual string ConcurrencyStamp { get; set; }
		public virtual string SecurityStamp { get; set; }

		public virtual ICollection<UserRole> Roles { get; } = new List<UserRole>();

		IList<IUserRole<long>> IUser<long>.Roles
		{
			get
			{
				// TODO:
				return new List<IUserRole<long>>();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		IList<Claim> IUser<long>.Claims { get; set; }
		IList<IUserLogin<long>> IUser<long>.Logins { get; set; }
	}
}
