using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftRecipeGetGradeupRecipeHandler : GameRequestPacketHandler<C2SCraftRecipeGetCraftGradeupRecipeReq, S2CCraftRecipeGetCraftGradeupRecipeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftRecipeGetGradeupRecipeHandler));

        public CraftRecipeGetGradeupRecipeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftRecipeGetCraftGradeupRecipeRes Handle(GameClient client, C2SCraftRecipeGetCraftGradeupRecipeReq request)
        {
            var response = new S2CCraftRecipeGetCraftGradeupRecipeRes()
            {
                Category = request.Category, 
                RecipeList = [],
                UpgradableItemList = [],
                IsEnd = true
            };
            return response;
        }
    }
}
