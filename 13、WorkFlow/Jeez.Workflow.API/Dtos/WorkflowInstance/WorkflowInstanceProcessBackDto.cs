using JeezFoundation.WorkFlow;

namespace Jeez.Workflow.API.Dtos
{
    /// <summary>
    /// 流程实例执行-退回Dto
    /// </summary>
    public class WorkflowInstanceProcessBackDto
    {
        public string? InstanceId { set; get; } // 工作流实例Id
        public string? BackContent { set; get; } // 同意内容
        public string? UserId { set; get; } // 用户Id
        public string? UserName { set; get; } // 用户名

        //
        // 摘要:
        //     驳回类型
        public NodeRejectType? NodeRejectType { get; set; }
        //
        // 摘要:
        //     当驳回类型为JadeFramework.WorkFlow.NodeRejectType.ForOneStep时候的那个节点ID
        public Guid? RejectNodeId { get; set; }

    }
}
