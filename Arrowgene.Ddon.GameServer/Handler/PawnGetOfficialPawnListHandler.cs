using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetOfficialPawnListHandler : GameRequestPacketHandler<C2SPawnGetOfficialPawnListReq, S2CPawnGetOfficialPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetOfficialPawnListHandler));

        public PawnGetOfficialPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetOfficialPawnListRes Handle(GameClient client, C2SPawnGetOfficialPawnListReq request)
        {
            var results = new S2CPawnGetOfficialPawnListRes();

            Server.Database.ExecuteInTransaction(connection =>
            {
                var officialPawnIds = Server.Database.SelectOfficialPawns(connection);
                foreach (var pawnId in officialPawnIds)
                {
                    var pawn = Server.Database.SelectPawn(connection, pawnId);
                    results.OfficialPawnList.Add(new CDataRegisterdPawnList()
                    {
                        Name = pawn.Name,
                        Sex = pawn.EditInfo.Sex,
                        Updated = DateTimeOffset.UtcNow,
                        PawnId = pawn.PawnId,
                        PawnListData = new CDataPawnListData()
                        {
                            Job = pawn.Job,
                            Level = pawn.ActiveCharacterJobData.Lv,
                            PawnCraftSkillList = pawn.CraftData.PawnCraftSkillList,
                        }
                    });
                }
            });
            

            var mixin = Server.ScriptManager.MixinModule.Get<IRentalCostMixin>("rental_cost");
            foreach (var registeredPawn in results.OfficialPawnList)
            {
                // TODO: Should official pawns always be discounted? Or have some other adjustment?
                registeredPawn.RentalCost = mixin.GetRentalCost(client, registeredPawn, false);
            }

            return results;
        }
    }
}
