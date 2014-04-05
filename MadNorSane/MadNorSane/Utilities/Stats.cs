using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    public class Stats
    {
        public float original_mana_points = 10;
        public float move_speed = 15, jump_speed = -20, health_points = 10, mana_points = 10;
        public int maxArrows = 4;
        public int arrownr = 4;
        public int primaryDamage = 2;
        public int secondaryDamage = 3;
        public float width;
        public float height;

        internal void apply(Modifier mod)
        {
            move_speed += mod.move_speed;
            jump_speed += mod.jump_speed;
            health_points += mod.health_points;
            mana_points += mod.mana_points;
            original_mana_points += mod.mana_points;
            maxArrows += mod.maxArrows;
            arrownr = maxArrows;
            primaryDamage += mod.primaryDamage;
            secondaryDamage += mod.secondaryDamage;
            width += mod.width;
            height += mod.height;
        }
    }
}
