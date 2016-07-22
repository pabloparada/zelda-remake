using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class EnergyBall : Weapon
    {
        private readonly Vector2 _velocity;
        private readonly Vector2 _direction;

        private readonly Player _player;

        public EnergyBall(Entity p_source, Player p_player, Vector2 p_size) : base(p_source, p_size, p_source.direction)
        {
            _velocity = new Vector2(120.0f, 120.0f);
            _player = p_player;
            _direction = Vector2.Normalize(GetEntityCenter(p_player.position, p_player.size) - GetEntityCenter(p_source.position, p_source.size));

            position = GetEntityCenter(p_source.position, p_source.size);
        }

        private static Vector2 GetEntityCenter(Vector2 p_position, Vector2 p_size)
        {
            return p_position + p_size * 0.5f;
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (state != State.ACTIVE) return;

            var __tmpPos = position + _direction * _velocity * p_delta;
            
            if (!IsAtScreenBoundaries(__tmpPos, size))
            {
                state = State.DISABLED;
            }
            else
            {
                position = __tmpPos;
            }

            base.Update(p_delta, p_collider);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Blue);

            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawLine(MathUtil.ScaleVectorForDrawing(GetEntityCenter(source.position, source.size)), MathUtil.ScaleVectorForDrawing(GetEntityCenter(_player.position, _player.size)), Color.Orange, 2.0f);
            p_spriteBatch.DrawCircle(MathUtil.ScaleVectorForDrawing(position), 10.0f, 100, Color.Orange, 2.0f);
            p_spriteBatch.DrawCircle(MathUtil.ScaleVectorForDrawing(GetEntityCenter(_player.position, _player.size)), 10.0f, 100, Color.Orange, 2.0f);
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
