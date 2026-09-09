using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    /// <summary>
    /// Drives "key enemy" doors from the server. The client normally opens these doors itself when
    /// it receives <c>S2CInstanceEnemyGroupDestroyNtc</c> for the enemy group its stage data marks
    /// as the key group. For stages where that mapping is unavailable, KeyDoors.json maps an enemy
    /// group onto the object-manager states that open the door, and this manager pushes them.
    /// Opened doors are tracked per party and re-sent on area change so they stay open.
    /// </summary>
    public class KeyDoorManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(KeyDoorManager));

        private readonly DdonGameServer _server;
        private readonly Dictionary<uint, HashSet<(uint StageId, uint GroupId, uint PosId, byte State)>> _openedDoors;

        public KeyDoorManager(DdonGameServer server)
        {
            _server = server;
            _openedDoors = new Dictionary<uint, HashSet<(uint, uint, uint, byte)>>();
        }

        public void ResetInstance(PartyGroup party)
        {
            lock (_openedDoors)
            {
                _openedDoors.Remove(party.Id);
            }
        }

        /// <summary>
        /// Called when every required enemy of an enemy group has been killed. Opens any key door
        /// registered against that group.
        /// </summary>
        public void EvaluateGroupDestroyed(PartyGroup party, StageLayoutId stageId, PacketQueue queue)
        {
            var entries = _server.AssetRepository.KeyDoorAsset
                .Where(x => x.StageId == stageId.Id && x.LayerNo == stageId.LayerNo && x.GroupId == stageId.GroupId)
                .ToList();

            if (entries.Count == 0)
            {
                return;
            }

            foreach (var entry in entries)
            {
                foreach (var om in entry.Opens)
                {
                    uint omStageId = om.StageId == 0 ? entry.StageId : om.StageId;

                    lock (_openedDoors)
                    {
                        if (!_openedDoors.TryGetValue(party.Id, out var opened))
                        {
                            opened = new HashSet<(uint, uint, uint, byte)>();
                            _openedDoors[party.Id] = opened;
                        }
                        opened.Add((omStageId, om.GroupId, om.PosId, om.State));
                    }

                    party.EnqueueToAll(BuildNtc(omStageId, om.GroupId, om.PosId, om.State), queue);
                    Logger.Info($"[KeyDoor] Party={party.Id} group {stageId} destroyed -> OM StageId={omStageId} GroupId={om.GroupId} PosId={om.PosId} State={om.State} ({entry.Comment})");
                }
            }
        }

        /// <summary>
        /// Re-sends every key door this party has already opened in the given stage. Without this
        /// the door reverts to its closed state whenever the stage is reloaded.
        /// </summary>
        public void AreaChange(GameClient client, uint stageId, PacketQueue queue)
        {
            if (client.Party is null)
            {
                return;
            }

            List<(uint StageId, uint GroupId, uint PosId, byte State)> doors;
            lock (_openedDoors)
            {
                if (!_openedDoors.TryGetValue(client.Party.Id, out var opened))
                {
                    return;
                }
                doors = opened.Where(x => x.StageId == stageId).ToList();
            }

            foreach (var door in doors)
            {
                client.Enqueue(BuildNtc(door.StageId, door.GroupId, door.PosId, door.State), queue);
                Logger.Info($"[KeyDoor] Restoring opened door for Party={client.Party.Id} StageId={door.StageId} GroupId={door.GroupId} PosId={door.PosId} State={door.State}");
            }
        }

        private static S2CSeasonDungeonSetOmStateNtc BuildNtc(uint stageId, uint groupId, uint posId, byte state)
        {
            return new S2CSeasonDungeonSetOmStateNtc()
            {
                LayoutId = new CDataStageLayoutId()
                {
                    StageId = stageId,
                    GroupId = groupId
                },
                PosId = posId,
                State = (SeasonDungeonOmState)state
            };
        }
    }
}
