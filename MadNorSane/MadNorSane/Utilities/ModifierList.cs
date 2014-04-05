using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    
    public class ModifierList
    {
        public List<Modifier> modifiers = new List<Modifier>();
        public List<Modifier> sizemodifiers = new List<Modifier>();
        Random r;
        public ModifierList()
        {
           
            r = new Random((int)DateTime.Now.Ticks);
            modifiers.Add(new Modifier()
                {
                    descriere = "Shukarime",
                    health_points = 2,
                    jump_speed = -5,
                    mana_points = 50,
                    move_speed = 15,
                });
            modifiers.Add(new Modifier()
            {
                descriere = "Valoare",
                health_points = -3,
                jump_speed = -20,
                mana_points = 50,
                move_speed = 25,
                
            });
            modifiers.Add(new Modifier()
            {
                descriere = "Bo$$",
                health_points = 0,
                jump_speed = -5,
                mana_points = 0,
                move_speed = -5,
            });
            for (int i = 0; i < 100; i++)
            {
                float f = -(float)r.NextDouble()+0.5f;

                sizemodifiers.Add(
                    new Modifier()
                    {
                        descriere = "Ai cazut in ceaun cand erai mic",
                        health_points = 0,
                        jump_speed = 0,
                        mana_points = 0,
                        move_speed = 0,
                        arrownr = 0,
                        maxArrows = 0,
                        primaryDamage = 0,
                        secondaryDamage = 0,
                        width = f,
                        height = -f
                    });
            }
        }
        public Modifier getMod()
        {
            return modifiers[r.Next(modifiers.Count())];
        }
        public Modifier getSizeMod()
        {
            return sizemodifiers[r.Next(sizemodifiers.Count())];
        }
    }
}
