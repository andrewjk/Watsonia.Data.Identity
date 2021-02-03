using System;
using System.Collections.Generic;
using System.Text;
using Watsonia.Data.Identity.Models;

namespace Watsonia.Data.Identity.Tests.Entities
{
	public class UserRole : IUserRole<long>
	{
		public virtual long Id { get; set; }

		public virtual User User { get; set; }
		public virtual string RoleName { get; set; }

		IUser<long> IUserRole<long>.User
		{
			get
			{
				return this.User;
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
