using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionCancelSallyHandler : GameRequestPacketHandler<C2SPawnExpeditionCancelSallyReq, S2CPawnExpeditionCancelSallyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionCancelSallyHandler));

        public PawnExpeditionCancelSallyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionCancelSallyRes Handle(GameClient client, C2SPawnExpeditionCancelSallyReq request)
        {
            Server.PawnExpeditionManager.CancelSally(client);
            return new S2CPawnExpeditionCancelSallyRes();
        }
    }
}
