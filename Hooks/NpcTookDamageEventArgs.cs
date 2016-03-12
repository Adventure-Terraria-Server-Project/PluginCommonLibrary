using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class NpcTookDamageEventArgs: NetHookEventArgs {
    public int NpcIndex { get; }
    public int Damage { get; }
    public float Knockback { get; }
    public int HitDirection { get; }
    public bool IsCritical { get; }

    public NpcTookDamageEventArgs(TSPlayer player, int npcIndex, int damage, float knockback, int hitDirection, bool isCrticial): base(player) {
      this.NpcIndex = npcIndex;
      this.Damage = damage;
      this.Knockback = knockback;
      this.HitDirection = hitDirection;
      this.IsCritical = isCrticial;
    }
  }
}
