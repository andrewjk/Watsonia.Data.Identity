using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Watsonia.Data.Identity
{
	public static class IdentityBuilderExtensions
	{
		public static IdentityBuilder AddWatsoniaDataStores<TKey>(this IdentityBuilder builder, Type loginType = null, Type claimType = null)
			where TKey : IEquatable<TKey>
		{
			var keyType = typeof(TKey);
			var userType = builder.UserType;
			var roleType = builder.RoleType;

			loginType = loginType ?? typeof(NoLogins);
			claimType = claimType ?? typeof(NoClaims);

			var userStoreInterface = typeof(IUserStore<>).MakeGenericType(userType);
			var userStoreType = typeof(UserStore<,,,,>).MakeGenericType(userType, roleType, loginType, claimType, keyType);
			builder.Services.AddScoped(userStoreInterface, userStoreType);

			var roleStoreInterface = typeof(IRoleStore<>).MakeGenericType(roleType);
			var roleStoreType = typeof(RoleStore<,>).MakeGenericType(roleType, keyType);
			builder.Services.AddScoped(roleStoreInterface, roleStoreType);

			return builder;
		}
	}
}
