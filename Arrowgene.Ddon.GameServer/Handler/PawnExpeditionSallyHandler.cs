using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionSallyHandler : GameRequestPacketHandler<C2SPawnExpeditionSallyReq, S2CPawnExpeditionSallyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionSallyHandler));

        public PawnExpeditionSallyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionSallyRes Handle(GameClient client, C2SPawnExpeditionSallyReq request)
        {
            Server.PawnExpeditionManager.StartSally(client, request.AreaId, request.SpotId);
            return new S2CPawnExpeditionSallyRes();
        }
    }
}
