using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 事件请求基类
    /// </summary>
    public class RequestEventRootMsg : RequestRootMsg
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual RequestEventType Event { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public override RequestMsgType MsgType => RequestMsgType.Event;
    }
}