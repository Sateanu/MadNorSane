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
        public float reload_time = 1;
        internal void set(Modifier mod)
        {
            if(move_speed!=0)
            move_speed = mod.move_speed;
            if(jump_speed!=0)
            jump_speed = mod.jump_speed;
            if(health_points!=0)
            health_points = mod.health_points;
            if(mana_points!=0)
            mana_points = mod.mana_points;
            if(maxArrows!=0)
            maxArrows = mod.maxArrows;
            if(maxArrows!=0)
            arrownr = maxArrows;
            if(primaryDamage!=0)
            primaryDamage = mod.primaryDamage;
            if(projectile_speed!=0)
            projectile_speed = mod.projectile_speed;
            if(reload_time!=0)
            reload_time = mod.reload_time;
            original_mana_points = mana_points;
            original_health_points = health_points;
            original_arrow_nr = arrownr;
        }
        internal void apply(Modifier mod)
        {
            move_speed += mod.move_speed;
            if (move_speed <= 5) move_speed = 5;

            jump_speed += mod.jump_speed;
            if (jump_speed >= -10) jump_speed = -10;

            health_points += mod.health_points;
            if (health_points <= 0) health_points = 1;

            mana_points += mod.mana_points;
            if (mana_points <= 0) mana_points = 1;

            maxArrows += mod.maxArrows;
            if (maxArrows <= 0) maxArrows = 1;
            arrownr = maxArrows;

            primaryDamage += mod.primaryDamage;
            if (primaryDamage <= 0) primaryDamage = 1;

            
            width += mod.width;
            if (width <= 0.2f) width = 0.2f;

            
            height += mod.height;
            if (height <= 0.2f) height = 0.2f;

            projectile_speed += mod.projectile_speed;
            if (projectile_speed <= 5) projectile_speed = 5;

            


            reload_time += mod.reload_time;

            if (reload_time <= 0)
                reload_time = 0.5f;
            original_mana_points = mana_points;
            original_health_points = health_points;
            original_arrow_nr = arrownr;
        }
    }
}
