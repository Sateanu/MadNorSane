using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    class Skill
    {
        public String name = "";
        private float cool_down = 0;
        public float time_until_reuse = 0;
        public float time_to_finish = 0;
        public int used_mp = 0;
        public int damage = 0;
        public int protection = 0;

        private long began_time = 0;

        public Skill(int _damage, int _protection, int _time_to_finish, int _cool_down)
        {
            damage = _damage;
            protection = _protection;
            time_to_finish = _time_to_finish;
            cool_down = _cool_down;
        }
        public Skill(int _damage, int _protection, int _time_to_finish, int _cool_down, int _used_mp)
        {
            damage = _damage;
            protection = _protection;
            time_to_finish = _time_to_finish;
            cool_down = _cool_down;
            used_mp = _used_mp;
        }

        public void use_skill(GameTime _game_time)
        {
            if(time_until_reuse == 0)
            {
                time_until_reuse = cool_down;
                began_time = _game_time.TotalGameTime.Seconds;
            }
        }

        public void update_skill_cool_down(GameTime _game_time)
        {
            if(time_until_reuse > 0)
            {
                long passed_time = _game_time.TotalGameTime.Seconds - began_time;
                time_until_reuse = cool_down - passed_time;
                Console.WriteLine("time until skill is reusable: " + time_until_reuse + "; " + _game_time.TotalGameTime.Seconds + " " + began_time);
            }
        }
    }
}
