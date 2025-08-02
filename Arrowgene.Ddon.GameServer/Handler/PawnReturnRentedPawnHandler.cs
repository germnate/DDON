using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnReturnRentedPawnHandler : GameRequestPacketHandler<C2SPawnReturnRentedPawnReq, S2CPawnReturnRentedPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnReturnRentedPawnHandler));

        public PawnReturnRentedPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnReturnRentedPawnRes Handle(GameClient client, C2SPawnReturnRentedPawnReq request)
        {
            var pawn = client.Character.RentedPawnBySlotNo(request.SlotNo);

            var mixin = Server.ScriptManager.MixinModule.Get<IRentalPointMixin>("rental_point");
            var rentalPointReward = mixin.GetRentalPointReward(client, pawn);

            S2CItemUpdateCharacterItemNtc walletUpdateNtc = null;
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.InsertRentalPawnFeedback(client.Character.CharacterId, pawn, request.PawnFeedbackList, connection);
                Server.Database.DeleteRentalPawn(client.Character.CharacterId, pawn.PawnId, connection);
                walletUpdateNtc = Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.RentalPoints, rentalPointReward, connectionIn: connection);
            });

            if (walletUpdateNtc is not null)
            {
                client.Send(walletUpdateNtc);
            }

            client.Character.RemoveRentedPawnBySlotNo(request.SlotNo);
            return new S2CPawnReturnRentedPawnRes();
        }
    }
}
