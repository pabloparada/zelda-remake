﻿using System;
using LegendOfZelda.Weapons;
using Microsoft.Xna.Framework;

namespace LegendOfZelda.Enemies
{
    public class Enemy : Entity
    {
        protected Vector2 aabbSize;
        protected WeaponType hittedBy;

        public event Action<Enemy, Weapon> AddWeaponToManager;
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
            stunTimer = 0.0f;
        }

        protected void InvokeAddWeaponToManager(Weapon p_weapon)
        {
            AddWeaponToManager?.Invoke(this, p_weapon);
        }

        protected void InvokeRemoveWeaponFromManager()
        {
            RemoveWeaponFromManager?.Invoke(this);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            aabb = CalculateAABBWithOffset(position, hitboxOffset, size);

            base.Update(p_delta, p_collider);
        }

        public override void OnCollide(Entity p_entity)
        {
            if (ShouldCountAsHit(p_entity))
            {
                var __weapon = (Weapon) p_entity;

                hittedBy = __weapon.weaponType;

                if (__weapon.weaponType == WeaponType.BOOMERANG)
                {
                    var __bom = (Boomerang) __weapon;
                    isStuned = !__bom.switchedDirection;
                }
                else
                {
                    life -= 1;

                    if (life == 0)
                    {
                        state = State.DISABLED;

                        if (weapon != null) InvokeRemoveWeaponFromManager();

                        DestroyEntity();

                        SoundManager.instance.Play(SoundType.ENEMY_KILL);
                    }
                    else
                    {
                        SoundManager.instance.Play(SoundType.ENEMY_HITTED);
                    }

                    immunityTimeAferHit = 0.0f;

                }
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
