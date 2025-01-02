using Jeez.Workflow.API.Model;
using JeezFoundation.Core.Domain.Entities;
using JeezFoundation.Core.ViewModel;

namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 资源列表实体
    /// </summary>
    public class ResourceCategoryResult
    {
        /// <summary>
        /// 资源实体
        /// </summary>
        public List<ResourceTree>? ResourceTree { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        public List<SelectListItem>? SystemItems { get; set; }
    }

    /// <summary>
    /// 资源实体
    /// </summary>
    public class ResourceTree
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 资源地址
        /// </summary>
        public string? ResourceUrl { get; set; }

        /// <summary>
        /// 同级排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 样式图标ICON
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public byte IsDel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        /// 拥有按钮集合
        /// </summary>
        public List<IdAndValue>? Buttons { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<ResourceTree>? Children { get; set; }
    }

    /// <summary>
    /// 资源展示
    /// </summary>
    public class ResourceShowResult
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public SysResource? SysResource { get; set; }

        /// <summary>
        /// 拥有按钮
        /// </summary>
        public List<Button>? ButtonViewModels { get; set; }

        /// <summary>
        /// 父节点选择菜单下拉
        /// </summary>
        public List<ZTree>? ParentMenus { get; set; }
    }

    /// <summary>
    /// 编辑页DTO
    /// </summary>
    public class ResourceShowDto
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public SysResource? SysResource { get; set; }

        /// <summary>
        /// 按钮DTO
        /// </summary>
        public List<Button>? ButtonDto { get; set; }
    }

    /// <summary>
    /// 所属按钮
    /// </summary>
    public class Button
    {
        /// <summary>
        /// 资源主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 按钮类型
        /// </summary>
        public byte ButtonType { get; set; }
        /// <summary>
        /// 按钮类型 ==>用于vue
        /// </summary>
        public byte ButtonModel { get; set; }
        /// <summary>
        ///  按钮名称
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 资源删除
    /// </summary>
    public class ResourceDeleteDTO
    {
        public long[]? Ids { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// 树
    /// </summary>
    public class RoleTreeDto
    {
        public long RoleId { get; set; }

        public List<string>? ResourceIds { get; set; }

        public long CreateUserId { get; set; }
    }
}
