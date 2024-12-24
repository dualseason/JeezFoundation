using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 订阅事件消息
    /// </summary>
    public class RequestSubscribeEventMsg : RequestEventRootMsg
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        public override RequestEventType Event => RequestEventType.Subscribe;
    }
}