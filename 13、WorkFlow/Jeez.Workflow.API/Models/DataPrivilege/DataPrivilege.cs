using JeezFoundation.Core.Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeez.Workflow.API.Models.DataPrivilege
{
    [Table("sys_data_privileges")]
    public class DataPrivilege
    {
        [Key, Identity]
        public long Id { get; set; }

        public long? UserId { get; set; }

        public long? DeptId { get; set; }

        public long? SystemId { get; set; }
    }
}
