﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfZelda.Animations;
using LegendOfZelda.Util;

namespace LegendOfZelda
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
        ITEM,
        WEAPON
    }

    public class Entity
    {
        public event Action<Entity> OnDestroyEntity;
        
        public string       tag = "Entity";
        public string       name = "Entity";
        public EntityType   type = EntityType.EMPTY;
        public State        state = State.DISABLED;
       
        public Vector2      parentPosition;
        public Vector2      position { get; protected set; }
        public Vector2      size { get; protected set; }
        public Direction    direction { get; protected set; }

        //Collision
        public AABB         aabb { get; protected set; }
        public Rectangle    hitbox { get; private set; }
        public Vector2      hitboxOffset { get; protected set; }
        public Vector2      hitboxSize { get; protected set; }
        
        public AnimationController  _animationController;
        public float                animationSpeed = 1.0f;

        public Entity()
        {
            aabb = new AABB();
        }

        public virtual void Update(float p_delta)
        {
            _animationController?.UpdateAnimationController(p_delta * animationSpeed);
        }

        public virtual void Update(float p_delta, Collider p_collider)
        {
            _animationController?.UpdateAnimationController(p_delta * animationSpeed);
        }

        public virtual void Draw(SpriteBatch p_spriteBatch) {}

        public virtual void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(hitbox, parentPosition), Color.Yellow, 3f);
        }

        public virtual void OnCollide(Entity p_entity) {}

        public void UpdateAABB()
        {
            aabb = new AABB(position + hitboxOffset, position + hitboxOffset + hitboxSize);
            hitbox = aabb.ToRectangle((int)hitboxSize.X,(int)hitboxSize.Y);
        }

        protected void DestroyEntity()
        {
            OnDestroyEntity?.Invoke(this);
        }

        public AABB CalculateAABBWithOffset(Vector2 p_position, Vector2 p_hitboxOffset, Vector2 p_size)
        {
            var __min = new Vector2(p_position.X + p_hitboxOffset.X, p_position.Y + p_hitboxOffset.Y);
            var __max = new Vector2(p_position.X + p_size.X - p_hitboxOffset.X, p_position.Y + p_size.Y - p_hitboxOffset.Y);
            return new AABB(__min, __max);
        }
    }
}
