//   DO NOT MAKE CHANGES TO THIS FILE. THEY MIGHT GET OVERWRITTEN!!!
//   Auto-generated by Supermodel.Mobile on 7/22/2020 1:28:06 PM
//

// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;
using Supermodel.Mobile.Runtime.Common.Models;
using System.ComponentModel;

// ReSharper restore RedundantUsingDirective

// ReSharper disable once CheckNamespace
namespace Supermodel.ApiClient.Models
{
	#region ToDoListApiController
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class ToDoList : Model
	{
		#region Properties
		public DateTime CreatedOnUtc { get; set; } = new DateTime();
		public DateTime ModifiedOnUtc { get; set; } = new DateTime();
		public long ListOwnerId { get; set; }
		public string Name { get; set; }
		public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
		#endregion
	}
	
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class SimpleSearch
	{
		#region Properties
		public string SearchTerm { get; set; }
		#endregion
	}
	#endregion
	
	#region TDMUserUpdatePasswordApiController
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class TDMUserUpdatePassword : Model
	{
		#region Properties
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
		#endregion
	}
	#endregion
	
	#region DeleteAllBeforeApiController
	//Extension method for DeleteAllBeforeApi command
	public static class DeleteAllBeforeApiCommandExt
	{
		public static async Task<DeleteAllBeforeOutput> DeleteAllBeforeApiAsync(this WebApiDataContext me, DeleteAllBeforeInput input)
		{
			return await me.ExecutePostAsync<DeleteAllBeforeInput, DeleteAllBeforeOutput>("DeleteAllBeforeApi", input);
		}
	}
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class DeleteAllBeforeInput
	{
		#region Properties
		public DateTime OlderThanUtc { get; set; } = new DateTime();
		#endregion
	}
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class DeleteAllBeforeOutput
	{
		#region Properties
		public long DeletedCount { get; set; }
		#endregion
	}
	#endregion
	
	#region Types models depend on and types that were specifically marked with [IncludeInApiClient]
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class ToDoItem
	{
		#region Properties
		public string Name { get; set; }
		public PriorityEnum? Priority { get; set; }
		public DateTime? DueOn { get; set; }
		public bool Completed { get; set; }
		public long Id { get; set; }
		#endregion
	}
	public enum PriorityEnum
	{
		High = 0,
		Medium = 1,
		Low = 2
	}
	#endregion
}
