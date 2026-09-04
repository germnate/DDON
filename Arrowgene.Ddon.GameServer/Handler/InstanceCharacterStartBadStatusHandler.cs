using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceCharacterStartBadStatusHandler : GameStructurePacketHandler<C2SInstanceCharacterStartBadStatusNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceCharacterStartBadStatusHandler));

        public InstanceCharacterStartBadStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceCharacterStartBadStatusNtc> packet)
        {
            Logger.Debug($"CharacterStartBadStatusNtc: CharacterId={client.Character.CharacterId}, CommonId={client.Character.CommonId}, StatusId={packet.Structure.StatusId}, StageId={client.Character.Stage.Id}, StageGroupId={client.Character.Stage.GroupId}, StageNo={client.Character.StageNo}");

            if (Server.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                Server.EpitaphRoadManager.EvaluatePlayerAbnormalStatus(client.Party, packet.Structure.StatusId);
            }
        }
    }
}
