using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters;

internal sealed class ServerSupportPawnManager(DdonGameServer server)
{
    private readonly DdonGameServer Server = server;

    public List<Pawn> SelectAvailablePawns(Character character, DbConnection connection)
    {
        var definitions = Server.Database.SelectServerSupportPawnDefinitions(connection)
            .Where(x => x.Enabled && IsQuestRequirementSatisfied(character, x.RequiredQuestId))
            .ToDictionary(x => x.PawnId);

        return Server.Database.SelectPawnsByCharacterId(Character.ServerCharacterId, connection)
            .Where(x => definitions.ContainsKey(x.PawnId))
            .ToList();
    }

    public bool IsAvailable(Character character, uint pawnId, DbConnection connection)
    {
        var definition = Server.Database.SelectServerSupportPawnDefinitions(connection)
            .FirstOrDefault(x => x.PawnId == pawnId);

        return definition is not null
            && definition.Enabled
            && IsQuestRequirementSatisfied(character, definition.RequiredQuestId)
            && Server.Database.SelectPawnsByCharacterId(Character.ServerCharacterId, connection)
                .Any(x => x.PawnId == pawnId);
    }

    public bool IsManaged(uint pawnId, DbConnection connection)
    {
        return Server.Database.SelectServerSupportPawnDefinitions(connection)
            .Any(x => x.PawnId == pawnId);
    }

    public static bool MatchesSearch(Pawn pawn, CDataPawnSearchParameter search)
    {
        if (search.IsClan || search.IsFriend)
            return false;

        if (search.OwnerCharacterName.FirstName.Length > 0
            && !Character.ServerCharacterFirstName.Contains(search.OwnerCharacterName.FirstName, StringComparison.OrdinalIgnoreCase))
            return false;

        if (search.OwnerCharacterName.LastName.Length > 0)
            return false;

        if (search.PawnName.Length > 0
            && !pawn.Name.Contains(search.PawnName, StringComparison.OrdinalIgnoreCase))
            return false;

        if (search.Sex != PawnSex.Any && pawn.EditInfo.Sex != (byte)search.Sex)
            return false;

        var activeJob = pawn.CharacterJobDataList.FirstOrDefault(x => x.Job == pawn.Job);
        if (activeJob is null)
            return false;

        if (search.CharacterParam.VocationMin != 0 && activeJob.Lv < search.CharacterParam.VocationMin)
            return false;
        if (search.CharacterParam.VocationMax != 0 && activeJob.Lv > search.CharacterParam.VocationMax)
            return false;
        if (search.CharacterParam.Job != 0
            && (search.CharacterParam.Job & (1u << (int)pawn.Job)) == 0)
            return false;
        if (search.CraftRankMin != 0 && pawn.CraftData.CraftRank < search.CraftRankMin)
            return false;
        if (search.CraftRankMax != 0 && pawn.CraftData.CraftRank > search.CraftRankMax)
            return false;

        var craftLevels = search.CraftSkillList.ToDictionary(x => x.Type, x => x.Level);
        return pawn.CraftData.PawnCraftSkillList.All(skill =>
            !craftLevels.TryGetValue(skill.Type, out uint minimum) || skill.Level >= minimum);
    }

    public static CDataRegisterdPawnList ToListEntry(Pawn pawn)
    {
        return new CDataRegisterdPawnList
        {
            PawnId = pawn.PawnId,
            Name = pawn.Name,
            Sex = pawn.EditInfo.Sex,
            Updated = DateTimeOffset.UtcNow,
            PawnListData = pawn.CDataRegisterdPawnList.PawnListData,
        };
    }

    private static bool IsQuestRequirementSatisfied(Character character, QuestId requiredQuestId)
    {
        return requiredQuestId == QuestId.None || character.HasQuestCompleted(requiredQuestId);
    }
}
