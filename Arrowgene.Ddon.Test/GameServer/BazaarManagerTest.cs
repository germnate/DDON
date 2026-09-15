using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Test.Database;
using System.Linq;
using Xunit;

namespace Arrowgene.Ddon.GameServer;

public class BazaarManagerTest
{
    private static readonly ushort[] ExpectedGeneratedQuantities = [1, 3, 5, 10];

    [Fact]
    public void ServerBazaarCharacter_ShouldMatchDatabaseSeed()
    {
        Assert.Equal(40u, Character.ServerCharacterId);
        Assert.Equal("Server", Character.ServerCharacterFirstName);
    }

    [Fact]
    public void BuildGeneratedExhibitions_ShouldCreateAboutFourHundredRankedListings()
    {
        var settings = new GameServerSetting();
        var scriptableSettings = new ScriptableSettings();
        scriptableSettings.Set<uint>("GameLogicSettings", "GameClockTimescale", 90);
        scriptableSettings.Set<uint>("GameLogicSettings", "WeatherSequenceLength", 20);
        scriptableSettings.Set("GameLogicSettings", "WeatherStatistics", new System.Collections.Generic.List<(uint MeanLength, uint Weight)>
        {
            (60 * 30, 1),
            (60 * 30, 1),
            (60 * 30, 1),
        });
        scriptableSettings.Set("GameLogicSettings", "WalletLimits", new System.Collections.Generic.Dictionary<WalletType, uint>
        {
            { WalletType.Gold, 999999999 },
            { WalletType.RiftPoints, 999999999 },
            { WalletType.BloodOrbs, 500000 },
            { WalletType.SilverTickets, 999999999 },
            { WalletType.GoldenGemstones, 99999 },
            { WalletType.RentalPoints, 99999 },
            { WalletType.ResetJobPoints, 99 },
            { WalletType.ResetCraftSkills, 99 },
            { WalletType.HighOrbs, 5000 },
            { WalletType.DominionPoints, 999999999 },
            { WalletType.AdventurePassPoints, 80 },
            { WalletType.CustomMadeServiceTickets, 999999999 },
            { WalletType.BitterblackMazeResetTicket, 3 },
            { WalletType.GoldenDragonMark, 30 },
            { WalletType.SilverDragonMark, 150 },
            { WalletType.RedDragonMark, 99999 },
        });

        var assets = new AssetRepository("Files/Assets");
        assets.Initialize();
        var gameLogicSetting = new GameSettings(scriptableSettings);
        var server = new DdonGameServer(settings, gameLogicSetting, new MockDatabase(), assets);

        var listings = server.BazaarManager.BuildGeneratedExhibitions();
        var listingsByItemId = listings.GroupBy(listing => listing.Info.ItemInfo.ItemBaseInfo.ItemId).ToList();

        Assert.InRange(listingsByItemId.Count, 350, 400);
        Assert.Equal(listingsByItemId.Count * ExpectedGeneratedQuantities.Length, listings.Count);
        Assert.All(listings, listing => Assert.Equal(Character.ServerCharacterId, listing.CharacterId));
        Assert.All(listings, listing =>
        {
            var itemInfo = assets.ClientItemInfos[listing.Info.ItemInfo.ItemBaseInfo.ItemId];
            Assert.Equal(2, itemInfo.Category);
            Assert.InRange(itemInfo.SubCategory, ItemSubCategory.MaterialInorganicMetal, ItemSubCategory.MaterialPawnInspiration);
            Assert.Contains(listing.Info.ItemInfo.ItemBaseInfo.Num, ExpectedGeneratedQuantities);
        });
        Assert.All(listingsByItemId, group =>
            Assert.Equal(ExpectedGeneratedQuantities, group.Select(listing => listing.Info.ItemInfo.ItemBaseInfo.Num).Order()));
        Assert.True(listingsByItemId.GroupBy(group => assets.ClientItemInfos[group.Key].Rank)
            .All(group => group.Count() <= 50));
    }
}
