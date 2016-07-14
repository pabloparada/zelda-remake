using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda.Weapons
{
    public class PlayerWeaponManager
    {
        private Weapon _firstProjectile;
        private Weapon _secondProjectile;
        private Weapon _melee;

        private readonly Entity _source;

        private ProjectileState _firstProjectileState;
        private ProjectileState _secondProjectileState;

        private enum ProjectileState
        {
            SHOULD_FIRE,
            CREATED,
            FIRED,
            DISABLED
        }

        public PlayerWeaponManager(Entity p_source)
        {
            _source = p_source;
            _firstProjectileState = ProjectileState.DISABLED;
            _secondProjectileState = ProjectileState.DISABLED;
        }

        public void Update(float p_delta, Collider p_collider)
        {
            UpdateFirstProjectile(p_delta, p_collider);
            UpdateSecondProjectile(p_delta, p_collider);
        }

        private void UpdateSecondProjectile(float p_delta, Collider p_collider)
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

        private void UpdateFirstProjectile(float p_delta, Collider p_collider)
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
        }
    }
}
