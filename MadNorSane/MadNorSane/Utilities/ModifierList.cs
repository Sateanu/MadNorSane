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
                    descriere = "HP+++\nJUMP++\nMANA/ARROWS++\nMOVEMENT--\nDAMAGE++",
                    health_points = 3,
                    jump_speed = -1.5f,
                    mana_points = 1,
                    maxArrows=1,
                    move_speed = -1.5f,
                    primaryDamage=-1,
                    projectile_speed=10,
                });
            modifiers.Add(new Modifier()
            {
                descriere = "HP-\nJUMP++\nMANA/ARROWS+\nMOVEMENT++\n",
                health_points = -1,
                jump_speed = -1.5f,
                mana_points = 2,
                maxArrows=2,
                move_speed = 1.5f,
                primaryDamage=0,
                projectile_speed = 10,
            });
            modifiers.Add(new Modifier()
            {
                descriere = "JUMP+++\nMOVEMENT---",
                health_points = 0,
                jump_speed = -5,
                mana_points = 0,
                maxArrows=0,
                move_speed = -5,
                primaryDamage=0,
                projectile_speed = 10,
            });
            modifiers.Add(new Modifier()
            {
                descriere = "HP+++\nJUMP--\nMOVEMENT++\nDAMAGE--",
                health_points = 2,
                jump_speed = +1.5f,
                mana_points = 0,
                maxArrows = 0,
                move_speed = 3,
                primaryDamage=-1,
                projectile_speed = 10,
            });
            modifiers.Add(new Modifier()
            {
                descriere = "MANA/ARROWS++\nMOVEMENT++\nDAMAGE++",
                health_points = 0,
                jump_speed = 0,
                mana_points = 2,
                maxArrows = 2,
                move_speed = 3,
                primaryDamage = 1,
                projectile_speed = 10,
            });
            for (int i = 0; i < 50; i++)
            {
                float f = (float)r.NextDouble();

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
                        height = 0,
                        projectile_speed=0,
                    });
                sizemodifiers.Add(
                    new Modifier()
                    {
                        descriere = "Michael Jordan V2",
                        health_points = 0,
                        jump_speed = 0,
                        mana_points = 0,
                        move_speed = 0,
                        arrownr = 0,
                        maxArrows = 0,
                        primaryDamage = 0,
                        secondaryDamage = 0,
                        width = 0,
                        height = f,
                        projectile_speed = 0,
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
