using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace LegendOfZelda
{
    public class Collider
    {
        private readonly Layer _collisionMasks;
        private readonly List<AABB> _collisions;

        public List<AABB> Collisions  => _collisions;

        public Collider(RootObject p_rootObject)
        {
            _collisionMasks = RootObjectUtil.GetLayerByName(p_rootObject, "CollisionMask");

            _collisions = new List<AABB>();

            var __x = 0;
            var __y = 0;

            for (var i = 0; i < _collisionMasks.data.Count; i++)
            {
                var __mask = (CollisionMask) _collisionMasks.data[i];
                var __position = new Vector2(__x, __y);
                var __aabb = new AABB(__position, __position + new Vector2(16), __mask);

                _collisions.Add(__aabb);

                __x += 16;

                if (__x == 256)
                {
                    __x = 0;
                    __y += 16;
                }
            }
        }
        public void ChangeAABBMaskType(int p_aabbIndex, CollisionMask p_mask)
        {
            _collisions[p_aabbIndex].ChangeMask(p_mask);
        }
        public List<AABB> FilterCollisionsByCollisionMasks(CollisionMask p_mask)
        {
            return _collisions.FindAll(p_aabb => p_aabb.Mask == p_mask);
        }

        public List<CollisionMask> FindCollisionMasksByDirection(AABB p_aabb, Direction p_direction)
        {
            var __points = GetPointsByDirection(p_aabb, p_direction);

            var __pos1 = __points.Item1;
            var __pos2 = __points.Item2;

            return new List<CollisionMask> {
                _collisions[GetIndexByPosition(__pos1)].Mask,
                _collisions[GetIndexByPosition(__pos1)].Mask
            };
        }

        public bool IsColliding(AABB p_aabb)
        {
            return IsColliding(p_aabb, Direction.UP) ||
                   IsColliding(p_aabb, Direction.DOWN) ||
                   IsColliding(p_aabb, Direction.RIGHT) ||
                   IsColliding(p_aabb, Direction.LEFT);
        }

        public bool IsColliding(AABB p_aabb, Direction p_direction)
        {
            var __points = GetPointsByDirection(p_aabb, p_direction);

            var __pos1 = __points.Item1;
            var __pos2 = __points.Item2;

            var __index = GetIndexByPosition(__pos1);
            if (__index >= _collisions.Count)
                return true;
            if (CheckCollision(_collisions[__index], __pos1, p_aabb))
            {
                return true;
            }

            __index = GetIndexByPosition(__pos2);

            if (CheckCollision(_collisions[__index], __pos2, p_aabb))
            {
                return true;
            }

            return false;
        }

        private int GetIndexByPosition(Vector2 p_pos)
        {
            return (int) (p_pos.X / 16.0f) + ( (int) (p_pos.Y / 16.0f) * 16);
        }

        private Tuple<Vector2, Vector2> GetPointsByDirection(AABB aabb, Direction p_direction)
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

            return new Tuple<Vector2, Vector2>(__pos1, __pos2);
        }

        private bool CheckCollision(AABB p_tileAABB, Vector2 p_point, AABB p_targetAABB)
        {
            if (p_tileAABB.Mask == CollisionMask.NONE) return false;

            else if (p_tileAABB.Mask == CollisionMask.WATER) return IsPointInsideRectangle(p_point, p_tileAABB.Min, p_tileAABB.Max, false);

            else if (p_tileAABB.Mask == CollisionMask.FULL) return IsPointInsideRectangle(p_point, p_tileAABB.Min, p_tileAABB.Max, false);

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_TOP_LEFT)
            {
                return IsPointInsideTriangle(p_point, p_tileAABB.Min, p_tileAABB.TopRight, p_tileAABB.BottomLeft)
                    || IsPointInsideRectangle(p_tileAABB.TopRight, p_targetAABB.Min, p_targetAABB.Max)
                    || IsPointInsideRectangle(p_tileAABB.BottomLeft, p_targetAABB.Min, p_targetAABB.Max);
            }

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_TOP_RIGHT)
            {
                return IsPointInsideTriangle(p_point, p_tileAABB.Min, p_tileAABB.Max, p_tileAABB.TopRight)
                    || IsPointInsideRectangle(p_tileAABB.Min + (Vector2.UnitX * 0.3f), p_targetAABB.Min, p_targetAABB.Max)
                    || IsPointInsideRectangle(p_tileAABB.Max, p_targetAABB.Min, p_targetAABB.Max);
            }

            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_BOTTOM_LEFT)
            {
                return IsPointInsideTriangle(p_point, p_tileAABB.Min, p_tileAABB.Max, p_tileAABB.BottomLeft)
                    || IsPointInsideRectangle(p_tileAABB.Min + (Vector2.UnitY * 0.3f), p_targetAABB.Min, p_targetAABB.Max)
                    || IsPointInsideRectangle(p_tileAABB.Max, p_targetAABB.Min, p_targetAABB.Max);
            }
            else if (p_tileAABB.Mask == CollisionMask.DIAGONAL_BOTTOM_RIGHT)
            {
                return IsPointInsideTriangle(p_point, p_tileAABB.Max, p_tileAABB.BottomLeft, p_tileAABB.TopRight)
                    || IsPointInsideRectangle(p_tileAABB.TopRight, p_targetAABB.Min, p_targetAABB.Max)
                    || IsPointInsideRectangle(p_tileAABB.BottomLeft, p_targetAABB.Min, p_targetAABB.Max);
            }
            else if (p_tileAABB.Mask == CollisionMask.HALF_TOP)
                return IsPointInsideRectangle(p_point, p_tileAABB.Min, p_tileAABB.Right, false);

            else if (p_tileAABB.Mask == CollisionMask.HALF_BOTTOM)
                return IsPointInsideRectangle(p_point, p_tileAABB.Left, p_tileAABB.Max, false);

            else if (p_tileAABB.Mask == CollisionMask.HALF_LEFT)
                return IsPointInsideRectangle(p_point, p_tileAABB.Min, p_tileAABB.Bottom, false);

            else if (p_tileAABB.Mask == CollisionMask.HALF_RIGHT)
                return IsPointInsideRectangle(p_point, p_tileAABB.Top, p_tileAABB.Max, false);


            return false;
        }

        float Sign(Vector2 p_point1, Vector2 p_point2, Vector2 p_point3)
        {
            return (p_point1.X - p_point3.X) * (p_point2.Y - p_point3.Y) - (p_point2.X - p_point3.X) * (p_point1.Y - p_point3.Y);
        }

        public bool IsIntersectingRectangle(Rectangle p_rec1, Rectangle p_rec2)
        {
            if (p_rec1.X < p_rec2.X + p_rec2.Width && p_rec1.X + p_rec1.Width > p_rec2.X && 
                p_rec1.Y < p_rec2.Y + p_rec2.Height && p_rec1.Y + p_rec1.Height > p_rec2.Y)
                return true;
            return false;
        }
        public bool IsIntersectingRectangle(AABB p_aabb1, AABB p_aabb2)
        {
            if (p_aabb1.Min.X < p_aabb2.Max.X && p_aabb1.Max.X > p_aabb2.Min.X
                && p_aabb1.Min.Y < p_aabb2.Max.Y && p_aabb1.Max.Y > p_aabb2.Min.Y)
                return true;
            return false;
        }
        public bool IsPointInsideTriangle(Vector2 p_point, Vector2 p_vertex1, Vector2 p_vertex2, Vector2 p_vertex3)
        {
            bool __b1, __b2, __b3;

            __b1 = Sign(p_point, p_vertex1, p_vertex2) < 0.0f;
            __b2 = Sign(p_point, p_vertex2, p_vertex3) < 0.0f;
            __b3 = Sign(p_point, p_vertex3, p_vertex1) < 0.0f;
            
            return ((__b1 == __b2) && (__b2 == __b3));
        }
        public bool IsPointInsideRectangle(Vector2 p_point, Vector2 p_min, Vector2 p_max, bool p_collisionOnHover = true)
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
        private Vector2 _size;
        private CollisionMask _mask;
        public Vector2 Size { get; set; }
        public Vector2 Min { get { return _min; } set { _min = value; } }
        public Vector2 Max { get { return _max; } set { _max = value; } }
        public Vector2 TopRight => new Vector2(_max.X, _min.Y);
        public Vector2 BottomLeft => new Vector2(_min.X, _max.Y);
        public Vector2 Top => new Vector2((_min.X + _max.X) / 2f, _min.Y);
        public Vector2 Bottom => new Vector2((_min.X + _max.X) / 2f, _max.Y);
        public Vector2 Left => new Vector2(_min.X, (_min.Y + _max.Y) / 2f);
        public Vector2 Right => new Vector2(_max.X, (_min.Y + _max.Y) / 2f);
        public Vector2 Center => new Vector2((_min.X + _max.X) / 2f, (_min.Y + _max.Y) / 2f);
       
        public CollisionMask Mask => _mask;

        public AABB() {}

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
        public void ChangeMask(CollisionMask p_mask)
        {
            _mask = p_mask;
        }
        public Rectangle ToRectangle()
        {
            return ToRectangle(16, 16);
        }
        public Rectangle ToRectangle(int p_width, int p_height)
        {
            return new Rectangle((int)_min.X, (int)_min.Y, p_width, p_height);
        }

        public Rectangle ScaledRectangleFromAABB()
        {
            return ScaledRectangleFromAABB(Math.Abs(Max.X - Min.X), Math.Abs(Max.Y - Min.Y));
        }

        public Rectangle ScaledRectangleFromAABB(float p_width, float p_height)
        {
            return new Rectangle(
                (int) (_min.X * Main.s_scale), (int) ((_min.Y + 48) * Main.s_scale),
                (int) (p_width * Main.s_scale), (int) (p_height * Main.s_scale)
            );
        }
    }
}
