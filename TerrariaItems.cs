using System;
using System.Collections.Generic;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaItems {
    #region [Methods: IsValidItemType, IsCraftableItem, IsEquipableItem, GetItemName]
    public bool IsValidItemType(int itemType) {
      return (itemType >= Terraria.ItemType_Min && itemType <= Terraria.ItemType_Max);
    }

    public bool IsCraftableItem(ItemType itemType) {
      return !(
        itemType == ItemType.DirtBlock ||
        itemType == ItemType.StoneBlock ||
        itemType == ItemType.Mushroom ||
        itemType == ItemType.Wood || 
        (itemType >= ItemType.IronOre && itemType <= ItemType.SilverOre) ||
        itemType == ItemType.Gel ||
        itemType == ItemType.Acorn ||
        itemType == ItemType.LifeCrystal ||
        itemType == ItemType.Lens ||
        itemType == ItemType.Shuriken || 
        (itemType >= ItemType.BandofRegeneration && itemType <= ItemType.DemoniteOre) || 
        (itemType >= ItemType.Heart && itemType <= ItemType.Starfury) ||
        itemType == ItemType.RottenChunk ||
        itemType == ItemType.WormTooth ||
        itemType == ItemType.FallenStar ||
        itemType == ItemType.ShadowScale ||
        itemType == ItemType.PiggyBank ||
        itemType == ItemType.MiningHelmet || 
        (itemType >= ItemType.FlintlockPistol && itemType <= ItemType.Minishark) || 
        (itemType >= ItemType.BandofStarpower && itemType <= ItemType.Meteorite) ||
        itemType == ItemType.Hook ||
        itemType == ItemType.RocketBoots || 
        (itemType >= ItemType.ClayBlock && itemType <= ItemType.PinkBrickWall) || 
        (itemType >= ItemType.Spike && itemType <= ItemType.Cobweb) || 
        (itemType >= ItemType.Bone && itemType <= ItemType.SandBlock) || 
        (itemType >= ItemType.AshBlock && itemType <= ItemType.Hellstone) || 
        (itemType >= ItemType.MudBlock && itemType <= ItemType.Star) ||
        itemType == ItemType.BreathingReed ||
        itemType == ItemType.Flipper ||
        itemType == ItemType.MushroomGrassSeeds ||
        itemType == ItemType.JungleGrassSeeds || 
        (itemType >= ItemType.JungleRose && itemType <= ItemType.Shackle) ||
        itemType == ItemType.Hellforge ||
        itemType == ItemType.NaturesGift ||
        itemType == ItemType.BlackLens ||
        itemType == ItemType.WizardHat ||
        itemType == ItemType.TopHat || 
        (itemType >= ItemType.SummerHat && itemType <= ItemType.PlumbersHat) ||
        itemType == ItemType.ArchaeologistsHat ||
        itemType == ItemType.BlackDye || 
        (itemType >= ItemType.NinjaHood && itemType <= ItemType.NinjaPants) ||
        itemType == ItemType.RedHat ||
        itemType == ItemType.Goldfish ||
        itemType == ItemType.RobotHat || 
        (itemType >= ItemType.GuideVoodooDoll && itemType <= ItemType.DemonScythe) || 
        (itemType >= ItemType.Coral && itemType <= ItemType.Aglet) || 
        (itemType >= ItemType.GoldChest && itemType <= ItemType.ShadowKey) ||
        itemType == ItemType.JungleSpores ||
        itemType == ItemType.Safe ||
        itemType == ItemType.TatteredCloth || 
        (itemType >= ItemType.CobaltOre && itemType <= ItemType.Pwnhammer) ||
        itemType == ItemType.HallowedSeeds ||
        itemType == ItemType.EbonsandBlock ||
        itemType == ItemType.Compass ||
        itemType == ItemType.TinkerersWorkshop || 
        (itemType >= ItemType.Toolbelt && itemType <= ItemType.MiningPants) ||
        itemType == ItemType.SiltBlock ||
        itemType == ItemType.BreakerBlade ||
        itemType == ItemType.ClockworkAssaultRifle || 
        (itemType >= ItemType.DualHook && itemType <= ItemType.PiranhaStatue) || 
        (itemType >= ItemType.MoonCharm && itemType <= ItemType.RangerEmblem) || 
        (itemType >= ItemType.PixieDust && itemType <= ItemType.ClownPants) || 
        (itemType >= ItemType.Bell && itemType <= ItemType.WireCutter) ||
        itemType == ItemType.Lever ||
        itemType == ItemType.LaserRifle ||
        itemType == ItemType.MagicDagger || 
        (itemType >= ItemType.SoulofLight && itemType <= ItemType.CursedFlame) || 
        (itemType >= ItemType.UnicornHorn && itemType <= ItemType.StarCloak) || 
        (itemType >= ItemType.Shotgun && itemType <= ItemType.TitanGlove) || 
        itemType == ItemType.Switch ||
        itemType == ItemType.DartTrap || (
        itemType >= ItemType.GreenPressurePlate && itemType <= ItemType.BrownPressurePlate) || 
        (itemType >= ItemType.SoulofFright && itemType <= ItemType.SoulofSight) ||
        itemType == ItemType.CrossNecklace || 
        (itemType >= ItemType.MusicBox_OverworldDay && itemType <= ItemType.MusicBox) || 
        itemType == ItemType.CandyCaneBlock || 
        (itemType >= ItemType.SantaHat && itemType <= ItemType.GreenCandyCaneBlock) ||
        itemType == ItemType.SnowBlock || 
        (itemType >= ItemType.BlueLight && itemType <= ItemType.Carrot)
      );
    }

    public bool IsEquipableItem(ItemType itemType) {
      return (
        (itemType >= ItemType.CopperWatch && itemType <= ItemType.DepthMeter) ||
        itemType == ItemType.Goggles ||
        itemType == ItemType.BandofRegeneration ||
        itemType == ItemType.CloudinaBottle ||
        itemType == ItemType.HermesBoots ||
        (itemType >= ItemType.CopperGreaves && itemType <= ItemType.GoldChainmail) ||
        (itemType >= ItemType.MiningHelmet && itemType <= ItemType.GoldHelmet) ||
        (itemType >= ItemType.ShadowGreaves && itemType <= ItemType.ShadowHelmet) ||
        itemType == ItemType.BandofStarpower ||
        (itemType >= ItemType.MeteorHelmet && itemType <= ItemType.MeteorLeggings) ||
        itemType == ItemType.RocketBoots ||
        (itemType >= ItemType.NecroHelmet && itemType <= ItemType.NecroGreaves) ||
        itemType == ItemType.LuckyHorseshoe ||
        itemType == ItemType.ShinyRedBalloon ||
        itemType == ItemType.Flipper ||
        itemType == ItemType.ObsidianSkull ||
        itemType == ItemType.JungleRose ||
        itemType == ItemType.FeralClaws ||
        itemType == ItemType.AnkletoftheWind ||
        itemType == ItemType.Shackle ||
        itemType == ItemType.NaturesGift ||
        (itemType >= ItemType.JungleHat && itemType <= ItemType.MoltenGreaves) ||
        itemType == ItemType.Sunglasses ||
        (itemType >= ItemType.WizardHat && itemType <= ItemType.ArchaeologistsPants) ||
        (itemType >= ItemType.NinjaHood && itemType <= ItemType.NinjaPants) ||
        itemType == ItemType.RedHat ||
        (itemType >= ItemType.Robe && itemType <= ItemType.GoldCrown) ||
        (itemType >= ItemType.GuideVoodooDoll && itemType <= ItemType.FamiliarWig) ||
        itemType == ItemType.Aglet ||
        itemType == ItemType.MimeMask ||
        itemType == ItemType.TheDoctorsShirt ||
        itemType == ItemType.TheDoctorsPants ||
        (itemType >= ItemType.CobaltHat && itemType <= ItemType.MythrilGreaves) ||
        (itemType >= ItemType.Compass && itemType <= ItemType.ObsidianShield) ||
        (itemType >= ItemType.CloudinaBalloon && itemType <= ItemType.SpectreBoots) ||
        itemType == ItemType.Toolbelt ||
        itemType == ItemType.MiningShirt ||
        itemType == ItemType.MiningPants ||
        itemType == ItemType.MoonCharm ||
        itemType == ItemType.Ruler ||
        (itemType >= ItemType.SorcererEmblem && itemType <= ItemType.AngelWings) ||
        itemType == ItemType.NeptunesShell ||
        (itemType >= ItemType.ClownHat && itemType <= ItemType.ClownPants) ||
        itemType == ItemType.StarCloak ||
        itemType == ItemType.PhilosophersStone ||
        itemType == ItemType.TitanGlove ||
        (itemType >= ItemType.HallowedPlateMail && itemType <= ItemType.ManaFlower) ||
        itemType == ItemType.HallowedHeadgear ||
        itemType == ItemType.HallowedMask ||
        (itemType >= ItemType.MusicBox_OverworldDay && itemType <= ItemType.MusicBox_Boss3) ||
        itemType == ItemType.MusicBox ||
        (itemType >= ItemType.SantaHat && itemType <= ItemType.SantaPants)
      );
    }

    public string GetItemName(ItemType itemType) {
      return Main.itemName[(int)itemType];
    }
    #endregion

    #region [Method: CreateNew]
    public void CreateNew(TSPlayer forPlayer, DPoint location, ItemMetadata itemData) {
      int itemIndex = Item.NewItem(
        location.X, location.Y, 16, 16, (int)itemData.Type, itemData.StackSize, true, (int)itemData.Prefix
      );

      forPlayer.SendData(PacketTypes.ItemDrop, string.Empty, itemIndex);
    }
    #endregion

    #region [Methods: EnumerateItemsInRect, EnumerateItemsAroundPoint]
    public IEnumerable<Item> EnumerateItemsInRect(Rectangle rect) {
      int areaL = rect.Left - (rect.Width / 2);
      int areaT = rect.Top - (rect.Height / 2);
      int areaR = rect.Left + (rect.Width / 2);
      int areaB = rect.Top + (rect.Height / 2);

      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (
          item.position.X > areaL && item.position.X < areaR &&
          item.position.Y > areaT && item.position.Y < areaB
        ) {
          yield return item;
        }
      }
    }

    public IEnumerable<Item> EnumerateItemsAroundPoint(DPoint location, int radius) {
      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (Math.Sqrt(Math.Pow(item.position.X - location.X, 2) + Math.Pow(item.position.Y - location.Y, 2)) <= radius)
          yield return item;
      }
    }
    #endregion
  }
}
