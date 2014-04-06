using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    public class Stats
    {
        public float original_mana_points = 10;
        public float move_speed = 15, jump_speed = -23, health_points = 10, mana_points = 10;
        public float projectile_speed = 15;
        public float original_health_points = 10;
        public float original_arrow_nr = 5;

        public int maxArrows = 4;
        public int arrownr = 4;
        public int primaryDamage = 2;
        public int secondaryDamage = 2;
        public float width;
        public float height;
        public int reload_time = 1;

        internal void apply(Modifier mod)
        {
            move_speed += mod.move_speed;
            jump_speed += mod.jump_speed;
            health_points += mod.health_points;
            mana_points += mod.mana_points;
            maxArrows += mod.maxArrows;
            arrownr = maxArrows;
            primaryDamage += mod.primaryDamage;
            secondaryDamage += mod.secondaryDamage;
            width += mod.width;
            height += mod.height;
            projectile_speed += mod.projectile_speed;

            original_mana_points = mana_points;
            original_health_points = health_points;
            original_arrow_nr = arrownr;
        }
    }
}
