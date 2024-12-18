using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    public class RequestTextMsg: RequestRootMsg
    {
        /// <summary>
        /// 文本
        /// </summary>
        public override RequestMsgType MsgType => RequestMsgType.Text;

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string? Content { get; set; }
    }
}
