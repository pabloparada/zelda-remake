﻿using Microsoft.Xna.Framework;

namespace LegendOfZelda.Util
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemyByObject(Object p_object)
        {
            var __name = p_object.properties.Name;

            if ("Gel".Equals(__name))
            { 
                return new Gel(new Vector2(p_object.x, p_object.y));
            }
            else
            {
                return new Stalfos(new Vector2(p_object.x, p_object.y));
            }
        }
    }
}
