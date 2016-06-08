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
            if (CheckCollision(_collisionMask[__index], __pos1))
                return true;
            __index = (int)(__pos2.X / 16f) + ((int)(__pos2.Y / 16f) * 16);
            if (CheckCollision(_collisionMask[__index], __pos2))
                return true;
            //System.Console.WriteLine(__index);
            return false;
        }
        private bool CheckCollision(AABB p_aabb, Vector2 p_point)
        {
            if (p_aabb.Mask == CollisionMask.NONE)
                return false;
            else if (p_aabb.Mask == CollisionMask.FULL)
                return true;
            else if (p_aabb.Mask == CollisionMask.DIAGONAL_TOP_LEFT)
                return PointInsideTriangle(p_point, p_aabb.Min, new Vector2(p_aabb.Max.X, p_aabb.Min.Y), new Vector2(p_aabb.Min.X, p_aabb.Max.Y));
            else if (p_aabb.Mask == CollisionMask.DIAGONAL_TOP_RIGHT)
                return PointInsideTriangle(p_point, p_aabb.Min, new Vector2(p_aabb.Max.X, p_aabb.Min.Y), p_aabb.Max);
            else if (p_aabb.Mask == CollisionMask.DIAGONAL_BOTTOM_LEFT)
                return PointInsideTriangle(p_point, p_aabb.Min, new Vector2(p_aabb.Min.X, p_aabb.Max.Y), p_aabb.Max);
            else if (p_aabb.Mask == CollisionMask.DIAGONAL_BOTTOM_RIGHT)
                return PointInsideTriangle(p_point, p_aabb.Max, new Vector2(p_aabb.Min.X, p_aabb.Max.Y), new Vector2(p_aabb.Max.X, p_aabb.Min.Y));
            else if (p_aabb.Mask == CollisionMask.HALF_TOP)
                return PointInsideRectangle(p_point, p_aabb.Min, new Vector2(p_aabb.Max.X, (p_aabb.Max.Y + p_aabb.Min.Y) / 2f));
            else if (p_aabb.Mask == CollisionMask.HALF_BOTTOM)
                return PointInsideRectangle(p_point, new Vector2(p_aabb.Min.X, (p_aabb.Max.Y + p_aabb.Min.Y) / 2f), p_aabb.Max);
            else if (p_aabb.Mask == CollisionMask.HALF_LEFT)
                return PointInsideRectangle(p_point, p_aabb.Min, new Vector2((p_aabb.Max.X + p_aabb.Min.X) / 2f, p_aabb.Max.Y));
            else if (p_aabb.Mask == CollisionMask.HALF_RIGHT)
                return PointInsideRectangle(p_point, new Vector2((p_aabb.Max.X + p_aabb.Min.X) / 2f, p_aabb.Min.Y), p_aabb.Max);
            return false;
        }
        float Sign(Vector2 p_point1, Vector2 p_point2, Vector2 p_point3)
        {
            return (p_point1.X - p_point3.X) * (p_point2.Y - p_point3.Y) - (p_point2.X - p_point3.X) * (p_point1.Y - p_point3.Y);
        }

        bool PointInsideTriangle(Vector2 p_point, Vector2 p_vertex1, Vector2 p_vertex2, Vector2 p_vertex3)
        {
            bool __b1, __b2, __b3;

            __b1 = Sign(p_point, p_vertex1, p_vertex2) < 0.0f;
            __b2 = Sign(p_point, p_vertex2, p_vertex3) < 0.0f;
            __b3 = Sign(p_point, p_vertex3, p_vertex1) < 0.0f;

            return ((__b1 == __b2) && (__b2 == __b3));
        }
        bool PointInsideRectangle(Vector2 p_point, Vector2 p_min, Vector2 p_max)
        {
            if (p_point.X >= p_min.X && p_point.X <= p_max.X &&
                p_point.Y >= p_min.Y && p_point.Y <= p_max.Y)
                return true;
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
