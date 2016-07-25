using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace LegendOfZelda
{
    public enum SoundType
    {
        OVERWORLD,
        DUNGEON,
        SWORD,
        SWORD_COMBINED,
        SWORD_PROJECTILE,
        BOOMERANG,
        PLAYER_DEATH,
        PLAYER_HITTED,
        ENEMY_KILL,
        ENEMY_HITTED,
        BOSS_SCREAM_1,
        BOSS_SCREAM_2,
        GET_ITEM,
        GET_HEART,
        GET_RUPEE,
        OPEN_DOOR
    }

    public class SoundManager
    {
        private static SoundManager s_instance = null;

        public static SoundManager instance => s_instance ?? (s_instance = new SoundManager());

        private SoundManager()
        {
            _playing = new Dictionary<SoundType, SoundEffectInstance>();
        }

        private Dictionary<SoundType, SoundEffect> _sounds;
        private Dictionary<SoundType, SoundEffectInstance> _playing;

        public void LoadContent()
        {
            _sounds = new Dictionary<SoundType, SoundEffect>
            {
                { SoundType.OVERWORLD, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Overworld") },
                { SoundType.SWORD, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Sword") },
                { SoundType.SWORD_PROJECTILE, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Sword_Shoot") },
                { SoundType.SWORD_COMBINED, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Sword_Combined") },
                { SoundType.BOOMERANG, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Boomerang") },
                { SoundType.PLAYER_HITTED, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Hurt") },
                { SoundType.PLAYER_DEATH, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Die") },
                { SoundType.ENEMY_HITTED, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Hit") },
                { SoundType.ENEMY_KILL, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Kill") },
                { SoundType.DUNGEON, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Dungeon") },
                { SoundType.BOSS_SCREAM_1, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Boss_Scream1") },
                { SoundType.BOSS_SCREAM_2, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Boss_Scream2") },
                { SoundType.GET_ITEM, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Get_Item") },
                { SoundType.GET_HEART, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Get_Heart") },
                { SoundType.GET_RUPEE, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Get_Rupee") },
                { SoundType.OPEN_DOOR, Main.s_game.Content.Load<SoundEffect>("Sounds/LOZ_Unlock") }

            };
        }

        public void Play(SoundType p_soundType, bool p_repeat = false, float p_volume = 1.0f)
        {
            if (_playing.ContainsKey(p_soundType))
            {
                var __playingSound = _playing[p_soundType];

                if (__playingSound.State == SoundState.Stopped)
                {
                    __playingSound.Dispose();
                    _playing.Remove(p_soundType);
                }
                else
                {
                    return;
                }
            }

            var __soundEffect = _sounds[p_soundType];

            var __soundInstance = __soundEffect.CreateInstance();

            __soundInstance.IsLooped = p_repeat;
            __soundInstance.Volume = p_volume;
            __soundInstance.Play();

            _playing[p_soundType] = __soundInstance;
        }

        public void StopAndDispose(SoundType p_soundType)
        {
            if (_playing.ContainsKey(p_soundType))
            {
                var __playingSound = _playing[p_soundType];

                __playingSound.Stop();
                __playingSound.Dispose();
                _playing.Remove(p_soundType);
            }
        }
    }
}