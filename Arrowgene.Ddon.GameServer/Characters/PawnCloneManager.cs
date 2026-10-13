using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PawnCloneManager
    {
        private readonly DdonGameServer Server;

        public PawnCloneManager(DdonGameServer server)
        {
            Server = server;
        }

        public PawnClone Save(Character character, byte slot)
        {
            if (slot is < 1 or > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, "Clone slots must be between 1 and 10.");
            }

            if (character.ActiveCharacterJobData is null)
            {
                throw new InvalidOperationException("The current vocation has no saved job data and cannot be cloned.");
            }

            var clonePawn = CreatePawnSnapshot(character, slot);
            var rentalRecord = RentalPawnRecord.FromPawn(clonePawn, character);
            return Server.Database.SavePawnClone(character.CharacterId, slot, clonePawn, rentalRecord);
        }

        public List<PawnClone> List(Character character)
        {
            return Server.Database.SelectPawnClones(character.CharacterId);
        }

        private static Pawn CreatePawnSnapshot(Character character, byte slot)
        {
            JobId job = character.Job;
            var activeJobData = character.ActiveCharacterJobData
                ?? throw new InvalidOperationException("The current vocation has no saved job data and cannot be cloned.");
            var clonePawn = new Pawn(character.CharacterId)
            {
                Name = $"{character.FirstName} Clone {slot}",
                HmType = 1,
                PawnType = PawnType.Support,
                PawnState = PawnState.None,
                Job = job,
                HideEquipHead = character.HideEquipHead,
                HideEquipLantern = character.HideEquipLantern,
                EditInfo = character.EditInfo,
                StatusInfo = character.StatusInfo,
                CharacterJobDataList = [activeJobData],
                EquipmentTemplate = character.EquipmentTemplate,
                Equipment = character.Equipment,
                JewelrySlotNum = character.JewelrySlotNum,
                LearnedNormalSkills = [.. character.LearnedNormalSkills.Where(x => x.Job == job)],
                LearnedCustomSkills = [.. character.LearnedCustomSkills.Where(x => x.Job == job)],
                EquippedCustomSkillsDictionary = new()
                {
                    [job] = [.. character.EquippedCustomSkillsDictionary.GetValueOrDefault(job, [])]
                },
                LearnedAbilities = [.. character.LearnedAbilities.Where(x => x.Job == job)],
                EquippedAbilitiesDictionary = new()
                {
                    [job] = [.. character.EquippedAbilitiesDictionary.GetValueOrDefault(job, [])]
                },
                ExtendedParams = character.ExtendedParams,
                ExtendedJobParams = character.ExtendedJobParams,
                ReleasedExtendedJobParams = character.ReleasedExtendedJobParams,
                UnlockedCustomSkills = character.UnlockedCustomSkills,
                UnlockedAbilities = character.UnlockedAbilities,
                EmblemStatList = character.EmblemStatList,
                OrbRelease = character.OrbRelease,
                CharacterProfile = character.CharacterProfile,
                Server = character.Server,
            };

            return clonePawn;
        }
    }
}
