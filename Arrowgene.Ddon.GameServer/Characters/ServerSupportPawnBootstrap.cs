using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters;

internal static class ServerSupportPawnBootstrap
{
    private const string PawnName = "ServerPawn";
    private const uint PawnLevel = 16;

    public static void EnsureSeeded(DdonGameServer server)
    {
        Pawn? pawn = server.Database
            .SelectPawnsByCharacterId(Character.ServerCharacterId)
            .FirstOrDefault(x => x.Name == PawnName);

        if (pawn is null)
        {
            pawn = CreatePawn(server);
            if (!server.Database.CreatePawn(pawn))
                throw new InvalidOperationException($"Could not create server support pawn '{PawnName}'.");

            // Player pawns get this record inserted as part of PawnCreatePawnHandler.
            // Without it, CharacterManager.UpdateCharacterExtendedParams() blows up
            // (NullReferenceException on pawn.ExtendedParams) the next time the pawn
            // is loaded, which is what left the pawn stuck at 0 HP and unhireable.
            if (!server.Database.InsertGainExtendParam(pawn.CommonId, pawn.ExtendedParams))
                throw new InvalidOperationException($"Could not initialize extended params for server support pawn '{PawnName}'.");

            // CharacterManager.UpdateCharacterExtendedParams() (re-)computes StatusInfo.MaxHP
            // etc. from ExtendedParams every time the pawn's owner "character" (id 40) is
            // loaded via SelectCharacter -> SelectPawns, now that ExtendedParams is present.
        }
        else if (server.Database.SelectOrbGainExtendParam(pawn.CommonId) is null)
        {
            // Repair pawns that were created by older, buggy versions of this bootstrap
            // before the ddon_orb_gain_extend_param row was seeded.
            if (!server.Database.InsertGainExtendParam(pawn.CommonId, new CDataOrbGainExtendParam()))
                throw new InvalidOperationException($"Could not repair extended params for server support pawn '{PawnName}'.");
        }

        if (!server.Database.InsertServerSupportPawnDefinition(new ServerSupportPawnDefinition
        {
            PawnId = pawn.PawnId,
            RequiredQuestId = QuestId.None,
            Enabled = true
        }))
        {
            throw new InvalidOperationException($"Could not register server support pawn '{PawnName}'.");
        }
    }

    private static Pawn CreatePawn(DdonGameServer server)
    {
        ArisenCsv preset = server.AssetRepository.ArisenAsset.Single(x => x.Job == JobId.Fighter);

        return new Pawn(Character.ServerCharacterId)
        {
            Name = PawnName,
            Job = JobId.Fighter,
            HmType = 1,
            PawnType = PawnType.Support,
            ExtendedParams = new CDataOrbGainExtendParam(),
            StatusInfo = new CDataStatusInfo
            {
                HP = preset.HP,
                MaxHP = preset.MaxHP,
                WhiteHP = preset.WhiteHP,
                Stamina = preset.Stamina,
                MaxStamina = preset.MaxStamina,
                RevivePoint = preset.RevivePoint
            },
            CharacterJobDataList =
            [
                new CDataCharacterJobData
                {
                    Job = JobId.Fighter,
                    Lv = PawnLevel,
                    Exp = preset.Exp,
                    JobPoint = preset.JobPoint,
                    Atk = preset.PAtk,
                    Def = preset.PDef,
                    MAtk = preset.MAtk,
                    MDef = preset.MDef
                }
            ],
            TrainingPoints = uint.MaxValue,
            AvailableTraining = uint.MaxValue
        };
    }
}
