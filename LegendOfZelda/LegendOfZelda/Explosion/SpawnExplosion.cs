﻿using LegendOfZelda.Util;
using LegendOfZelda.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class SpawnExplosion : Entity
    {
        public SpawnExplosion(Vector2 p_pos)
        {
            type = EntityType.EFFECTS;
            tag = "SpawnExplosion";
            position = new Vector2(p_pos.X, p_pos.Y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            _animationController = new AnimationController("SpawnExplosion");
            _animationController.Animation.OnAnimationEnd += DestroyEntity;
            hitboxSize = new Vector2(8f, 12f);
            hitboxOffset = new Vector2(4f, 2f);
            UpdateAABB();
        }

        private void Animation_OnAnimationEnd()
        {
            DestroyEntity();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _animationController.DrawFrame(spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            base.DebugDraw(p_spriteBatch);
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(hitbox, parentPosition), Color.Yellow, 3f);
        }
    }
}
