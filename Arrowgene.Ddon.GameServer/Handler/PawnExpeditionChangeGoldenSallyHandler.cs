using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionChangeGoldenSallyHandler : GameRequestPacketHandler<C2SPawnExpeditionChangeGoldenSallyReq, S2CPawnExpeditionChangeGoldenSallyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionChangeGoldenSallyHandler));

        public PawnExpeditionChangeGoldenSallyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionChangeGoldenSallyRes Handle(GameClient client, C2SPawnExpeditionChangeGoldenSallyReq request)
        {
            Server.PawnExpeditionManager.ChangeGoldenSally(client, request.Price);
            return new S2CPawnExpeditionChangeGoldenSallyRes();
        }
    }
}
