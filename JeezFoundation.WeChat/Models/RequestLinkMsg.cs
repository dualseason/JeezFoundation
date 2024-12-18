using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 链接信息
    /// </summary>
    public class RequestLinkMsg : RequestRootMsg
    {
        /// <summary>
        /// 链接
        /// </summary>
        public override RequestMsgType MsgType => RequestMsgType.Link;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string? Url { get; set; }
    }
}
