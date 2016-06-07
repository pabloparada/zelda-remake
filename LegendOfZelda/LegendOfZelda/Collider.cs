using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LegendOfZelda
{
    public class Collider
    {
        private readonly Layer _collisionMaskLayer;
        private readonly List<AABB> _collisionMask;

        public List<AABB> Collisions  => _collisionMask;

        public Collider(RootObject p_rootObject)
        {
            _collisionMaskLayer = RootObjectUtil.GetLayerByName(p_rootObject, "CollisionMask");

            _collisionMask = new List<AABB>();

            int __x = 0;
            int __y = 0;

            for (var i = 0; i < _collisionMaskLayer.data.Count; i++)
            {
                var __mask = (CollisionMask)_collisionMaskLayer.data[i];
                
                    var __position = new Vector2(__x, __y);
                    var __aabb = new AABB(__position, __position + new Vector2(16), __mask);

                    _collisionMask.Add(__aabb);
                //}

                __x += 16;

                if (__x == 256)
                {
                    __x = 0;
                    __y += 16;
                }
            }
        }

        public bool IsColliding(AABB aabb)
        {
            foreach (var obj in _collisionMask)
            {
                if (obj.Mask == CollisionMask.NONE)
                    return false;
                if ((aabb.Min.X >= obj.Min.X && aabb.Max.X <= obj.Max.X) && (aabb.Min.Y >= obj.Min.Y && aabb.Max.Y <= obj.Max.Y))
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsColliding(AABB aabb, Direction p_direction)
        {
            /*foreach (var obj in _collisionMask)
            {
                if ((aabb.Min.X >= obj.Min.X && aabb.Max.X <= obj.Max.X) && (aabb.Min.Y >= obj.Min.Y && aabb.Max.Y <= obj.Max.Y))
                {
                    return true;
                }
            }*/
            Vector2 __pos1 = new Vector2();
            Vector2 __pos2 = new Vector2();
            if (p_direction == Direction.UP)
            {
                __pos1 = aabb.Min;
                __pos2 = new Vector2(aabb.Max.X, aabb.Min.Y);
            }
            else if (p_direction == Direction.RIGHT)
            {
                __pos1 = aabb.Max;
                __pos2 = new Vector2(aabb.Max.X, aabb.Min.Y);
            }
            else if (p_direction == Direction.LEFT)
            {
                __pos1 = aabb.Min;
                __pos2 = new Vector2(aabb.Min.X, aabb.Max.Y);
            }
            else if (p_direction == Direction.DOWN)
            {
                __pos1 = aabb.Max;
                __pos2 = new Vector2(aabb.Min.X, aabb.Max.Y);
            }
            int __index = (int)(__pos1.X / 16f) + ((int)(__pos1.Y/16f)*16);
            if (CheckCollision(_collisionMask[__index].Mask, __pos1))
                return true;
            __index = (int)(__pos2.X / 16f) + ((int)(__pos2.Y / 16f) * 16);
            if (CheckCollision(_collisionMask[__index].Mask, __pos2))
                return true;
            //System.Console.WriteLine(__index);
            return false;
        }
        private bool CheckCollision(CollisionMask p_mask, Vector2 p_pos)
        {
            if (p_mask == CollisionMask.NONE)
                return false;
            else if (p_mask == CollisionMask.FULL)
                return true;
            //else if (p_mask == CollisionMask.)
            return false;
        }

    }

    public class AABB
    {
        private readonly Vector2 _min;
        private readonly Vector2 _max;
        private CollisionMask _mask;

        public Vector2 Min => _min;
        public Vector2 Max => _max;
        public CollisionMask Mask => _mask;

        public AABB(Vector2 p_min, Vector2 p_max)
        {
            _min = p_min;
            _max = p_max;
        }

        public AABB(Vector2 p_min, Vector2 p_max, CollisionMask p_mask)
        {
            _min = p_min;
            _max = p_max;
            _mask = p_mask;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)_min.X, (int)_min.Y, 16, 16);
        }
    }
}
