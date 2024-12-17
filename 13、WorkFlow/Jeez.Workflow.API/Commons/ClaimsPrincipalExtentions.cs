using JeezFoundation.Core.Domain.Entities;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Jeez.Workflow.API.Commons
{
    /// <summary>
    /// ClaimsPrincipal 扩展 转换成为SysUser
    /// </summary>
    public static class ClaimsPrincipalExtentions
    {
        public static SystemUser ToSystemUser(this ClaimsPrincipal principal)
        {
            SystemUser systemUser = new SystemUser();
            List<Claim> claims = principal.Claims.ToList();
            systemUser.UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            Enum.TryParse<UserSex>(claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender)?.Value, true, out UserSex sex);
            systemUser.Sex = sex;
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var uriClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri)?.Value;
            var otherClaim = JsonConvert.DeserializeObject(claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value ?? "");
            return systemUser;
        }
    }
}
