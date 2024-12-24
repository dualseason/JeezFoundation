using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class RequestLocationMsg : RequestRootMsg
    {
        /// <summary>
        /// 地理位置
        /// </summary>
        public override RequestMsgType MsgType => RequestMsgType.Location;

        /// <summary>
        /// 地理位置维度
        /// </summary>
        public double Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string? Label { get; set; }
    }
}