using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda
{
    public class TilesetManager
    {
       
        public enum TileSetType
        {
            ITEMS,
            ENEMIES,
            PLAYER,
            PROJECTILES,
            INVENTORY
        }
        public enum ItemTileSet
        {
            HEART_CONTAINER = 0,
            KEY,
            COMPASS,
            WOOD_SWORD,
            HEART1,
            HEART2,
            RUPEE1,
            RUPEE2,
            TRIFORCE1,
            TRIFORCE2,
            MAP,
            BOW,
            BOMB,
            BOOMERANG,
            FIRE1,
            FIRE2
        }
        public enum EnemyTileSet
        {
            OCTOROK_RED_R1,
            OCTOROK_RED_R2,
            OCTOROK_RED_U1,
            OCTOROK_RED_U2,
            OCTOROK_RED_L1,
            OCTOROK_RED_L2,
            OCTOROK_RED_D1,
            OCTOROK_RED_D2,
            OCTOROK_BLUE_R1,
            OCTOROK_BLUE_R2,
            OCTOROK_BLUE_U1,
            OCTOROK_BLUE_U2,
            OCTOROK_BLUE_L1,
            OCTOROK_BLUE_L2,
            OCTOROK_BLUE_D1,
            OCTOROK_BLUE_D2,
            GORIYA_R1,
            GORIYA_R2,
            GORIYA_U1,
            GORIYA_U2,
            GORIYA_L1,
            GORIYA_L2,
            GORIYA_D1,
            GORIYA_D2,
            STALFOS_1,
            STALFOS_2,
            KEESE_1,
            KEESE_2,
            ZORA_UNDERWATER_1,
            ZORA_UNDERWATER_2,
            ZORA_FRONT,
            ZORA_BACK,
            LEEVER_UNDERGROUND_1,
            LEEVER_UNDERGROUND_2,
            LEEVER_RED_EMERGING,
            LEEVER_RED_1,
            LEEVER_RED_2,
            LEEVER_BLUE_EMERGING,
            LEEVER_BLUE_1,
            LEEVER_BLUE_2,
            WALLMASTER_L1,
            WALLMASTER_L2,
            WALLMASTER_R1,
            WALLMASTER_R2,
            GEL_1,
            GEL_2,
            BLADETRAP_1,
            NOTHING_0,
            AQUAMENTUS_IDLE_1,
            AQUAMENTUS_IDLE_2,
            AQUAMENTUS_ATTACK_1,
            AQUAMENTUS_ATTACK_2
        }
        public enum PlayerTileSet
        {

        }
        public enum ProjectileTileSet
        {

        }
        public enum InventoryTileSet
        {

        }
        public static Texture2D itemsTileset { get; protected set; }
        public static Texture2D enemiesTileset { get; protected set; }
        public static Texture2D playerTileset { get; protected set; }
        public static Texture2D projectilesTileset { get; protected set; }
        public static Texture2D inventoryTileset { get; protected set; }

        public static void Setup()
        {
            itemsTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Items");
            enemiesTileset = Main.s_game.Content.Load<Texture2D>("TileSet_Enemies");
        }
        public static Texture2D GetTileSet(TileSetType p_tileType)
        {
            if (p_tileType == TileSetType.ITEMS)
                return itemsTileset;
            else if (p_tileType == TileSetType.ENEMIES)
                return enemiesTileset;
            else if (p_tileType == TileSetType.PLAYER)
                return playerTileset;
            else if (p_tileType == TileSetType.PROJECTILES)
                return projectilesTileset;
            else if (p_tileType == TileSetType.INVENTORY)
                return inventoryTileset;
            return itemsTileset;
        }
        public static Rectangle GetSourceRectangle(TileSetType p_type, int p_index)
        {
            Rectangle __sourceRect = new Rectangle(0, 0, 16, 16);
            if (p_type == TileSetType.ITEMS)
            {
                __sourceRect.X = (p_index % (itemsTileset.Width / 16)) * 16;
                __sourceRect.Y = (p_index / (itemsTileset.Height / 16)) * 16;
            }
            else if (p_type == TileSetType.ENEMIES)
            {
                __sourceRect.X = (p_index % (enemiesTileset.Width / 16)) * 16;
                __sourceRect.Y = (p_index / (enemiesTileset.Height / 16)) * 16;
            }
            return __sourceRect;
        }
    }
}
