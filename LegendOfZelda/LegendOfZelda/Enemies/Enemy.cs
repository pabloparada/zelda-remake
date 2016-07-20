using System;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;

namespace LegendOfZelda.Enemies
{
    public class Enemy : Entity
    {
        protected Vector2 aabbSize;
        protected int life;
        protected float immunityTimeAferHit;

        public event Action<Enemy, WeaponType> AddWeaponToManager;
        public event Action<Enemy> RemoveWeaponFromManager;

        public Weapon weapon { get; set; }

        public Enemy(Vector2 p_position, Vector2 p_size, Vector2 p_aabbOffset, int p_life = 1)
        {
            state = State.ACTIVE;
            type = EntityType.ENEMY;
            size = p_size;
            life = p_life;
            hitboxOffset = p_aabbOffset;
            aabbSize = size - hitboxOffset;
            position = CenterInTile(p_position);
            aabb = CalculateAABBWithOffset(position, hitboxOffset, size);
            immunityTimeAferHit = -0.5f;
        }

        protected void InvokeAddWeaponToManager(WeaponType p_weaponType)
        {
            AddWeaponToManager?.Invoke(this, p_weaponType);
        }

        protected void InvokeRemoveWeaponFromManager()
        {
            RemoveWeaponFromManager?.Invoke(this);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            aabb = CalculateAABBWithOffset(position, hitboxOffset, size);

            if (immunityTimeAferHit >= 0.0f)
            {
                immunityTimeAferHit += p_delta;
            }

            if (immunityTimeAferHit >= 1.0f)
            {
                immunityTimeAferHit = -0.5f;
            }
            
            base.Update(p_delta, p_collider);
        }

        public override void OnCollide(Entity p_entity)
        {
            if (ShouldCountAsHit(p_entity))
            {
                life -= 1;

                if (life == 0)
                {
                    state = State.DISABLED;
                    DestroyEntity();
                }

                immunityTimeAferHit = 0.0f;
            }

            base.OnCollide(p_entity);
        }

        private bool ShouldCountAsHit(Entity p_entity)
        {
            if (p_entity.type == EntityType.WEAPON && immunityTimeAferHit == -0.5f)
            {
                var __weapon = (Weapon) p_entity;

                return __weapon.source.type != EntityType.ENEMY;
            }

            return false;
        }

        protected Vector2 CenterInTile(Vector2 p_position)
        {
            return p_position - (size * 0.5f) + (Vector2.One * 8.0f);
        }

        protected bool IsBoundary(Vector2 p_pos)
        {
            var __line = (int)((p_pos.Y) / 16.0f);
            var __col = (int)(p_pos.X / 16.0f);

            return __col >= 13 || __col <= 1 || __line <= 1 || __line >= 9;
        }
    }
}
