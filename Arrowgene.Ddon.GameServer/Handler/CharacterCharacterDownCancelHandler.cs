using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterDownCancelHandler : GameStructurePacketHandler<C2SCharacterCharacterDownCancelNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterDownCancelHandler));

        public CharacterCharacterDownCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterDownCancelNtc> packet)
        {
            Logger.Debug($"CharacterDownCancelNtc: CharacterId={client.Character.CharacterId}, CommonId={client.Character.CommonId}, StageId={client.Character.Stage.Id}, StageGroupId={client.Character.Stage.GroupId}, StageNo={client.Character.StageNo}");
        }
    }
}
