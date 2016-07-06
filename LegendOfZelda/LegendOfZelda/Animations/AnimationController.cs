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
                    AnimationsList.Add(new Animation("Fire"));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.FIRE1, 0.5f));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.FIRE2, 0.5f));
                    break;
                case "Triforce":
                    AnimationsList.Add(new Animation("Triforce"));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.TRIFORCE1, 0.25f));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.TRIFORCE2, 0.25f));
                    break;
                case "Heart":
                    AnimationsList.Add(new Animation("Heart"));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.HEART1, 0.25f));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.HEART2, 0.25f));
                    break;
                case "Rupee":
                    AnimationsList.Add(new Animation("Rupee"));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.RUPEE1, 0.25f));
                    AnimationsList[0].FramesList.Add(new AnimationFrame(TilesetManager.ItemTileSet.RUPEE2, 0.25f));
                    break;
            }
            Animation = AnimationsList[0];
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
            p_spriteBatch.Draw(TilesetManager.itemsTileset, p_pos, 
                TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), p_color);
        }
        public void DrawFrame(SpriteBatch p_spriteBatch, Rectangle p_pos)
        {
            if (Animation == null || Animation.Frame == null)
                return;

            p_spriteBatch.Draw(TilesetManager.itemsTileset, p_pos, 
                TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), Color.White);
        }
    }
}
