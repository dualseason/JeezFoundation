using JeezFoundation.WeChat.Enums;

namespace JeezFoundation.WeChat.Models
{
    /// <summary>
    /// 短视频消息
    /// </summary>
    public class RequestShortVideoMsg : RequestRootMsg
    {
        /// <summary>
        /// 短视频
        /// </summary>
        public override RequestMsgType MsgType => RequestMsgType.ShortVideo;

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string? MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string? ThumbMediaId { get; set; }
    }
}
