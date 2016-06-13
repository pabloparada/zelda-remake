using System.Collections.Generic;
using System;
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

            var __x = 0;
            var __y = 0;

            for (var i = 0; i < _collisionMaskLayer.data.Count; i++)
            {
                var __mask = (CollisionMask)_collisionMaskLayer.data[i];
                var __position = new Vector2(__x, __y);
                var __aabb = new AABB(__position, __position + new Vector2(16), __mask);

                _collisionMask.Add(__aabb);

                __x += 16;

                if (__x == 256)
                {
                    __x = 0;
                    __y += 16;
                }
            }
        }

        public bool IsColliding(AABB aabb, Direction p_direction)
        {
            var __pos1 = new Vector2();
            var __pos2 = new Vector2();

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

            var __index = (int)(__pos1.X / 16f) + ((int)(__pos1.Y/16f)*16);

            if (CheckCollision(_collisionMask[__index], __pos1, aabb))
            {
                return true;
            }

            __index = (int)(__pos2.X / 16f) + ((int)(__pos2.Y / 16f) * 16);

            if (CheckCollision(_collisionMask[__index], __pos2, aabb))
            {
                return true;
            }

            return false;
        }

        private bool CheckCollision(AABB p_tileAABB, Vector2 p_point, AABB p_targetAABB)
        {
            if (p_tileAABB.Mask == CollisionMask.NONE) return false;

            else if (p_tileAABB.Mask == CollisionMask.FULL) return PointInsideRectangle(p_point, p_tileAABB.Min, p_tileAABB.Max);

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_TOP_LEFT)
            {
                return PointInsideTriangle(p_point, p_tileAABB.Min, new Vector2(p_tileAABB.Max.X, p_tileAABB.Min.Y), new Vector2(p_tileAABB.Min.X, p_tileAABB.Max.Y))
                    || PointInsideRectangle(new Vector2(p_tileAABB.Max.X, p_tileAABB.Min.Y), p_targetAABB.Min, p_targetAABB.Max, true)
                    || PointInsideRectangle(new Vector2(p_tileAABB.Min.X, p_tileAABB.Max.Y), p_targetAABB.Min, p_targetAABB.Max, true);
            }

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_TOP_RIGHT)
            {
                return PointInsideTriangle(p_point, p_tileAABB.Min, p_tileAABB.Max, new Vector2(p_tileAABB.Max.X, p_tileAABB.Min.Y))
                    || PointInsideRectangle(p_tileAABB.Min, p_targetAABB.Min, p_targetAABB.Max, true)
                    || PointInsideRectangle(p_tileAABB.Max, p_targetAABB.Min, p_targetAABB.Max, true);
            }

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_BOTTOM_LEFT)
            {
                return PointInsideTriangle(p_point, p_tileAABB.Min, p_tileAABB.Max, new Vector2(p_tileAABB.Min.X, p_tileAABB.Max.Y))
                    || PointInsideRectangle(p_tileAABB.Min, p_targetAABB.Min, p_targetAABB.Max, true)
                    || PointInsideRectangle(p_tileAABB.Max, p_targetAABB.Min, p_targetAABB.Max, true);
            }
            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_BOTTOM_RIGHT)
            {
                return PointInsideTriangle(p_point, p_tileAABB.Max, new Vector2(p_tileAABB.Min.X, p_tileAABB.Max.Y), new Vector2(p_tileAABB.Max.X, p_tileAABB.Min.Y))
                    || PointInsideRectangle(new Vector2(p_tileAABB.Max.X, p_tileAABB.Min.Y), p_targetAABB.Min, p_targetAABB.Max, true)
                    || PointInsideRectangle(new Vector2(p_tileAABB.Min.X, p_tileAABB.Max.Y), p_targetAABB.Min, p_targetAABB.Max, true);
            }
            else if (p_tileAABB.Mask == CollisionMask.HALF_TOP)
                return PointInsideRectangle(p_point, p_tileAABB.Min, new Vector2(p_tileAABB.Max.X, (p_tileAABB.Max.Y + p_tileAABB.Min.Y) / 2f));

            else if (p_tileAABB.Mask == CollisionMask.HALF_BOTTOM)
                return PointInsideRectangle(p_point, new Vector2(p_tileAABB.Min.X, (p_tileAABB.Max.Y + p_tileAABB.Min.Y) / 2f), p_tileAABB.Max);

            else if (p_tileAABB.Mask == CollisionMask.HALF_LEFT)
                return PointInsideRectangle(p_point, p_tileAABB.Min, new Vector2((p_tileAABB.Max.X + p_tileAABB.Min.X) / 2f, p_tileAABB.Max.Y));

            else if (p_tileAABB.Mask == CollisionMask.HALF_RIGHT)
                return PointInsideRectangle(p_point, new Vector2((p_tileAABB.Max.X + p_tileAABB.Min.X) / 2f, p_tileAABB.Min.Y), p_tileAABB.Max);

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

        bool PointInsideRectangle(Vector2 p_point, Vector2 p_min, Vector2 p_max, bool p_collisionOnHover = false)
        {
            if (p_collisionOnHover && p_point.X >= p_min.X && p_point.X <= p_max.X &&
                p_point.Y >= p_min.Y && p_point.Y <= p_max.Y)
                return true;
            else if (p_point.X > p_min.X && p_point.X < p_max.X &&
                p_point.Y > p_min.Y && p_point.Y < p_max.Y)
                return true;
            return false;
        }
    }

    public class AABB
    {
        private Vector2 _min;
        private Vector2 _max;
        private CollisionMask _mask;

        public Vector2 Min { get { return _min; } set { _min = value; } }
        public Vector2 Max { get { return _max; } set { _max = value; } }
        
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

        public Vector2 top => new Vector2((_min.X + _max.X)/2f, _min.Y);
        public Vector2 bottom => new Vector2((_min.X + _max.X) / 2f, _max.Y);
    }
}
