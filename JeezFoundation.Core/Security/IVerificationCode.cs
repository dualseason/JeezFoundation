using System.IO;

namespace JeezFoundation.Core.Security
{
    /// <summary>
    /// 验证码
    /// </summary>
    public interface IVerificationCode
    {
        MemoryStream Create(out string code, int numbers = 4);
    }
}
