using AgentCoordination.CLI.Data;
using Support.Chat.Portal.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Support.Chat.Portal.Common.Enums;

namespace AgentCoordination.CLI
{
    internal class AgentService
    {
        internal AgentCapacityPerShift GetAgentCapacityPerShift(Shift shift)
        {
            AgentCapacityPerShift agentCapacityPerShift = new();
            const double MaxConcurrentCount = 15; //number of parallel chats into 1.5 (multiply factor).
            using (var db = new CoordinatorDbContext())
            {
                var team = db.Teams.Include(o => o.Agents).ThenInclude(o => o.Seniority).FirstOrDefault(o => o.Shift == shift);
                var seniorities = db.Seniorities.ToList();

                if (team == null)
                {
                    return agentCapacityPerShift;
                }
                var availableAgents = team?.Agents.ToList();

                var juniorAgents = availableAgents?.Where(x => x.SeniorityId == 1);
                if (juniorAgents?.Count() > 0)
                {
                    var juniorLevelData = seniorities.FirstOrDefault(x => x.Name == "Junior");
                    agentCapacityPerShift.JuniorCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(juniorLevelData?.Efficiency * MaxConcurrentCount))) * juniorAgents.Count();
                }

                var midLevelAgents = availableAgents?.Where(x => x.SeniorityId == 2);
                if (midLevelAgents?.Count() > 0)
                {
                    var midLevelData = seniorities.FirstOrDefault(x => x.Name == "MidLevel");
                    agentCapacityPerShift.MidLevelCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(midLevelData?.Efficiency * MaxConcurrentCount))) * midLevelAgents.Count();
                }

                var seniorAgents = availableAgents?.Where(x => x.SeniorityId == 3);
                if (seniorAgents?.Count() > 0)
                {
                    var seniorLevelData = seniorities.FirstOrDefault(x => x.Name == "Senior");
                    agentCapacityPerShift.SeniorCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(seniorLevelData?.Efficiency * MaxConcurrentCount))) * seniorAgents.Count();
                }

                var teamLeadAgents = availableAgents?.Where(x => x.SeniorityId == 4);
                if (teamLeadAgents?.Count() > 0)
                {
                    var teamLeadlData = seniorities.FirstOrDefault(x => x.Name == "TeamLead");
                    agentCapacityPerShift.TeamLeadCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(teamLeadlData?.Efficiency * MaxConcurrentCount))) * teamLeadAgents.Count();
                }

                var overFlowAgents = db.Agents?.Where(x => x.TeamId == 4);
                if (overFlowAgents?.Count() > 0)
                {
                    var overFlowLevelData = seniorities.FirstOrDefault(x => x.Name == "Junior");
                    agentCapacityPerShift.OverFlowCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(overFlowLevelData?.Efficiency * MaxConcurrentCount))) * overFlowAgents.Count();
                }

                return agentCapacityPerShift;
            }
        }
        internal int GetAgentCapacityPerShift(AgentCapacityPerShift agentCapacityPerShift)
        {
            return agentCapacityPerShift.JuniorCapacity + agentCapacityPerShift.MidLevelCapacity
            + agentCapacityPerShift.SeniorCapacity + agentCapacityPerShift.TeamLeadCapacity
            + agentCapacityPerShift.OverFlowCapacity;
        }
    }
}
