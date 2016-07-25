using LegendOfZelda.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class MeleeSword : Weapon
    {
        private readonly Player _player;

        public MeleeSword(Player p_source) : base(p_source, new Vector2(15.0f, 15.0f), p_source.direction)
        {
            weaponType = WeaponType.SWORD;

            maxCooldown = 0.2f;
            _player = p_source;
            _animationController = new Animations.AnimationController("LinkWoodSword");
            _animationController.ChangeAnimation(DirectionUtil.DirectionToTitleCase(p_source.direction));
        }

        public override void Update(float p_delta, Collider p_collider)
        {
            if (!IsCooldownUp())
            {
                cooldown += p_delta;

                _player.ForcePosition(initialSourcePosition);
            }
            else
            {
                DestroyEntity();
                state = State.DISABLED;
            }

            direction = _player.direction;

            base.Update(p_delta, p_collider);
        }

        public override void Draw(SpriteBatch p_spriteBatch)
        {
            _animationController.DrawFrame(p_spriteBatch,MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition));
            //p_spriteBatch.FillRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Azure);
            base.Draw(p_spriteBatch);
        }

        public override void DebugDraw(SpriteBatch p_spriteBatch)
        {
            p_spriteBatch.DrawRectangle(MathUtil.GetDrawRectangle(MathUtil.AddHUDMargin(position), size, parentPosition), Color.Orange, 2.0f);
            base.DebugDraw(p_spriteBatch);
        }
    }
}
