using System;
using System.Collections.Generic;
using System.Linq;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "clone";
    public override string HelpText =>
        "usage: `/clone <slot 1-10>` - Save or overwrite a pawn clone from your current character.\n" +
        "       `/clone list` - List your saved pawn clones.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 1 && command[0].Equals("list", StringComparison.OrdinalIgnoreCase))
        {
            var clones = server.PawnCloneManager.List(client.Character).ToDictionary(x => x.Slot);
            for (byte slot = 1; slot <= 10; slot++)
            {
                if (!clones.TryGetValue(slot, out var clone))
                {
                    responses.Add(ChatResponse.ServerMessage(client, $"Clone slot {slot}: empty."));
                    continue;
                }

                responses.Add(ChatResponse.ServerMessage(
                    client,
                    $"Clone slot {slot}: {clone.Record.Name} - {clone.Record.Job}, Lv. {clone.Record.CharacterJobData.Lv} (Pawn ID {clone.PawnId})."));
            }

            return;
        }

        string slotArgument = command.Length == 2
            && (command[0].Equals("save", StringComparison.OrdinalIgnoreCase)
                || command[0].Equals("saveslot", StringComparison.OrdinalIgnoreCase))
            ? command[1]
            : command.Length == 1 ? command[0] : string.Empty;
        if (!byte.TryParse(slotArgument, out byte saveSlot) || saveSlot is < 1 or > 10)
        {
            responses.Add(ChatResponse.CommandError(client, "Usage: `/clone <slot 1-10>` or `/clone list`."));
            return;
        }

        if (client.Character.ActiveCharacterJobData is null)
        {
            responses.Add(ChatResponse.CommandError(client, "Your current vocation has no saved job data and cannot be cloned."));
            return;
        }

        var savedClone = server.PawnCloneManager.Save(client.Character, saveSlot);
        responses.Add(ChatResponse.ServerMessage(
            client,
            $"Saved {savedClone.Record.Name} to clone slot {saveSlot}; other players can now find and hire it."));
    }
}

return new ChatCommand();
