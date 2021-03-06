﻿namespace LegendOfZelda.Animations
{
    public class AnimationFrame
    {
        public TilesetManager.TileSetType           frameType;

        public TilesetManager.ItemTileSet           itemTile;
        public TilesetManager.EnemyTileSet          enemyTile;
        public TilesetManager.PlayerTileSet         playerTile;
        public TilesetManager.ProjectileTileSet     projectileTile;
        public TilesetManager.InventoryTileSet      inventoryTile;

        public float                                duration;

        public AnimationFrame(TilesetManager.ItemTileSet p_itemTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.ITEMS;
            itemTile = p_itemTile;
            duration = p_duration;
        }
        public AnimationFrame(TilesetManager.EnemyTileSet p_enemyTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.ENEMIES;
            enemyTile = p_enemyTile;
            duration = p_duration;
        }
        public AnimationFrame(TilesetManager.PlayerTileSet p_playerTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.PLAYER;
            playerTile = p_playerTile;
            duration = p_duration;
        }
        public AnimationFrame(TilesetManager.ProjectileTileSet p_projectileTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.PROJECTILES;
            projectileTile = p_projectileTile;
            duration = p_duration;
        }
        public AnimationFrame(TilesetManager.InventoryTileSet p_inventoryTile, float p_duration = 0.2f)
        {
            frameType = TilesetManager.TileSetType.INVENTORY;
            inventoryTile = p_inventoryTile;
            duration = p_duration;
        }
        public int GetFrameIndex()
        {
            if (frameType == TilesetManager.TileSetType.ITEMS)
                return (int)itemTile;
            else if (frameType == TilesetManager.TileSetType.ENEMIES)
                return (int)enemyTile;
            else if (frameType == TilesetManager.TileSetType.PLAYER)
                return (int)playerTile;
            else if (frameType == TilesetManager.TileSetType.PROJECTILES)
                return (int)projectileTile;
            else if (frameType == TilesetManager.TileSetType.INVENTORY)
                return (int)inventoryTile;
            return 0;
        }
        
    }
}
