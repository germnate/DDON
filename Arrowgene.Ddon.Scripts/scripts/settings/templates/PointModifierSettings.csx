/*
 * Settings file for Server customization.
 * This file supports hotloading.
 */

/// <summary>
/// Handles EXP penalties for the party based on the
/// difference between the lowest leveled member and highest
/// leveled member of the party.If the range is larger than
/// the last entry in AdjustPartyEnemyExpTiers, a 0% exp rate
/// is automatically applied.
/// </summary>
bool EnableAdjustPartyEnemyExp = true;

/// <summary>
/// The ranges used when EnableAdjustPartyEnemyExp is true.
/// </summary>
var AdjustPartyEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv and MaxLv define the relative level difference between the levels of the lowest and
    // highest members in the party.
    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)
    //
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

/// <summary>
/// Handles EXP penalties based on the highest leveled member
/// in the party and the level of the target enemy.If the range is
/// larger than the last entry in AdjustTargetLvEnemyExpTiers, a 0%
/// exp rate is automatically applied.
/// </summary>
bool EnableAdjustTargetLvEnemyExp = false;

/// <summary>
/// The ranges used when EnableAdjustTargetLvEnemyExp is true.
/// </summary>
var AdjustTargetLvEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv and MaxLv define the relative level difference between the target and highest member in the party.
    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)
    //
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

/// <summary>
/// If set to true, pawns owned by the player will not be included in exp penalty calculations.
/// </summary>
bool DisableExpCorrectionForMyPawn = true;

/// <summary>
/// If set to true, if the pawn is PawnCatchupLvDiff or more levels behind the players current level, an exp multiplers of PawnCatchupMultiplier is applied.
/// </summary>
bool EnablePawnCatchup = true;

/// <summary>
/// The exp bonus applied when the pawn catchup mechanic takes place if EnablePawnCatchup is set to true and the level difference requirements are met.
/// </summary>
double PawnCatchupMultiplier = 1.5;

/// <summary>
/// The minimum level difference required for the catchup mechanic to be active if EnablePawnCatchup is set to true.
/// </summary>
uint PawnCatchupLvDiff = 5;

