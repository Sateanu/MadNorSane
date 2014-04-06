using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    public class SoundManager
    {
        static Dictionary<string, SoundEffect> sounds;
        public SoundManager(ContentManager content)
        {
            sounds = new Dictionary<string, SoundEffect>();
            sounds.Add("bow_atack",content.Load<SoundEffect>(@"Sounds\bow_atack"));
            sounds.Add("pain1",content.Load<SoundEffect>(@"Sounds\pain1"));
            sounds.Add("pain2", content.Load<SoundEffect>(@"Sounds\pain2"));
            sounds.Add("pain3", content.Load<SoundEffect>(@"Sounds\pain3"));
            sounds.Add("jump",content.Load<SoundEffect>(@"Sounds\jump"));
            sounds.Add("loot", content.Load<SoundEffect>(@"Sounds\loot"));
            sounds.Add("laserShot", content.Load<SoundEffect>(@"Sounds\laserShot"));
            sounds.Add("button-16", content.Load<SoundEffect>(@"Sounds\button-15"));
            sounds.Add("button-15", content.Load<SoundEffect>(@"Sounds\button-16"));
            sounds.Add("crate", content.Load<SoundEffect>(@"Sounds\crate"));
        }
        public static void playSound(string s)
        {
            sounds[s].Play();
        }

        
    }
}
