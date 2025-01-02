using JeezFoundation.Core.Domain.Entities;

namespace Jeez.Workflow.API.Dtos
{
    public class SystemIndexSearch : BaseSearch
    {
    }

    public class SystemDeleteDto
    {
        public List<long>? Ids { get; set; }

        public long UserId { get; set; }
    }
}
