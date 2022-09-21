using AgentCoordination.CLI.Data;
using RabbitMQ.Client;
using Support.Chat.Portal.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgentCoordination.CLI
{
    public class AgentQueInitiatorService
    {
        private ConnectionFactory _factory;
        private readonly IConnection _connection;
        private IModel _channel;

        public AgentQueInitiatorService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void InitializeAgentQues()
        {
            const int MaxConcurrentCount = 10;
            // get team
            // get team members
            using (var db = new CoordinatorDbContext())
            {
                var team = db.Teams.Include(o => o.Agents).ThenInclude(o => o.Seniority).FirstOrDefault(o => o.Shift == Shift.OfficeTime);
                var seniorities = db.Seniorities.ToList();

                if (team == null)
                {
                    //return NotFound("Not found team");
                }

                var availableAgents = team?.Agents.ToList();

                var juniorAgents = availableAgents?.Select(x => x.SeniorityId == 1);
                if (juniorAgents?.Count() > 0)
                {
                    var juniorLevelData = seniorities.FirstOrDefault(x => x.Name == "Junior");

                    int juniorAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(juniorLevelData?.Efficiency * MaxConcurrentCount))) * juniorAgents.Count();

                    InitializeQue(juniorLevelData?.Name, juniorAgentCapacity);
                }

                var midLevelAgents = availableAgents?.Select(x => x.SeniorityId == 2);
                if (midLevelAgents?.Count() > 0)
                {
                    var midLevelData = seniorities.FirstOrDefault(x => x.Name == "MidLevel");
                    int midLevelAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(midLevelData?.Efficiency * MaxConcurrentCount))) * midLevelAgents.Count();

                    InitializeQue(midLevelData?.Name, midLevelAgentCapacity);
                }

                var seniorAgents = availableAgents?.Select(x => x.SeniorityId == 3);
                if (seniorAgents?.Count() > 0)
                {
                    var seniorLevelData = seniorities.FirstOrDefault(x => x.Name == "Senior");
                    int seniorLevelAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(seniorLevelData?.Efficiency * MaxConcurrentCount))) * seniorAgents.Count();

                    InitializeQue(seniorLevelData?.Name, seniorLevelAgentCapacity);
                }

                // update database with data

                //// update max count in shared database (redis)
                //var capacity = Math.Floor(concurrentChatCount * MaxQueueTheshold);
                //await _distributedCache.SetStringAsync(QueueMaxCountCacheKey, capacity.ToString());
                //var a = await _distributedCache.GetStringAsync(QueueMaxCountCacheKey);
            }
        }

        public void InitializeQue(string queName, int queLength)
        {
            _channel.QueueDeclare(
                queue: queName.ToUpper(),
                durable: true, exclusive: false,
                autoDelete: false,
                arguments:
                new Dictionary<string, object>()
                {
                    ["x-max-length"] = queLength,
                    ["x-overflow"] = "reject-publish"
                }
            );
        }
    }
}
