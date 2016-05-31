using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class Scene : Entity
    {
        private List<Entity> Entities { get; }

        public Scene(params Entity[] p_entities)
        {
            Entities = new List<Entity>(p_entities);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Entities.ForEach(e => e.Draw(spriteBatch));
            base.Draw(spriteBatch);
        }

        public override void Update(float delta)
        {
            Entities.ForEach(e => e.Update(delta));

            base.Update(delta);
        }
    }
}
