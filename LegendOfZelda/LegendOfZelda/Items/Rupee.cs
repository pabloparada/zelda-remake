﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;

namespace LegendOfZelda.Items
{
    public class Rupee : Item
    {
        public Rupee(Object p_obj)
        {
            tag = "Rupee";
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            animationController = new AnimationController("Rupee");
        }
        public override void Update(float delta, Collider p_collider)
        {
            base.Update(delta, p_collider);
            
        }
        public override void Draw(SpriteBatch p_spriteBatch)
        {
            base.Draw(p_spriteBatch);
            animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(position, size, parentPosition));
        }
    }
}
