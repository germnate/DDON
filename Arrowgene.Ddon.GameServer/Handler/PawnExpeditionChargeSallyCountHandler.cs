using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionChargeSallyCountHandler : GameRequestPacketHandler<C2SPawnExpeditionChargeSallyCountReq, S2CPawnExpeditionChargeSallyCountRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionChargeSallyCountHandler));

        public PawnExpeditionChargeSallyCountHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionChargeSallyCountRes Handle(GameClient client, C2SPawnExpeditionChargeSallyCountReq request)
        {
            Server.PawnExpeditionManager.ChargeSallyCount(client, request.Price);
            return new S2CPawnExpeditionChargeSallyCountRes();
        }
    }
}
