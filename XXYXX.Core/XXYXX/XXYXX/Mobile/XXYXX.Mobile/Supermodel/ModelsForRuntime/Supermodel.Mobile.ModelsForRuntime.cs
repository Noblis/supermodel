//   DO NOT MAKE CHANGES TO THIS FILE. THEY MIGHT GET OVERWRITTEN!!!
//   Auto-generated by Supermodel.Mobile on 10/8/2020 4:16:03 PM
//

// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;
using Supermodel.Mobile.Runtime.Common.Models;
using System.ComponentModel;
using Supermodel.DataAnnotations.Attributes;
// ReSharper restore RedundantUsingDirective

// ReSharper disable once CheckNamespace
namespace Supermodel.ApiClient.Models
{
	#region XXYXXUserUpdatePasswordApiController
	[RestUrl("XXYXXUserUpdatePassword")]
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class XXYXXUserUpdatePassword : Model
	{
		#region Properties
		public String OldPassword { get; set; }
		public String NewPassword { get; set; }
		#endregion
	}
	#endregion
	
	#region Types models depend on and types that were specifically marked with [IncludeInApiClient]
	#endregion
}
