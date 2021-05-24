using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaProjectiles {
    public int Shoot(DPoint fromLocation, DPoint toLocation, float speed, int projectileId, int damage = 1, float knockback = 0f, int lifeTimeOverride = -1, int owner = 255) {
      float angle = fromLocation.AngleBetween(toLocation);
      return this.Shoot(fromLocation, angle, speed, projectileId, damage, knockback, lifeTimeOverride, owner);
    }

    public int Shoot(DPoint fromLocation, float angle, float speed, int projectileId, int damage = 1, float knockback = 0f, int lifeTimeOverride = -1, int owner = 255) {
      Vector2 velocity = Vector2.Zero.OffsetPolar(angle, speed);
      return this.Shoot(fromLocation, velocity, projectileId, damage, knockback, lifeTimeOverride, owner);
    }

    public int Shoot(DPoint fromLocation, Vector2 velocity, int projectileId, int damage = 1, float knockback = 0f, int lifeTimeOverride = -1, int owner = 255) {
      int projectileIndex = Projectile.NewProjectile(Projectile.GetNoneSource(), fromLocation.X, fromLocation.Y, velocity.X, velocity.Y, projectileId, damage, knockback, owner);
      if (lifeTimeOverride != -1)
        Main.projectile[projectileIndex].timeLeft = lifeTimeOverride;

      return projectileIndex;
    }
  }
}
