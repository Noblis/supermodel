#nullable enable

using System.Collections.Generic;
using System.Security.Claims;

namespace Supermodel.Presentation.Mvc.Auth
{
    public static class AuthClaimsHelper
    {
        public static List<Claim> CreateNewClaimsListWithIdAndLabel(long id, string label)
        {
            return new List<Claim> 
            { 
                new Claim(ClaimTypes.NameIdentifier, id.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Name, label, ClaimValueTypes.String) 
            };
        }
    }
}