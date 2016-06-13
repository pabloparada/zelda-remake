using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class SwordProjectile : Projectile
    {
        private Vector2 _velocity;
        private Rectangle _hitbox;
        private AABB _aabb;
        private Vector2 _projectileSize;

        private bool _isColliding;

        public SwordProjectile(Vector2 p_position, Direction p_direction) : base(p_position, p_direction)
        {
            _projectileSize = new Vector2(10, 5);
            _aabb = new AABB(p_position, p_position + _projectileSize);
            _velocity = new Vector2(80, 80);
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            var __direction = InputManager.GetDirectionVectorByDirectionEnum(direction);

            position += __direction *_velocity * p_delta;

            _aabb.Min = position;
            _aabb.Max = position + _projectileSize;

            _isColliding = p_collider.IsColliding(_aabb, direction);

            if (_isColliding)
            {
                alive = false;
            }

            base.Update(p_delta, p_collider);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            var __debugHitbox = new Rectangle((int) (position.X * Main.s_scale), 
                                              (int) (position.Y * Main.s_scale + 48 * Main.s_scale), 
                                              (int) (position.X + _projectileSize.X * Main.s_scale), 
                                              (int) (_projectileSize.Y * Main.s_scale));

            p_spriteBatch.DrawRectangle(__debugHitbox, Color.Red, 1.0f);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
        }
    }
}
