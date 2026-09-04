using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceCharacterEndBadStatusHandler : GameStructurePacketHandler<C2SInstanceCharacterEndBadStatusNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceCharacterEndBadStatusHandler));

        public InstanceCharacterEndBadStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceCharacterEndBadStatusNtc> packet)
        {
            Logger.Debug($"CharacterEndBadStatusNtc: CharacterId={client.Character.CharacterId}, CommonId={client.Character.CommonId}, StatusId={packet.Structure.StatusId}, StageId={client.Character.Stage.Id}, StageGroupId={client.Character.Stage.GroupId}, StageNo={client.Character.StageNo}");
        }
    }
}
