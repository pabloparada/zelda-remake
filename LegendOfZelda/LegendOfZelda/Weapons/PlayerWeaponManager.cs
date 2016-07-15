using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda.Weapons
{
    public class PlayerWeaponManager
    {
        private Weapon _firstProjectile;
        private Weapon _secondProjectile;
        private Weapon _melee;

        private readonly Player _source;

        private ProjectileState _firstProjectileState;
        private ProjectileState _secondProjectileState;
        private MeleeSwordState _meleeSwordState;

        private enum MeleeSwordState
        {
            DISABLED,
            ACTIVE
        }

        private enum ProjectileState
        {
            SHOULD_FIRE,
            CREATED,
            FIRED,
            DISABLED
        }

        public PlayerWeaponManager(Player p_source)
        {
            _source = p_source;
            _firstProjectileState = ProjectileState.DISABLED;
            _secondProjectileState = ProjectileState.DISABLED;
            _meleeSwordState = MeleeSwordState.DISABLED;
        }

        public void Update(float p_delta, Collider p_collider)
        {
            UpdateFirstProjectile(p_delta, p_collider);
            UpdateSecondProjectile(p_delta, p_collider);
            UpdateMeleeAttack(p_delta, p_collider);
        }

        private void UpdateMeleeAttack(float p_delta, Collider p_collider)
        {
            if (ShouldCastMeleeAttack())
            {
                if (InputManager.GetKeyChange(Keys.X) && _meleeSwordState == MeleeSwordState.DISABLED)
                {
                    _melee = new MeleeSword(_source);
                    _meleeSwordState = MeleeSwordState.ACTIVE;
                }

                if (_meleeSwordState == MeleeSwordState.ACTIVE)
                {
                    _melee.Update(p_delta, p_collider);
                }

                if (_meleeSwordState == MeleeSwordState.ACTIVE && _melee.state == State.DISABLED)
                {
                    _meleeSwordState = MeleeSwordState.DISABLED;
                }
            }
        }

        private bool ShouldCastMeleeAttack()
        {
            return !_source.IsHealthFull() || 
                   _firstProjectileState == ProjectileState.FIRED ||
                   _secondProjectileState == ProjectileState.FIRED || 
                   _meleeSwordState == MeleeSwordState.ACTIVE;
        }

        private void UpdateSecondProjectile(float p_delta, Collider p_collider)
        {
            if (_source.IsHealthFull() && _firstProjectileState == ProjectileState.DISABLED)
            {
                if (_source.type == EntityType.PLAYER && _secondProjectileState == ProjectileState.SHOULD_FIRE)
                {
                    _secondProjectile = new DirectionalSword(_source);
                    _secondProjectileState = ProjectileState.CREATED;
                }

                if (_secondProjectileState == ProjectileState.CREATED || _secondProjectileState == ProjectileState.FIRED)
                {
                    _secondProjectile.Update(p_delta, p_collider);
                    _secondProjectileState = ProjectileState.FIRED;
                }

                if (InputManager.GetKeyChange(Keys.X) && _secondProjectileState == ProjectileState.DISABLED)
                {
                    _secondProjectileState = ProjectileState.SHOULD_FIRE;
                }

                if (_secondProjectileState == ProjectileState.FIRED && _secondProjectile.state != State.ACTIVE)
                {
                    _secondProjectileState = ProjectileState.DISABLED;
                }
            }
        }

        private void UpdateFirstProjectile(float p_delta, Collider p_collider)
        {
            if (_secondProjectileState == ProjectileState.DISABLED)
            {
                if (_source.type == EntityType.PLAYER && _firstProjectileState == ProjectileState.SHOULD_FIRE)
                {
                    _firstProjectile = new Boomerang(_source);
                    _firstProjectileState = ProjectileState.CREATED;
                }

                if (_firstProjectileState == ProjectileState.CREATED || _firstProjectileState == ProjectileState.FIRED)
                {
                    _firstProjectile.Update(p_delta, p_collider);
                    _firstProjectileState = ProjectileState.FIRED;
                }

                if (InputManager.GetKeyChange(Keys.Z) && _firstProjectileState == ProjectileState.DISABLED)
                {
                    _firstProjectileState = ProjectileState.SHOULD_FIRE;
                }

                if (_firstProjectileState == ProjectileState.FIRED && _firstProjectile.state != State.ACTIVE)
                {
                    _firstProjectileState = ProjectileState.DISABLED;
                }
            }
        }

        public void Draw(SpriteBatch p_spriteBatch)
        {
            if (_firstProjectileState == ProjectileState.FIRED)
            {
                _firstProjectile.Draw(p_spriteBatch);
            }

            if (_secondProjectileState == ProjectileState.FIRED)
            {
                _secondProjectile.Draw(p_spriteBatch);
            }

            if (_meleeSwordState == MeleeSwordState.ACTIVE)
            {
                _melee.Draw(p_spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch p_spriteBatch)
        {
            if (_firstProjectileState == ProjectileState.FIRED)
            {
                _firstProjectile.DebugDraw(p_spriteBatch);
            }

            if (_secondProjectileState == ProjectileState.FIRED)
            {
                _secondProjectile.DebugDraw(p_spriteBatch);
            }

            if (_meleeSwordState == MeleeSwordState.ACTIVE)
            {
                _melee.DebugDraw(p_spriteBatch);
            }
        }
    }
}
