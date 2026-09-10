using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetSallyInfoHandler : GameRequestPacketHandler<C2SPawnExpeditionGetSallyInfoReq, S2CPawnExpeditionGetSallyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetSallyInfoHandler));

        public PawnExpeditionGetSallyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetSallyInfoRes Handle(GameClient client, C2SPawnExpeditionGetSallyInfoReq request)
        {
            PawnExpeditionRecord record = Server.PawnExpeditionManager.GetOrCreateRecord(client);

            S2CPawnExpeditionGetSallyInfoRes res = new S2CPawnExpeditionGetSallyInfoRes()
            {
                SallyCount = record.SallyCount,
                AreaIdList = PawnExpeditionManager.SallySpots
                    .Select(x => x.AreaId)
                    .Distinct()
                    .Select(x => new CDataCommonU32((uint)x))
                    .ToList(),
                HotSpotInfoList = new List<CDataAreaSpotSet>(PawnExpeditionManager.SallySpots),
                ActiveBuffLineupList = new List<CDataCommonU32>(),
                ClanSallySpotInfoList = Server.PawnExpeditionManager.GetClanSallySpotInfoList(client)
            };

            return res;
        }
    }
}
