using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace LegendOfZelda
{
    public class Enemy : Entity
    {
        protected AABB _aabb;
        protected Vector2 _position;
        protected Vector2 _size;

        public Enemy(Vector2 p_position, Vector2 p_size)
        {
            state = State.ACTIVE;

            _position = p_position;
            _size = p_size;
            _aabb = new AABB(p_position, p_position + p_size);
        }

        public static Enemy CreateEnemyByObject(Object p_object)
        {
            var __name = p_object.properties.Name;

            if ("Gel".Equals(__name))
            {
                return new Gel(new Vector2(p_object.x, p_object.y));
            } else
            {
                return null;
            }
            
        }
    }
}
