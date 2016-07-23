using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LegendOfZelda
{
    public class SoundManager
    {
        private SoundManager() { }
        private static SoundManager s_instance = null;
        public static SoundManager GetInstance()
        {
            if (s_instance == null)
            {
                s_instance = new SoundManager();

            }
            return s_instance;
        }

        public bool BGMusicVolume = true;
        public bool SFXVolume = true;

        SoundEffect _bgMusic;
        SoundEffectInstance _bgMusicInstance;
        SoundEffect _buttonSFX;
        SoundEffectInstance _buttonSFXInstance;
        SoundEffect _backButtonSFX;
        SoundEffectInstance _backButtonSFXInstance;
        SoundEffect _matchEndSFX;
        SoundEffectInstance _matchEndSFXInstance;

        public void LoadContent(Main p_gameManager)
        {
            _bgMusic = p_gameManager.Content.Load<SoundEffect>("Sounds/BGMusic_00");
            _buttonSFX = p_gameManager.Content.Load<SoundEffect>("Sounds/SFX_00");
            _backButtonSFX = p_gameManager.Content.Load<SoundEffect>("Sounds/SFX_01");
            _matchEndSFX = p_gameManager.Content.Load<SoundEffect>("Sounds/SFX_02");
        }
        public void Initialize()
        {
            _bgMusicInstance = _bgMusic.CreateInstance();
            _bgMusicInstance.Pan = 0.0f;
            _bgMusicInstance.Volume = 1.0f;
            _bgMusicInstance.Pitch = 0.0f;
            _bgMusicInstance.IsLooped = true;
            _bgMusicInstance.Play();
            _buttonSFXInstance = _buttonSFX.CreateInstance();
            _backButtonSFXInstance = _backButtonSFX.CreateInstance();
            _matchEndSFXInstance = _matchEndSFX.CreateInstance();
        }

        public void PlayButtonSFX()
        {
            _buttonSFX.Play();
        }
        public void PlayBackButtonSFX()
        {
            _backButtonSFX.Play();
        }
        public void PlayMatchEndSFX()
        {
            _matchEndSFX.Play();
        }
    }
   

}
