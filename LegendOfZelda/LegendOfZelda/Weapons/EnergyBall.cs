using LegendOfZelda.Animations;
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

        private float _yPositionOffset;

        public EnergyBall(Entity p_source, Player p_player, Vector2 p_size, float p_yPositionOffset = 0.0f) : base(p_source, p_size, p_source.direction)
        {
            _velocity = new Vector2(120.0f, 120.0f);
            _player = p_player;
            _direction = Vector2.Normalize(MathUtil.GetEntityCenter(p_player.position, p_player.size) - MathUtil.GetEntityCenter(p_source.position, p_source.size));
            _yPositionOffset = p_yPositionOffset;

            _animationController = new AnimationController(SortEnergyBallAnimation());

            hitboxOffset = new Vector2(5.0f, 2.5f);
            position = MathUtil.GetEntityCenter(p_source.position, p_source.size);
            size = GetProjectileSizeAndControlComponentSwitch(p_size);
        }

        private string SortEnergyBallAnimation()
        {
            return World.s_random.Next(1, 3) == 1 ? "EnergyBallA" : "EnergyBallB";
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (state != State.ACTIVE) return;

            var __tmpPos = position + _direction * _velocity * p_delta;

            if (_yPositionOffset != 0.0f)
            {
                __tmpPos.Y += _yPositionOffset * p_delta;
            }

            if (!IsAtScreenBoundaries(__tmpPos, size))
            {
                DestroyEntity();
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
            _animationController.DrawFrame(p_spriteBatch, MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition));

            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawLine(MathUtil.ScaleVectorForDrawing(MathUtil.GetEntityCenter(source.position, source.size)), MathUtil.ScaleVectorForDrawing(MathUtil.GetEntityCenter(_player.position, _player.size)), Color.Orange, 2.0f);
            p_spriteBatch.DrawCircle(MathUtil.ScaleVectorForDrawing(position), 10.0f, 100, Color.Orange, 2.0f);
            p_spriteBatch.DrawCircle(MathUtil.ScaleVectorForDrawing(MathUtil.GetEntityCenter(_player.position, _player.size)), 10.0f, 100, Color.Orange, 2.0f);
            p_spriteBatch.DrawRectangle(aabb.ScaledRectangleFromAABB(), Color.Orange, 2.0f);

            base.DebugDraw(p_spriteBatch);
        }
    }
}
