namespace Jeez.Workflow.API.Models
{
    public class ModelBase
    {
        /// <summary>
        /// 是否删除 1:是，0：否
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 更新人Id
        /// </summary>
        public long UpdateUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }

    public class UpdateModel
    {
        /// <summary>
        /// 更新人Id
        /// </summary>
        public long UpdateUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }
    }

    public class CreateModel
    {
        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long CreateTime { get; set; }
    }

    public class LogicDelete
    {
        /// <summary>
        /// 是否删除 1:是，0：否
        /// </summary>
        public bool IsDel { get; set; }
    }

    public class ModelMemo
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }
    }
}
