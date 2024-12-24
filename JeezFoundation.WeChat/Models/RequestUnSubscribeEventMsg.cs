using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 取消订阅事件消息
    /// </summary>
    public class RequestUnSubscribeEventMsg : RequestEventRootMsg
    {
        /// <summary>
        /// 取消订阅
        /// </summary>
        public override RequestEventType Event => RequestEventType.UnSubscribe;
    }
}