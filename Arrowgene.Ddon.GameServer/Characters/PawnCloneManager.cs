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
            List<CustomSkill> learnedCustomSkills = character.LearnedCustomSkills
                .Where(x => x.Job == job)
                .ToList();
            List<CustomSkill> equippedCustomSkills = character.EquippedCustomSkillsDictionary
                .GetValueOrDefault(job, [])
                .ToList();
            foreach (CustomSkill skill in equippedCustomSkills.Where(x => x is not null))
            {
                if (!learnedCustomSkills.Any(x => x.Job == skill.Job && x.SkillId == skill.SkillId))
                {
                    learnedCustomSkills.Add(skill);
                }
            }

            List<Ability> learnedAbilities = character.LearnedAbilities
                .Where(x => x.Job == job)
                .ToList();
            List<Ability> equippedAbilities = character.EquippedAbilitiesDictionary
                .GetValueOrDefault(job, [])
                .ToList();
            foreach (Ability ability in equippedAbilities.Where(x => x is not null))
            {
                if (!learnedAbilities.Any(x => x.Job == ability.Job && x.AbilityId == ability.AbilityId))
                {
                    learnedAbilities.Add(ability);
                }
            }

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
                LearnedCustomSkills = learnedCustomSkills,
                EquippedCustomSkillsDictionary = new()
                {
                    [job] = equippedCustomSkills
                },
                LearnedAbilities = learnedAbilities,
                EquippedAbilitiesDictionary = new()
                {
                    [job] = equippedAbilities
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
