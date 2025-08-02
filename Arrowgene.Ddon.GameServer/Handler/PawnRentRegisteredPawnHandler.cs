using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnRentRegisteredPawnHandler : GameRequestPacketQueueHandler<C2SPawnRentRegisteredPawnReq, S2CPawnRentRegisteredPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnRentRegisteredPawnHandler));

        public PawnRentRegisteredPawnHandler(DdonGameServer server) : base(server)
        {
        }

        private byte AdventureCount => Server.GameSettings.GameServerSettings.RentalPawnAdventureCount;
        private byte CraftCount => Server.GameSettings.GameServerSettings.RentalPawnCraftCount;

        public override PacketQueue Handle(GameClient client, C2SPawnRentRegisteredPawnReq request)
        {
            PacketQueue packetQueue = new();
            Pawn pawn = null;

            // Make sure this pawn was not already rented (we don't allow duplicates)
            if (client.Character.RentedPawns.Where(x => x.PawnId == request.RequestedPawnId).FirstOrDefault() != null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_ALREADY_RENTED);
            }

            if (Server.WalletManager.GetWalletAmount(client.Character, WalletType.RiftPoints) < request.RentalCost)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_NO_RIM);
            }

            S2CPawnRentRegisteredPawnRes response = new();
            RentalPawnRecord rentalRecord = null;

            Server.Database.ExecuteInTransaction(connectionIn =>
            {
                uint ownerCharacterId = Server.Database.GetPawnOwnerCharacterId(request.RequestedPawnId, connectionIn);
                if (ownerCharacterId == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PAWN_PARAM_NOT_FOUND);
                }

                var ownerCharacter = Server.CharacterManager.SelectCharacter(ownerCharacterId, true, connectionIn);
                for (int i = 0; i < ownerCharacter.Pawns.Count; i++)
                {
                    if (ownerCharacter.Pawns[i].PawnId != request.RequestedPawnId)
                    {
                        continue;
                    }

                    pawn = ownerCharacter.Pawns[i];
                    break;
                }

                if (pawn == null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_REGISTERD_DATA_NOT_FOUND);
                }

                var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.RiftPoints, request.RentalCost, connectionIn)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL, "Insufficient Rim for pawn rental.");

                response.TotalRim = walletUpdate.Value;

                rentalRecord = RentalPawnRecord.FromPawn(pawn, ownerCharacter);
                Server.Database.InsertRentalPawn(client.Character.CharacterId, rentalRecord, AdventureCount, CraftCount, connectionIn);

                packetQueue.AddRange(Server.AchievementManager.HandleHirePawn(client, connectionIn));
            });
            
            if (rentalRecord is not null)
            {
                var rentalPawn = rentalRecord.ToRentalPawn(client.Character.CharacterId, AdventureCount, CraftCount);
                rentalPawn.MaxAdventureCount = Server.GameSettings.GameServerSettings.RentalPawnAdventureCount;
                rentalPawn.MaxCraftCount = Server.GameSettings.GameServerSettings.RentalPawnCraftCount;
                
                client.Character.RentedPawns.Add(rentalPawn);
            }

            client.Enqueue(response, packetQueue);

            return packetQueue;
        }
    }
}
