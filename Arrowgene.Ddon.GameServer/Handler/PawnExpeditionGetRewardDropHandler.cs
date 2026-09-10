using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetRewardDropHandler : GameRequestPacketHandler<C2SPawnExpeditionGetRewardDropReq, S2CPawnExpeditionGetRewardDropRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetRewardDropHandler));

        public PawnExpeditionGetRewardDropHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetRewardDropRes Handle(GameClient client, C2SPawnExpeditionGetRewardDropReq request)
        {
            List<PawnExpeditionRewardBox> boxes = Server.Database.GetPawnExpeditionRewardBoxes(client.Character.CharacterId)
                .Where(x => !x.Claimed)
                .ToList();

            return new S2CPawnExpeditionGetRewardDropRes()
            {
                RewardDropNum = (uint)boxes.Count,
                BoxIdVec = boxes.Select(x => new CDataCommonU32(x.BoxId)).ToList(),
                MdlTypeVec = boxes.Select(x => new CDataCommonU8(x.MdlType)).ToList()
            };
        }
    }
}
