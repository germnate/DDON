using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Shop
{
    public class ShopManager : AssetManager<Shared.Model.Shop>
    {
        protected Dictionary<uint, S2CShopGetShopGoodsListRes> Goods;

        public ShopManager(AssetRepository assetRepository, IDatabase database) : base(assetRepository, AssetRepository.ShopKey, database, assetRepository.ShopAsset)
        {
        }

        protected override void OnInit()
        {
            Goods = new Dictionary<uint, S2CShopGetShopGoodsListRes>();
        }

        public override void Load()
        {
            Goods.Clear();
            foreach (Shared.Model.Shop shop in this._assetList)
            {
                Goods.Add(shop.ShopId, shop.Data);
            }
        }

        public S2CShopGetShopGoodsListRes GetAssets(uint ShopId)
        {
            return Goods.GetValueOrDefault(ShopId, new S2CShopGetShopGoodsListRes());
        }

        public bool TryGetItemPrice(uint itemId, out uint price)
        {
            price = this._assetList
                .SelectMany(shop => shop.Data.GoodsParamList)
                .Where(good => good.ItemId == itemId)
                .Select(good => good.Price)
                .DefaultIfEmpty()
                .Min();

            return price > 0;
        }

        // Most equipment (drops, quest rewards, etc.) is never sold in an NPC shop,
        // so TryGetItemPrice() has nothing to look up for it. This estimates what
        // that gear would reasonably cost to buy, based on Level (biggest factor),
        // then rarity (CrestSlots), then Quality, so ItemSellItemHandler always has
        // a sensible value to derive a sell price from instead of silently falling
        // back to the (usually 0) ClientItemInfo.Price.
        private const double EquipmentLevelValueScale = 40.0;
        private const double EquipmentRarityStepValue = 0.15;
        private const double EquipmentQualityStepValue = 0.05;

        public uint EstimateEquipmentBuyPrice(ClientItemInfo itemInfo)
        {
            byte level = itemInfo.Level ?? 1;
            byte crestSlots = Math.Min(itemInfo.CrestSlots ?? 0, (byte)4);
            byte quality = Math.Min(itemInfo.Quality ?? 0, (byte)4);

            double baseValue = level * level * EquipmentLevelValueScale;
            double rarityMultiplier = 1.0 + (crestSlots * EquipmentRarityStepValue);
            double qualityMultiplier = 1.0 + (quality * EquipmentQualityStepValue);

            return (uint) Math.Max(1, Math.Round(baseValue * rarityMultiplier * qualityMultiplier));
        }
    }
}