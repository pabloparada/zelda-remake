using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace LegendOfZelda.Weapons
{
    public class PlayerWeaponManager
    {
        public event Action<Vector2> OnLinkSwordDie;
        private Weapon _firstProjectile;
        private Weapon _secondProjectile;
        private Weapon _melee;
        private readonly Player _source;

        private WeaponState _firstProjectileState;
        private WeaponState _secondProjectileState;
        private WeaponState _meleeSwordState;

        private float _secondProjectileCooldown;

        public List<Weapon> weapons
        {
            get
            {
                var __ws = new List<Weapon>();

                if (_firstProjectileState != WeaponState.DISABLED) __ws.Add(_firstProjectile);
                if (_secondProjectileState != WeaponState.DISABLED && _secondProjectileState != WeaponState.WAITING_FOR_COOLDOWN) __ws.Add(_secondProjectile);
                if (_meleeSwordState != WeaponState.DISABLED) __ws.Add(_melee);

                return __ws;
            }
        }

        private enum WeaponState
        {
            DISABLED,
            WAITING_FOR_COOLDOWN,
            ACTIVE
        }

        public PlayerWeaponManager(Player p_source)
        {
            _source = p_source;

            _firstProjectileState = WeaponState.DISABLED;
            _secondProjectileState = WeaponState.DISABLED;
            _meleeSwordState = WeaponState.DISABLED;
        }

        public void Update(float p_delta, Collider p_collider)
        {
            UpdateFirstProjectile(p_delta, p_collider);
            UpdateSecondProjectile(p_delta, p_collider);
            UpdateMeleeAttack(p_delta, p_collider);
        }

        private void UpdateMeleeAttack(float p_delta, Collider p_collider)
        {
            if (ShouldCastMeleeAttack() && Inventory.Instance.hasSword)
            {
                if (InputManager.GetKeyChange(Keys.X) && _meleeSwordState == WeaponState.DISABLED)
                {
                    _melee = new MeleeSword(_source);
                    _meleeSwordState = WeaponState.ACTIVE;
                    _source.SetAttackAnimation();

                    SoundManager.instance.Play(SoundType.SWORD);
                }

                if (_meleeSwordState == WeaponState.ACTIVE)
                {
                    _melee.Update(p_delta, p_collider);
                }

                if (_meleeSwordState == WeaponState.ACTIVE && _melee.state == State.DISABLED)
                {
                    _meleeSwordState = WeaponState.DISABLED;
                }
            }
        }

        private bool ShouldCastMeleeAttack()
        {
            return !_source.IsHealthFull() || 
                   _firstProjectileState == WeaponState.ACTIVE ||
                   _secondProjectileState == WeaponState.ACTIVE ||
                   _secondProjectileState == WeaponState.WAITING_FOR_COOLDOWN ||
                   _meleeSwordState == WeaponState.ACTIVE;
        }

        private void UpdateSecondProjectile(float p_delta, Collider p_collider)
        {
            if (Inventory.Instance.hasSword && _source.IsHealthFull() && _firstProjectileState == WeaponState.DISABLED || _secondProjectileState == WeaponState.ACTIVE)
            {
                if (_secondProjectileState == WeaponState.ACTIVE)
                {
                    _secondProjectile.Update(p_delta, p_collider);
                }

                if (InputManager.GetKeyChange(Keys.X) && _secondProjectileState == WeaponState.DISABLED)
                {
                    _secondProjectile = new DirectionalProjectile(_source, new Vector2(16.0f, 16.0f), new Vector2(2.0f, 5.0f), "LinkSword");
                    _secondProjectile.OnDestroyEntity += SecondProjectileOnDestroyEntity;
                    _secondProjectileState = WeaponState.ACTIVE;

                    SoundManager.instance.Play(SoundType.SWORD_PROJECTILE);
                }

                if (_secondProjectileState == WeaponState.ACTIVE && _secondProjectile.state != State.ACTIVE)
                {
                    _secondProjectileState = WeaponState.WAITING_FOR_COOLDOWN;
                }
            }

            if (_secondProjectileState == WeaponState.WAITING_FOR_COOLDOWN)
            {
                if (_secondProjectileCooldown >= 1.0f)
                {
                    _secondProjectileState = WeaponState.DISABLED;
                    _secondProjectileCooldown = 0.0f;
                }
                else
                {
                    _secondProjectileCooldown += p_delta;
                }
            }
        }

        private void SecondProjectileOnDestroyEntity(Entity p_entity)
        {
            _secondProjectileState = WeaponState.WAITING_FOR_COOLDOWN;
            OnLinkSwordDie(p_entity.position);
        }

        private void UpdateFirstProjectile(float p_delta, Collider p_collider)
        {
            if (_secondProjectileState == WeaponState.DISABLED && Inventory.Instance.hasBoomerang)
            {
                if (_firstProjectileState == WeaponState.ACTIVE)
                {
                    _firstProjectile.Update(p_delta, p_collider);
                }

                if (InputManager.GetKeyChange(Keys.Z) && _firstProjectileState == WeaponState.DISABLED)
                {
                    _firstProjectile = new Boomerang(_source);
                    _firstProjectileState = WeaponState.ACTIVE;

                    SoundManager.instance.Play(SoundType.BOOMERANG);
                }

                if (_firstProjectileState == WeaponState.ACTIVE && _firstProjectile.state != State.ACTIVE)
                {
                    _firstProjectileState = WeaponState.DISABLED;
                }
            }
        }

        public void Draw(SpriteBatch p_spriteBatch)
        {
            if (_firstProjectileState == WeaponState.ACTIVE)
            {
                _firstProjectile.Draw(p_spriteBatch);
            }

            if (_secondProjectileState == WeaponState.ACTIVE)
            {
                _secondProjectile.Draw(p_spriteBatch);
            }

            if (_meleeSwordState == WeaponState.ACTIVE)
            {
                _melee.Draw(p_spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch p_spriteBatch)
        {
            if (_firstProjectileState == WeaponState.ACTIVE)
            {
                _firstProjectile.DebugDraw(p_spriteBatch);
            }

            if (_secondProjectileState == WeaponState.ACTIVE)
            {
                _secondProjectile.DebugDraw(p_spriteBatch);
            }

            if (_meleeSwordState == WeaponState.ACTIVE)
            {
                _melee.DebugDraw(p_spriteBatch);
            }
        }
    }
}
