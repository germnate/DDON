#nullable enable
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PawnExpeditionManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionManager));

        private readonly DdonGameServer Server;

        public PawnExpeditionManager(DdonGameServer server)
        {
            Server = server;
        }

        // Hardcoded pool of (AreaId, SpotId) sally destinations, since no expedition
        // area/spot asset data currently exists in the repository.
        public static readonly List<CDataAreaSpotSet> SallySpots = new()
        {
            new() { AreaId = QuestAreaId.HidellPlains, SpotId = 1 },
            new() { AreaId = QuestAreaId.HidellPlains, SpotId = 2 },
            new() { AreaId = QuestAreaId.BreyaCoast, SpotId = 1 },
            new() { AreaId = QuestAreaId.BitterblackMaze, SpotId = 1 },
        };

        private static readonly List<ItemId> NormalRewardPool = new()
        {
            ItemId.AnimalBone,
            ItemId.AnimalPelt,
            ItemId.BeastLeather,
            ItemId.BabyHerb,
            ItemId.BlackOre,
            ItemId.BlackWood,
        };

        private static readonly List<ItemId> GoldenRewardPool = new()
        {
            ItemId.AncientIronOre,
            ItemId.AdamasOre,
            ItemId.BeastLeather,
            ItemId.BlackOre,
        };

        private byte MaxSallyCount => Server.GameSettings.GameServerSettings.PawnExpeditionMaxSallyCount;
        private uint SallyDurationInSeconds => Server.GameSettings.GameServerSettings.PawnExpeditionSallyDurationInSeconds;

        public static PawnExpeditionRecord CreateDefaultRecord(uint characterId)
        {
            return new PawnExpeditionRecord()
            {
                CharacterId = characterId,
                PawnId = 0,
                Status = PawnExpeditionStatus.Tired,
                SallyCount = 1
            };
        }

        public static bool CanPersistRecord(PawnExpeditionRecord record)
        {
            return record.PawnId != 0;
        }

        public PawnExpeditionRecord GetOrCreateRecord(GameClient client, DbConnection? connectionIn = null)
        {
            return GetOrCreateRecord(client, 0, connectionIn);
        }

        public PawnExpeditionRecord GetOrCreateRecord(GameClient client, uint pawnId, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                if (pawnId == 0)
                {
                    PawnExpeditionRecord? currentRecord = GetCurrentRecord(client, connection);
                    return currentRecord ?? CreateDefaultRecord(client.Character.CharacterId);
                }

                PawnExpeditionRecord? record = Server.Database.GetPawnExpeditionRecord(client.Character.CharacterId, connection, pawnId);
                if (record == null)
                {
                    record = new PawnExpeditionRecord()
                    {
                        CharacterId = client.Character.CharacterId,
                        PawnId = pawnId,
                        Status = PawnExpeditionStatus.Tired,
                        SallyCount = 1
                    };
                    Server.Database.UpsertPawnExpeditionRecord(record, connection);
                    return record;
                }

                return TryFinalizeSally(record, connection);
            });
        }

        private PawnExpeditionRecord? GetCurrentRecord(GameClient client, DbConnection? connectionIn = null)
        {
            List<PawnExpeditionRecord> records = Server.Database
                .GetPawnExpeditionRecordsForClanMembers(new List<uint> { client.Character.CharacterId }, connectionIn);
            PawnExpeditionRecord? record = records
                .OrderBy(x => GetRecordPriority(x.Status))
                .ThenByDescending(x => x.SallyStartTime ?? DateTime.MinValue)
                .ThenByDescending(x => x.SallyCount)
                .ThenBy(x => x.PawnId)
                .FirstOrDefault();

            return record == null ? null : TryFinalizeSally(record, connectionIn);
        }

        private static int GetRecordPriority(PawnExpeditionStatus status)
        {
            return status switch
            {
                PawnExpeditionStatus.OnSally => 0,
                PawnExpeditionStatus.Returned => 1,
                PawnExpeditionStatus.Tired => 2,
                _ => 3
            };
        }

        /// <summary>
        /// If the character's sally has run its course, generates the battle result/reward
        /// boxes and moves the record into the "Returned" state. This is evaluated lazily
        /// (wall-clock based) since Pawn Expeditions must survive server restarts.
        /// </summary>
        private PawnExpeditionRecord TryFinalizeSally(PawnExpeditionRecord record, DbConnection? connectionIn = null)
        {
            if (record.Status != PawnExpeditionStatus.OnSally || record.SallyStartTime == null)
            {
                return record;
            }

            if (DateTime.UtcNow - record.SallyStartTime.Value < TimeSpan.FromSeconds(SallyDurationInSeconds))
            {
                return record;
            }

            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                GenerateRewardBox(record, connection);
                record.Status = PawnExpeditionStatus.Returned;
                Server.Database.UpsertPawnExpeditionRecord(record, connection);
                return record;
            });
        }

        private void GenerateRewardBox(PawnExpeditionRecord record, DbConnection? connectionIn = null)
        {
            byte mdlType = (byte)(record.IsGoldenSally ? 2 : 1);
            uint boxId = Server.Database.InsertPawnExpeditionRewardBox(record.CharacterId, mdlType, connectionIn);

            List<ItemId> pool = record.IsGoldenSally ? GoldenRewardPool : NormalRewardPool;
            int itemCount = Random.Shared.Next(2, 5);
            for (uint slotNo = 0; slotNo < itemCount; slotNo++)
            {
                ItemId itemId = pool[Random.Shared.Next(pool.Count)];
                Server.Database.InsertPawnExpeditionRewardBoxItem(new PawnExpeditionRewardBoxItem()
                {
                    BoxId = boxId,
                    SlotNo = slotNo,
                    ItemId = (uint)itemId,
                    ItemNum = (uint)Random.Shared.Next(1, 4),
                    Quality = 0,
                    IsHidden = false,
                    Claimed = false
                }, connectionIn);
            }
        }

        public CDataBattleResultInfo GetLastBattleResult(PawnExpeditionRecord record)
        {
            // Battle results aren't currently persisted, so a plausible result
            // is synthesized deterministically enough for display purposes.
            return new CDataBattleResultInfo()
            {
                EnemyId = 1,
                EnemyNum = (uint)Random.Shared.Next(1, 6),
                EnemyLevel = (uint)Random.Shared.Next(1, 50)
            };
        }

        /// <summary>
        /// Marks the reward summary for a completed sally as viewed, resetting the record so
        /// a new sally can be started. Reward boxes remain claimable independently of this.
        /// </summary>
        public bool FinishViewingReward(GameClient client, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                PawnExpeditionRecord record = GetOrCreateRecord(client, connectionIn: connection);
                if (!CanPersistRecord(record) || record.Status != PawnExpeditionStatus.Returned)
                {
                    return false;
                }

                record.Status = PawnExpeditionStatus.Tired;
                record.AreaId = 0;
                record.SpotId = 0;
                record.IsGoldenSally = false;
                record.SallyStartTime = null;

                return Server.Database.UpsertPawnExpeditionRecord(record, connection);
            });
        }

        public bool StartSally(GameClient client, uint pawnId, uint areaId, uint spotId, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                if (pawnId == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_POINTER_NOT_EXIST, "Pawn expedition requires a valid pawn id");
                }

                PawnExpeditionRecord? currentRecord = GetCurrentRecord(client, connection);
                if (currentRecord?.Status == PawnExpeditionStatus.OnSally && currentRecord.PawnId != pawnId)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_NOT_SALLY, "A sally is already in progress");
                }

                PawnExpeditionRecord record = GetOrCreateRecord(client, pawnId, connection);
                if (!CanPersistRecord(record))
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_POINTER_NOT_EXIST, "Pawn expedition requires a valid pawn id");
                }

                if (record.Status == PawnExpeditionStatus.OnSally)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_NOT_SALLY, "A sally is already in progress");
                }

                if (record.SallyCount == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_POINTER_NOT_EXIST, "No sally count remaining");
                }

                record.Status = PawnExpeditionStatus.OnSally;
                record.PawnId = pawnId;
                record.AreaId = areaId;
                record.SpotId = spotId;
                record.IsHotSpot = SallySpots.Any(x => (uint)x.AreaId == areaId && x.SpotId == spotId) && Random.Shared.NextDouble() < 0.2;
                record.SallyStartTime = DateTime.UtcNow;
                record.SallyCount--;

                return Server.Database.UpsertPawnExpeditionRecord(record, connection);
            });
        }

        public bool CancelSally(GameClient client, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                PawnExpeditionRecord record = GetOrCreateRecord(client, connectionIn: connection);
                if (!CanPersistRecord(record))
                {
                    return false;
                }

                if (record.Status != PawnExpeditionStatus.OnSally)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_NOT_SALLY, "No sally in progress to cancel");
                }

                record.Status = PawnExpeditionStatus.Tired;
                record.AreaId = 0;
                record.SpotId = 0;
                record.IsHotSpot = false;
                record.IsGoldenSally = false;
                record.SallyStartTime = null;
                if (record.SallyCount < MaxSallyCount)
                {
                    record.SallyCount++;
                }

                return Server.Database.UpsertPawnExpeditionRecord(record, connection);
            });
        }

        public void ChargeSallyCount(GameClient client, byte price, DbConnection? connectionIn = null)
        {
            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                byte expectedPrice = Server.GameSettings.GameServerSettings.PawnExpeditionChargeSallyCountPrice;
                if (price != expectedPrice)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_DIFFERENT_PRICE, $"Expected price {expectedPrice}, got {price}");
                }

                PawnExpeditionRecord record = GetOrCreateRecord(client, connectionIn: connection);
                if (!CanPersistRecord(record))
                {
                    return false;
                }

                if (record.SallyCount >= MaxSallyCount)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_POINTER_NOT_EXIST, "Sally count is already at maximum");
                }

                uint cost = Server.GameSettings.GameServerSettings.PawnExpeditionPriceGoldPerUnit * price;
                Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection);

                record.SallyCount++;
                Server.Database.UpsertPawnExpeditionRecord(record, connection);
                return true;
            });
        }

        public void ChangeGoldenSally(GameClient client, byte price, DbConnection? connectionIn = null)
        {
            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                byte expectedPrice = Server.GameSettings.GameServerSettings.PawnExpeditionGoldenSallyPrice;
                if (price != expectedPrice)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_DIFFERENT_PRICE, $"Expected price {expectedPrice}, got {price}");
                }

                PawnExpeditionRecord record = GetOrCreateRecord(client, connectionIn: connection);
                if (!CanPersistRecord(record))
                {
                    return false;
                }

                if (record.Status == PawnExpeditionStatus.OnSally)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_NOT_SALLY, "Cannot change golden sally state while a sally is in progress");
                }

                uint cost = Server.GameSettings.GameServerSettings.PawnExpeditionPriceGoldPerUnit * price;
                Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection);

                record.IsGoldenSally = true;
                Server.Database.UpsertPawnExpeditionRecord(record, connection);
                return true;
            });
        }

        public List<CDataPawnExpeditionClanSallySpotInfo> GetClanSallySpotInfoList(GameClient client, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                List<CDataClanMemberInfo> members = Server.Database.GetClanMemberList(client.Character.ClanId, connection);
                List<uint> characterIds = members.Select(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId).ToList();
                List<PawnExpeditionRecord> records = Server.Database.GetPawnExpeditionRecordsForClanMembers(characterIds, connection);

                return records
                    .Where(x => x.Status == PawnExpeditionStatus.OnSally)
                    .GroupBy(x => (x.AreaId, x.SpotId))
                    .Select(g => new CDataPawnExpeditionClanSallySpotInfo()
                    {
                        AreaId = g.Key.AreaId,
                        SpotId = g.Key.SpotId,
                        SallyNum = (uint)g.Count()
                    })
                    .ToList();
            });
        }
    }
}
