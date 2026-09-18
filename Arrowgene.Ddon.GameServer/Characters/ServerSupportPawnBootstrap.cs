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
