/*
 * Point modifier overrides.
 * This file is read at runtime (unlike templates) and supports hotloading.
 */

bool EnableAdjustPartyEnemyExp = true;

var AdjustPartyEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
    (     11,   200,           0.4),
};

bool EnableAdjustTargetLvEnemyExp = true;

var AdjustTargetLvEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
    (     11,   200,           0.4),
};

bool DisableExpCorrectionForMyPawn = true;

bool EnablePawnCatchup = true;
double PawnCatchupMultiplier = 1.5;
uint PawnCatchupLvDiff = 5;
