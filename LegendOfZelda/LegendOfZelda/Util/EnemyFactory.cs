using LegendOfZelda.Enemies;
using Microsoft.Xna.Framework;

namespace LegendOfZelda.Util
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemyByObject(Object p_object, Player p_player, Collider p_collider)
        {
            var __name = p_object.properties.Name;

            if ("Gel".Equals(__name))
            { 
                return new Gel(new Vector2(p_object.x, p_object.y));
            }
            else if ("Stalfos".Equals(__name))
            {
                return new Stalfos(new Vector2(p_object.x, p_object.y));
            }
            else if ("OctorokBlue".Equals(__name))
            {
                return new Octorok(EnemyType.BLUE, new Vector2(p_object.x, p_object.y));
            }
            else if ("OctorokRed".Equals(__name))
            {
                return new Octorok(EnemyType.RED, new Vector2(p_object.x, p_object.y));
            }
            else if ("LeeverBlue".Equals(__name))
            {
                return new Leever(EnemyType.BLUE, new Vector2(p_object.x, p_object.y), p_player);
            }
            else if ("LeeverRed".Equals(__name))
            {
                return new Leever(EnemyType.RED, new Vector2(p_object.x, p_object.y), p_player);
            }
            else if ("Zora".Equals(__name))
            {
                return new Zora(new Vector2(p_object.x, p_object.y), p_collider, p_player);
            }
            else if ("Goriya".Equals(__name))
            {
                return new Goriya(new Vector2(p_object.x, p_object.y));
            }
            else
            {
                return new Kesee(new Vector2(p_object.x, p_object.y));
            }
        }
    }
}
