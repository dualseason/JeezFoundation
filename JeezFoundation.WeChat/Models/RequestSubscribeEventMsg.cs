using JeezFoundation.WeChat.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
