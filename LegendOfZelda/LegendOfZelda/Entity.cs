﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;

namespace LegendOfZelda
{
    public class Entity
    {
        public enum State
        {
            ACTIVE,
            DRAW_ONLY,
            DISABLED
        }
        public enum EntityType
        {
            EMPTY,
            WORLD,
            SCENE,
            PLAYER,
            ENEMY,
            PROJECTILE,
            PORTAL,
            ITEM
        }

        public string       tag = "Entity";
        public string       name = "Entity";
        public EntityType   type = EntityType.EMPTY;
        public State        state = State.DISABLED;

        public Vector2      parentPosition;
        public Vector2      position { get; protected set; }
        public Vector2      size { get; protected set; }
        public Rectangle    hitbox { get; protected set; }

        public AnimationController animationController;

        public virtual void Update(float p_delta)
        {
            if (animationController != null && type == EntityType.ITEM)
                animationController.UpdateAnimationController(p_delta);
        }
        public virtual void Update(float p_delta, Collider p_collider)
        {
            if (animationController != null && type == EntityType.ITEM)
                animationController.UpdateAnimationController(p_delta);
        }
        public virtual void Draw(SpriteBatch p_spriteBatch) { }
        public virtual void DebugDraw(SpriteBatch p_spriteBatch) { }
    }
}
