using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Animations
{
    public class AnimationController
    {
        public List<Animation>  AnimationsList { get; private set; }
        public Animation        Animation { get; private set; }

        public AnimationController(string p_animatorType)
        {
            AnimationsList = new List<Animation>();
            switch (p_animatorType)
            {
                case "Fire":
                    AddTwoFrameAnimation("Fire", TilesetManager.ItemTileSet.FIRE1, 0.5f);
                    break;
                case "Triforce":
                    AddTwoFrameAnimation("Triforce", TilesetManager.ItemTileSet.TRIFORCE1, 0.5f);
                    break;
                case "Heart":
                    AddTwoFrameAnimation("Heart", TilesetManager.ItemTileSet.FIRE1);
                    break;
                case "Rupee":
                    AddTwoFrameAnimation("Rupee", TilesetManager.ItemTileSet.RUPEE1);
                    break;
                case "OctorokRed":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.OCTOROK_RED_R1);
                    break;
                case "OctorokBlue":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.OCTOROK_BLUE_R1);
                    break;
                case "Goriya":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.GORIYA_R1);
                    break;
                case "Stalfos":
                    AddTwoFrameAnimation("Stalfos", TilesetManager.EnemyTileSet.STALFOS_1, 0.15f);
                    break;
                case "Keese":
                    AddTwoFrameAnimation("Keese", TilesetManager.EnemyTileSet.KEESE_1);
                    break;
                case "Zora":
                    AddTwoFrameAnimation("Underwater", TilesetManager.EnemyTileSet.ZORA_UNDERWATER_1);
                    AddOneFrameAnimation("Front", TilesetManager.EnemyTileSet.ZORA_FRONT);
                    AddOneFrameAnimation("Back", TilesetManager.EnemyTileSet.ZORA_BACK);
                    break;
                case "LeeverRed":
                    AddTwoFrameAnimation("Underground", TilesetManager.EnemyTileSet.LEEVER_UNDERGROUND_1);
                    AddOneFrameAnimation("Emerging", TilesetManager.EnemyTileSet.LEEVER_RED_EMERGING);
                    AddTwoFrameAnimation("Leever", TilesetManager.EnemyTileSet.LEEVER_RED_1);
                    break;
                case "LeeverBlue":
                    AddTwoFrameAnimation("Underground", TilesetManager.EnemyTileSet.LEEVER_UNDERGROUND_1);
                    AddOneFrameAnimation("Emerging", TilesetManager.EnemyTileSet.LEEVER_BLUE_EMERGING);
                    AddTwoFrameAnimation("Leever", TilesetManager.EnemyTileSet.LEEVER_BLUE_1);
                    break;
                case "Wallmaster":
                    AddTwoFrameAnimation("Left", TilesetManager.EnemyTileSet.WALLMASTER_L1);
                    AddTwoFrameAnimation("Right", TilesetManager.EnemyTileSet.WALLMASTER_R1);
                    break;
                case "Gel":
                    AddTwoFrameAnimation("Gel", TilesetManager.EnemyTileSet.GEL_1);
                    break;
                case "BladeTrap":
                    AddOneFrameAnimation("Blade", TilesetManager.EnemyTileSet.BLADETRAP_1);
                    break;
                case "Aquamentus":
                    AddTwoFrameAnimation("Idle", TilesetManager.EnemyTileSet.AQUAMENTUS_IDLE_1);
                    AddTwoFrameAnimation("Attack", TilesetManager.EnemyTileSet.AQUAMENTUS_ATTACK_1);
                    break;

            }
            Animation = AnimationsList[0];
        }
        private void AddOneFrameAnimation(string p_animName, TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
        }
        private void AddTwoFrameAnimation(string p_animName, TilesetManager.ItemTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
        }
        private void AddTwoFrameAnimation(string p_animName, TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
        }
        private void AddAllDirectionAnimation(TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation("Right"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
            AnimationsList.Add(new Animation("Up"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 2, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 3, p_frameDuration));
            AnimationsList.Add(new Animation("Left"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 4, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 5, p_frameDuration));
            AnimationsList.Add(new Animation("Down"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 6, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 7, p_frameDuration));
        }
        
        public void ChangeAnimation(string p_animationName)
        {
            foreach(Animation __anim in AnimationsList)
                if (__anim.name == p_animationName)
                {
                    Animation = __anim;
                    Animation.ResetAnimation();
                }
        }
        public void UpdateAnimationController(float p_deltaTime)
        {
            if (Animation == null)
                Animation = AnimationsList[0];
            Animation.UpdateAnimation(p_deltaTime);
        }

        public void DrawFrame(SpriteBatch p_spriteBatch, Rectangle p_pos, Color p_color)
        {
            if (Animation == null || Animation.Frame == null)
                return;
            p_spriteBatch.Draw(TilesetManager.GetTileSet(Animation.Frame.frameType), p_pos, 
                TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), p_color);
        }
        public void DrawFrame(SpriteBatch p_spriteBatch, Rectangle p_pos)
        {
            if (Animation == null || Animation.Frame == null)
                return;
            p_spriteBatch.Draw(TilesetManager.GetTileSet(Animation.Frame.frameType), p_pos, 
                TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), Color.White);
        }
    }
}
