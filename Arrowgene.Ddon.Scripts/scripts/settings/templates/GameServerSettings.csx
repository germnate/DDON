/*
 * Settings file for Server customization.
 * This file supports hotloading.
 */

/// <summary>
/// Additional factor to change how long crafting a recipe will take to finish.
/// </summary>
double AdditionalProductionSpeedFactor = 1;

/// <summary>
/// Additional factor to change how much a recipe will cost.
/// </summary>
double AdditionalCostPerformanceFactor = 1;

/// <summary>
/// The amount of seconds that the partner pawn must be a member of the
/// party, adventuring in a non-safe area to receive adventure credit
/// for the day.
/// </summary>
uint PartnerPawnAdventureDurationInSeconds = 1800;

/// <summary>
/// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
/// The default is a value of 10 which is equivalent to the original game's behavior.
/// </summary>
byte CraftConsumableProductionTimesMax = 10;

/// <summary>
/// Determines the maximum amount of items you can recycle/disassemble at Craig before a reset is required.
/// </summary>
byte CraftItemRecycleMax = 10;

/// <summary>
/// The amount of Golden Gemstones (GG) required to reset the recycle/disassemble count.
/// </summary>
byte CraftItemRecycleResetGGCost = 1;

/// <summary>
/// Modifier used to skew the randomness during equipment unlimit.
/// 
/// Example bias values (note that fractional amounts are also valid):
/// Bias of -1.0 Inverts the bias, favoring higher indices
/// Bias of 0.0 No bias, equal probabilty for all.
/// Bias of 1.0 Balanced bias towers lower indices
/// Bias of 2.0 strongly prefers lower indices
/// </summary>
double EquipmentLimitBreakBias = 1.5;

/// <summary>
/// The number of real world minutes that make up an in-game day.
/// </summary>
uint GameClockTimescale = 90;

/// <summary>
/// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
/// </summary>
uint WeatherSequenceLength = 20;

/// <summary>
/// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
/// meanLength: Average length of the weather, in seconds, when it gets rolled.
/// weight: Relative weight of rolling that weather. Set to 0 to disable.
/// </summary>
var WeatherStatistics = new List<(uint MeanLength, uint Weight)>
{
    (60 * 30, 1), // Fair
    (60 * 30, 1), // Cloudy
    (60 * 30, 1), // Windy
};

/// <summary>
/// Configures the default time in seconds a lantern is active after igniting it.
/// </summary>
uint LanternBurnTimeInSeconds = 1500;

/// <summary>
/// When using the adventure guide, configures the listing level range +/- the value
/// of the level of the current job when displaying world quests.
/// </summary>
uint AdventureGuideLevelRangeFilter = 10;

/// <summary>
/// Configures the maximum amount of quests to display in the adventure guide
/// at one time.
/// </summary>
uint AdventureGuideMaxQuestList = 50;

/// <summary>
/// Uses the automatic exp calculation system for all enemies instead of just using the
/// ones marked in quest files.
/// </summary>
bool EnableAutomaticExpCalculationForAll = false;

/// <summary>
/// When set to true, if the party leader has the content unlock of "OrbEnemy", random enemies
/// will appear as "Blood Orb [name]" each time the instance is reset. The amount of BO will
/// be calculated based on the enemy level.
/// </summary>
bool EnableRandomizedBoEnemies = false;

/// <summary>
/// If EnableRandomizedBoEnemies is set to true, this setting configures the chance % that
/// an enemy will be upgraded to being a BoEnemy instead of a normal enemy.
/// </summary>
double RandomizedBoEnemyChance = 0.05;

/// <summary>
/// Maximum amount of play points the client will display in the UI.
/// Play points past this point will also trigger a chat log message saying you've reached the cap.
/// </summary>
uint PlayPointMax = 2000;

/// <summary>
/// Maximum level for each job.
/// Shared with the login server.
/// Level caps based on season release
/// Alpha:        10
/// CBT           15
/// Season 1.0:   40
/// Season 1.1:   45
/// Season 1.2:   55
/// Season 1.3:   60
/// Season 2.0:   65
/// Season 2.1:   70
/// Season 2.2:   75
/// Season 2.3:   80
/// Season 3.0:   85
/// Season 3.1:   90
/// Season 3.2:   95
/// Season 3.3:  100
/// Season 3.41: 105
/// Season 3.42: 110
/// Season 3.43: 120
/// </summary>
uint JobLevelMax = 120;

/// <summary>
/// The maximum job points which a job can own at a given time.
/// job points past this point will trigger a UI message saying
/// you can't earn anymore.
/// </summary>
uint JobPointMax = 500000;

/// <summary>
/// Maximum number of members in a single clan.
/// Shared with the login server.
/// </summary>
uint ClanMemberMax = 100;

/// <summary>
/// Maximum number of characters per account.
/// Shared with the login server.
/// </summary>
byte CharacterNumMax = 4;

/// <summary>
/// Toggles the visual equip set for all characters.
/// Shared with the login server.
/// </summary>
bool EnableVisualEquip = true;

/// <summary>
/// Maximum entries in the friends list.
/// Shared with the login server.
/// </summary>
uint FriendListMax = 200;

/// <summary>
/// Limits for each wallet type.
/// </summary>
var WalletLimits = new Dictionary<WalletType, uint>()
{
    {WalletType.Gold, 999999999},
    {WalletType.RiftPoints, 999999999},
    {WalletType.BloodOrbs, 500000},
    {WalletType.SilverTickets, 999999999},
    {WalletType.GoldenGemstones, 99999},
    {WalletType.RentalPoints, 99999},
    {WalletType.ResetJobPoints, 99},
    {WalletType.ResetCraftSkills, 99},
    {WalletType.HighOrbs, 5000},
    {WalletType.DominionPoints, 999999999},
    {WalletType.AdventurePassPoints, 80},
    {WalletType.CustomMadeServiceTickets, 999999999},
    {WalletType.BitterblackMazeResetTicket, 3},
    {WalletType.GoldenDragonMark, 30},
    {WalletType.SilverDragonMark, 150},
    {WalletType.RedDragonMark, 99999},
};

/// <summary>
/// Number of bazaar entries that are given to new characters.
/// </summary>
uint DefaultMaxBazaarExhibits = 5;

/// <summary>
/// Number of favorite warps that are given to new characters.
/// </summary>
uint DefaultWarpFavorites = 5;

/// <summary>
/// Controls the party size for regular adventuring content.
/// Used to control main pawns auto-joining parties alongside their owners.
/// </summary>
uint NormalPartySize = 4;

/// <summary>
/// Global modifier for enemy exp calculations to scale up or down.
/// </summary>
double EnemyExpModifier = 1;

/// <summary>
/// Global modifier for BBM enemy exp calculations to scale up or down.
/// </summary>
double BBMEnemyExpModifier = 1;

/// <summary>
/// Global modifier for quest exp calculations to scale up or down.
/// </summary>
double QuestExpModifier = 1;

/// <summary>
/// Global modifier for playpoint calculations to scale up or down.
/// </summary>
double PpModifier = 1;

/// <summary>
/// Global modifier for Gold calculations to scale up or down.
/// </summary>
double GoldModifier = 1;

/// <summary>
/// Global modifier for Rift calculations to scale up or down.
/// </summary>
double RiftModifier = 1;

/// <summary>
/// Global modifier for BO calculations to scale up or down.
/// </summary>
double BoModifier = 1;

/// <summary>
/// Global modifier for HO calculations to scale up or down.
/// </summary>
double HoModifier = 1;

/// <summary>
/// Global modifier for JP calculations to scale up or down.
/// </summary>
double JpModifier = 1;

/// <summary>
/// Global modifier for AP calculations to scale up or down.
/// </summary>
double ApModifier = 1;

/// <summary>
/// Configures the maximum amount of reward box slots.
/// </summary>
byte RewardBoxMax = 100;

/// <summary>
/// Configures the maximum amount of quests that can be ordered at one time.
/// </summary>
byte QuestOrderMax = 20;

/// <summary>
/// Configures if epitaph rewards are limited once per weekly reset.
/// </summary>
bool EnableEpitaphWeeklyRewards = true;

/// <summary>
/// Enables main pawns in party to gain EXP and JP from quests
/// Original game apparantly did not have pawns share quest reward, so will set to false for default,
/// change as needed
/// </summary>
bool EnableMainPartyPawnsQuestRewards = false;

/// <summary>
/// Specifies the time in seconds that a bazaar exhibit will last.
/// By default, the equivalent of 3 days
/// </summary>
ulong BazaarExhibitionTimeSeconds = (ulong) TimeSpan.FromDays(3).TotalSeconds;

/// <summary>
/// Specifies the time in seconds that a slot in the bazaar won't be able to be used again.
/// By default, the equivalent of 1 day
/// </summary>
ulong BazaarCooldownTimeSeconds = (ulong) TimeSpan.FromDays(1).TotalSeconds;

/// <summary>
/// Minimum price in G for a single item on the bazaar.
/// </summary>
uint BazaarExhibitionMinPrice = 1;

/// <summary>
/// Maximum price in G for a single item on the bazaar.
/// This ends up being interpreted as a signed integer by the client, so its capped at ~2 billion.
/// </summary>
uint BazaarExhibitionMaxPrice = 99999;

/// <summary>
/// Number of items that can be included in a single exhibition on the bazaar.
/// </summary>
ushort BazaarExhibitionMaxItemNum = 20;

/// <summary>
/// Ties area rank progress to various paths to dungeons.
/// </summary>
bool EnableAreaRankSpotLocks = true;

/// <summary>
/// Confgures the amount of AP to be rewarded when clearing an area or dungeon boss
/// in the normal game mode.
/// </summary>
uint AreaBossApReward = 500;

/// <summary>
/// Configures the chance that various gathering tools can break
/// when the player performs a gathering action.
/// </summary>
var ToolBreakChance = new Dictionary<ItemId,double>
{
    [ItemId.Pickaxe] = 0.3,
    [ItemId.EnhancedPickaxe] = 0.2,
    [ItemId.ArtisansPickaxe] = 0.1,
    [ItemId.LumberKnife] = 0.3,
    [ItemId.EnhancedLumberKnife] = 0.2,
    [ItemId.ArtisansLumberKnife] = 0.1,
    [ItemId.Lockpick] = 0.3,
    [ItemId.EnhancedLockpick] = 0.2,
    [ItemId.AllPurposeLockpick] = 0.1,
};;

/// <summary>
/// The maximum number of drop slots in a gather point
/// generated by the default drop generator.
/// </summary>
int DefaultGatherDropMaxSlots = 3;

/// <summary>
/// The maximum number of drops to be generated on a single roll
/// when auto generating gathering drops.
/// </summary>
int MaximumDropsPerDefaultGatherRoll = 3;

/// <summary>
/// Controls how punishing the gathering results are.
/// A high value is more punishing than a lower value.
/// </summary>
double DefaultGatherDropsRandomBias = 2;

/// <summary>
/// If set to true, enables the server to generate gathering drops
/// populated by ddon-tools.
/// </summary>
bool EnableToolGatheringDrops = true;

/// <summary>
/// If set to true, enables the automatically generate gathering drops
/// based on data scraped from wikis.
/// @note Experimental: This feature is still in development and needs
/// more balance and testing before being enabled
/// all the time.
/// </summary>
bool EnableDefaultGatheringDrops = false;

/// <summary>
/// The amount of golden gemstones it costs to use the beauty parlor.
/// </summary>
uint BeautyParlorGGPrice = 5;

/// <summary>
/// The amount of silver tickets it costs to use the beauty parlor.
/// </summary>
uint BeautyParlorSTPrice = 200;

/// <summary>
/// The amount of golden gemstones it costs to use the reincarnation menu.
/// </summary>
uint ReincarnationGGPrice = 5;

/// <summary>
/// Controls the relative weight of drop items to gathering items when generating delivery board quests.
/// Values less than 1 encourage gathering items, values greater than 1 encourage drop items.
/// </summary>
double LightQuestGenerationDropItemWeight = 0.5;

/// <summary>
/// When generating light quests, controls the amount of attempts that will be made to meet restraints on level bounds and uniqueness.
/// </summary>
int LightQuestGenerationAttemptsPerQuest = 20;

/// <summary>
/// The number of times a player can repeat a board quest before it is no longer offered. Resets when quests rotate.
/// </summary>
uint Board = 10000;

/// <summary>
/// 
/// </summary>
string UrlDomain = "http://localhost:{52099}";

/// <summary>
/// Various URLs used by the client.
/// Shared with the login server.
/// </summary>
string UrlManual = "http://localhost:{52099}/manual_nfb/";

/// <summary>
/// 
/// </summary>
string UrlShopDetail = "http://localhost:{52099}/shop/ingame/stone/detail";

/// <summary>
/// 
/// </summary>
string UrlShopCounterA = "http://localhost:{52099}/shop/ingame/counter?";

/// <summary>
/// 
/// </summary>
string UrlShopAttention = "http://localhost:{52099}/shop/ingame/attention?";

/// <summary>
/// 
/// </summary>
string UrlShopStoneLimit = "http://localhost:{52099}/shop/ingame/stone/limit";

/// <summary>
/// 
/// </summary>
string UrlShopCounterB = "http://localhost:{52099}/shop/ingame/counter?";

/// <summary>
/// 
/// </summary>
string UrlChargeCallback = "http://localhost:{52099}/opening/entry/ddo/cog_callback/charge";

/// <summary>
/// 
/// </summary>
string UrlChargeA = "http://localhost:{52099}/sp_ingame/charge/";

/// <summary>
/// 
/// </summary>
string UrlSample9 = "http://sample09.html";

/// <summary>
/// 
/// </summary>
string UrlSample10 = "http://sample10.html";

/// <summary>
/// 
/// </summary>
string UrlCampaignBanner = "http://localhost:{52099}/sp_ingame/campaign/bnr/bnr01.html?";

/// <summary>
/// 
/// </summary>
string UrlSupportIndex = "http://localhost:{52099}/sp_ingame/support/index.html";

/// <summary>
/// 
/// </summary>
string UrlPhotoupAuthorize = "http://localhost:{52099}/api/photoup/authorize";

/// <summary>
/// 
/// </summary>
string UrlApiA = "http://localhost:{52099}/link/api";

/// <summary>
/// 
/// </summary>
string UrlApiB = "http://localhost:{52099}/link/api";

/// <summary>
/// 
/// </summary>
string UrlIndex = "http://localhost:{52099}/sp_ingame/link/index.html";

/// <summary>
/// 
/// </summary>
string UrlCampaign = "http://localhost:{52099}/sp_ingame/campaign/bnr/slide.html";

/// <summary>
/// 
/// </summary>
string UrlChargeB = "http://localhost:{52099}/sp_ingame/charge/";

/// <summary>
/// 
/// </summary>
string UrlCompanionImage = "http://localhost:{52099}/";

/// <summary>
/// How many pawns to consider for random sampling e.g. for clan hall pawns.
/// Specifically this affects how many rows of the DB should be considered for randomization.
/// 0 disables random pawns, which might cause undefined behavior, a minimum of 100 is advised.
/// Avoid very large values like Integer.MAX_VALUE to not degrade performance.
/// </summary>
uint RandomPawnMaxSample = 10000;

/// <summary>
/// The bonus for Job Training kills with a Partner Pawn present.
/// Setting this to 0 effectively disables bonus kills with a Partner Pawn.
/// </summary>
uint JobTrainingPartnerBonus = 1;

/// <summary>
/// Configures the amount BO that that 1 HO will convert to.
/// </summary>
uint HighOrbConversionRate = 100;

/// <summary>
/// Configures if the HO exchange is enabled or not.
/// @warning Current implementation is able to be exploited for infinite conversion.
/// </summary>
bool EnableHighOrbConversion = false;

/// <summary>
/// When set to true, allows pawns to bypass Job Training requirements
/// and learn any skill or augment they otherwise meet the requirements of.
/// </summary>
bool PawnSkipJobTraining = true;

/// <summary>
/// The number of adventure charges that a support pawn has when hired.
/// Other pieces of the UI seemingly expect this to be 10, but it may be more flexible.
/// </summary>
byte RentalPawnAdventureCount = 10;

/// <summary>
/// The number of crafting charges that a support pawn has when hired.
/// Other pieces of the UI seemingly expect this to be 5, but it may be more flexible.
/// </summary>
byte RentalPawnCraftCount = 10;

/// <summary>
/// Time, in seconds, that a support pawn must be adventuring before it loses one of its adventuring charges.
/// By default, 1350 seconds = 22.5 minutes, or 6 hours Lestanian time.
/// </summary>
uint RentalPawnAdventureTimer = 1350;

/// <summary>
/// If true, active rental pawn timers are automatically reset upon returning to a safe area, even if the instance wouldn't normally reset.
/// This is a QOL feature, since removing and readding them to the party would reset the timer anyways.
/// </summary>
bool RentalPawnAdventureTimerAutoReset = true;

/// <summary>
/// If true, rental pawns will consume an adventure charge when starting an EXM, but won't have their usual adventure timer running.
/// </summary>
bool RentalPawnAdventureConsumeOnEXM = true;

/// <summary>
/// The number of rental points (RP), gained by renting and returning pawns, required to buy one JP for a pawn.
/// </summary>
uint RentalPointConversionRate = 10;

/// <summary>
/// The maximum number of effects that can be sealed in BBM using red marks.
/// </summary>
uint DispelSealMax = 80;

/// <summary>
/// The base rate for each seal in BBM, paid using red marks.
/// The first seal costs N, the second seal costs 2N, the third 3N, and so on.
/// </summary>
uint DispelSealCostRate = 2;

/// <summary>
/// The cost of resetting the seals in BBM, paid using red marks.
/// </summary>
uint DispelSealResetRate = 500;

/// <summary>
/// The number of reset tickets given for BBM weekly.
/// </summary>
uint BBMWeeklyResetTickets = 3;

/// <summary>
/// The maximum number of times you can reset BBM using GG, each week.
/// </summary>
uint BBMWeeklyGGResets = 6;

/// <summary>
/// The cost of resetting BBM using GG.
/// </summary>
uint BBMResetGGCost = 1;

/// <summary>
/// Controls how world quests are rolled and refreshed.
/// Both modes cannot be active simultaneously.
/// Valid values:
/// InstanceReset - each party instance rolls quests independently on area entry.
/// ServerReset   - all players share a single server-wide pool that rotates on a weekly schedule (original game behavior).
/// </summary>
WorldQuestSystemMode WorldQuestSystem = WorldQuestSystemMode.ServerReset;

/// <summary>
/// Timezone used for all calendar-aligned task scheduler resets (daily, weekly) and world
/// quest seed computation. Set this to the same value on every shard.
/// 
/// Use the named constants in TimeZoneId - they cover both DST-observing and fixed-offset
/// timezones, so no manual update is ever needed when clocks change:
/// TimeZoneInfo ServerTimeZone = TimeZoneId.Japan;          // Japan (JST) - original game timezone
/// TimeZoneInfo ServerTimeZone = TimeZoneId.CentralEurope;  // Germany, France, Spain, etc. (CET/CEST auto)
/// TimeZoneInfo ServerTimeZone = TimeZoneId.EasternEurope;  // Finland, Greece, Romania, etc. (EET/EEST auto)
/// TimeZoneInfo ServerTimeZone = TimeZoneId.UKIreland;      // UK/Ireland (GMT/BST auto)
/// TimeZoneInfo ServerTimeZone = TimeZoneId.Eastern;        // US/Canada Eastern (EST/EDT auto)
/// TimeZoneInfo ServerTimeZone = TimeZoneId.UTC;            // UTC
/// 
/// For a timezone not listed in TimeZoneId, use FindSystemTimeZoneById with any IANA ID:
/// TimeZoneInfo ServerTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Indiana/Knox");
/// 
/// Full list of IANA timezone IDs:
/// https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
/// 
/// For a fully custom fixed offset with no IANA ID:
/// TimeZoneInfo ServerTimeZone = TimeZoneInfo.CreateCustomTimeZone("custom", TimeSpan.FromHours(5.5), "UTC+5:30", "UTC+5:30");
/// </summary>
TimeZoneInfo ServerTimeZone = TimeZoneId.Japan;

/// <summary>
/// When true, world quests that the party leader does not meet the area rank requirement for
/// are hidden. In InstanceReset mode the slot is re-rolled with an eligible quest. In
/// ServerReset mode the ineligible quest is simply removed without replacement.
/// Applies to both WorldQuestSystem modes.
/// </summary>
bool WorldQuestFilterByLeaderAreaRank = false;

/// <summary>
/// When true, world quests use a first-clear / repeat-clear reward system per period.
/// First clear per period: full rewards (fixed, random, selectable).
/// Repeat clears: reduced random item pool (if defined per quest) plus configurable wallet reward penalties.
/// The WorldQuestResetTask resets first-clear records when it fires, regardless of WorldQuestSystem mode.
/// When false, every clear gives full rewards as if it were a first clear.
/// </summary>
bool WorldQuestFirstClearRewards = true;

/// <summary>
/// EXP reward ratio for repeat world quest clears (0.0 = none, 1.0 = full).
/// Only applies when WorldQuestFirstClearRewards = true.
/// </summary>
double WorldQuestRepeatClearExpPct = 1;

/// <summary>
/// Rift Points reward ratio for repeat world quest clears (0.0 = none, 1.0 = full).
/// Only applies when WorldQuestFirstClearRewards = true.
/// </summary>
double WorldQuestRepeatClearRpPct = 1;

/// <summary>
/// Gold reward ratio for repeat world quest clears (0.0 = none, 1.0 = full).
/// Only applies when WorldQuestFirstClearRewards = true.
/// </summary>
double WorldQuestRepeatClearGoldPct = 1;

/// <summary>
/// Job Points reward ratio for repeat world quest clears (0.0 = none, 1.0 = full).
/// Only applies when WorldQuestFirstClearRewards = true.
/// </summary>
double WorldQuestRepeatClearJpPct = 1;

