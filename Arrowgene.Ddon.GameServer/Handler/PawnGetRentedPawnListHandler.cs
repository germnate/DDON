using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRentedPawnListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRentedPawnListHandler));


        public PawnGetRentedPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PAWN_GET_RENTED_PAWN_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            var response = new S2CPawnGetRentedPawnListRes();
            for (int i = 0; i < client.Character.RentedPawns.Count; i++)
            {
                var pawn = client.Character.RentedPawns[i];
                var cdata = pawn.CDataRentedPawnList;
                cdata.SlotNo = (uint)(i + 1);
                response.RentedPawnList.Add(cdata);
            }

            client.Send(response);
        }
    }
}
