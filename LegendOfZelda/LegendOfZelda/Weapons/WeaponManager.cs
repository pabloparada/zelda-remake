using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfZelda.Weapons
{
    public class WeaponManager
    {
        private Weapon _projectile;
        private Weapon _melee;

        private Entity _source;

        private ProjectileState _projectileState;

        private enum ProjectileState
        {
            SHOULD_FIRE,
            CREATED,
            FIRED,
            DISABLED
        }

        public WeaponManager(Entity p_source)
        {
            _source = p_source;
            _projectileState = ProjectileState.DISABLED;
        }

        public void Update(float p_delta, Collider p_collider)
        {
            if (_source.type == EntityType.PLAYER && _projectileState == ProjectileState.SHOULD_FIRE)
            {
                _projectile = new Boomerang(_source);
                _projectileState = ProjectileState.CREATED;
            }

            if (_projectileState == ProjectileState.CREATED || _projectileState == ProjectileState.FIRED)
            {
                _projectile.Update(p_delta, p_collider);
                _projectileState = ProjectileState.FIRED;
            }

            if (InputManager.GetKeyChange(Keys.Z) && _projectileState == ProjectileState.DISABLED)
            {
                _projectileState = ProjectileState.SHOULD_FIRE;
            }

            if (_projectileState == ProjectileState.FIRED && _projectile.state != State.ACTIVE)
            {
                _projectileState = ProjectileState.DISABLED;
            }

        }

        public void Draw(SpriteBatch p_spriteBatch)
        {
            if (_projectileState == ProjectileState.FIRED)
            {
                _projectile.Draw(p_spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch p_spriteBatch)
        {
            if (_projectileState == ProjectileState.FIRED)
            {
                _projectile.DebugDraw(p_spriteBatch);
            }
        }
    }
}
