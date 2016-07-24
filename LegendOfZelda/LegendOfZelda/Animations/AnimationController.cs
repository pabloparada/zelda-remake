using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Animations
{
    public class AnimationController
    {
        public List<Animation>  AnimationsList { get; }
        public Animation        Animation { get; private set; }
        public string           Name { get; private set; }
        public Color            HitColor { get; private set; }

        public AnimationController(string p_animatorType)
        {
            HitColor = new Color(255.0f, 255.0f, 255.0f, 0.01f);
            Name = p_animatorType;
            AnimationsList = new List<Animation>();
            switch (p_animatorType)
            {
                //Items
                case "Fire":
                    AddTwoFrameAnimation("Fire", TilesetManager.ItemTileSet.FIRE1, 0.5f);
                    break;
                case "Triforce":
                    AddTwoFrameAnimation("Triforce", TilesetManager.ItemTileSet.TRIFORCE1, 0.5f);
                    break;
                case "Heart":
                    AddTwoFrameAnimation("Heart", TilesetManager.ItemTileSet.FIRE1);
                    break;
                case "Rupee":
                    AddTwoFrameAnimation("Rupee", TilesetManager.ItemTileSet.RUPEE1);
                    break;
                //Enemies
                case "OctorokRed":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.OCTOROK_RED_R1);
                    break;
                case "OctorokBlue":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.OCTOROK_BLUE_R1);
                    break;
                case "Goriya":
                    AddAllDirectionAnimation(TilesetManager.EnemyTileSet.GORIYA_R1);
                    break;
                case "Stalfos":
                    AddTwoFrameAnimation("Stalfos", TilesetManager.EnemyTileSet.STALFOS_1, 0.15f);
                    break;
                case "Kesee":
                    AddTwoFrameAnimation("Keese", TilesetManager.EnemyTileSet.KEESE_1);
                    break;
                case "Zora":
                    AddTwoFrameAnimation("Underwater", TilesetManager.EnemyTileSet.ZORA_UNDERWATER_1);
                    AddOneFrameAnimation("Front", TilesetManager.EnemyTileSet.ZORA_FRONT);
                    AddOneFrameAnimation("Back", TilesetManager.EnemyTileSet.ZORA_BACK);
                    break;
                case "LeeverRed":
                    AddTwoFrameAnimation("Underground", TilesetManager.EnemyTileSet.LEEVER_UNDERGROUND_1);
                    AddOneFrameAnimation("Emerging", TilesetManager.EnemyTileSet.LEEVER_RED_EMERGING);
                    AddTwoFrameAnimation("Leever", TilesetManager.EnemyTileSet.LEEVER_RED_1);
                    break;
                case "LeeverBlue":
                    AddTwoFrameAnimation("Underground", TilesetManager.EnemyTileSet.LEEVER_UNDERGROUND_1);
                    AddOneFrameAnimation("Emerging", TilesetManager.EnemyTileSet.LEEVER_BLUE_EMERGING);
                    AddTwoFrameAnimation("Leever", TilesetManager.EnemyTileSet.LEEVER_BLUE_1);
                    break;
                case "Wallmaster":
                    AddTwoFrameAnimation("Left", TilesetManager.EnemyTileSet.WALLMASTER_L1);
                    AddTwoFrameAnimation("Right", TilesetManager.EnemyTileSet.WALLMASTER_R1);
                    break;
                case "Gel":
                    AddTwoFrameAnimation("Gel", TilesetManager.EnemyTileSet.GEL_1, 0.3f);
                    break;
                case "BladeTrap":
                    AddOneFrameAnimation("Blade", TilesetManager.EnemyTileSet.BLADETRAP_1);
                    break;
                case "Aquamentus":
                    AddTwoFrameAnimation("Idle", TilesetManager.EnemyTileSet.AQUAMENTUS_IDLE_1);
                    AddTwoFrameAnimation("Attack", TilesetManager.EnemyTileSet.AQUAMENTUS_ATTACK_1);
                    break;
                //Projectiles
                case "LinkSword":
                    AddMultipleFrameAnimation("Right", TilesetManager.ProjectileTileSet.SWORD_R1, 2, 0.1f);
                    AddMultipleFrameAnimation("Up", TilesetManager.ProjectileTileSet.SWORD_U1, 2, 0.1f);
                    AddMultipleFrameAnimation("Left", TilesetManager.ProjectileTileSet.SWORD_L1, 2, 0.1f);
                    AddMultipleFrameAnimation("Down", TilesetManager.ProjectileTileSet.SWORD_D1, 2, 0.1f);
                    break;
                case "Arrow":
                    AddAllDirectionAnimation(TilesetManager.ProjectileTileSet.ARROW_R);
                    break;
                case "Boomerang":
                    AddMultipleFrameAnimation("Boomerang", TilesetManager.ProjectileTileSet.BOOMERANG_1, 8, 0.05f);
                    break;
                //Up-Left, Up-Right, Down-Left, Down-Right
                case "SwordExplosionUL":
                    AddMultipleFrameAnimation("UL", TilesetManager.ProjectileTileSet.SWORD_EXPLOSION_UL1, 4, 0.1f);
                    break;
                case "SwordExplosionUR":
                    AddMultipleFrameAnimation("UR", TilesetManager.ProjectileTileSet.SWORD_EXPLOSION_UR1, 4, 0.1f);
                    break;
                case "SwordExplosionDL":
                    AddMultipleFrameAnimation("DL", TilesetManager.ProjectileTileSet.SWORD_EXPLOSION_DL1, 4, 0.1f);
                    break;
                case "SwordExplosionDR":
                    AddMultipleFrameAnimation("DR", TilesetManager.ProjectileTileSet.SWORD_EXPLOSION_DR1, 4, 0.1f);
                    break;
                case "EnergyBallA":
                    AddMultipleFrameAnimation("EnergyBallA", TilesetManager.ProjectileTileSet.ENERGY_BALL_A1, 2);
                    break;
                case "EnergyBallB":
                    AddMultipleFrameAnimation("EnergyBallB", TilesetManager.ProjectileTileSet.ENERGY_BALL_B1, 2);
                    break;
                case "Rock":
                    AddOneFrameAnimation("Rock", TilesetManager.ProjectileTileSet.ROCK);
                    break;
                case "DeathExplosion":
                    AddMultipleFrameAnimation("Death", TilesetManager.ProjectileTileSet.DEATH_EXPLOSION_1, 8, 0.075f);
                    break;
                case "SpawnExplosion":
                    AddMultipleFrameAnimation("Spawn", TilesetManager.ProjectileTileSet.SPAWN_EXPLOSION_1, 3);
                    break;
                //Player
                case "Player":
                    AddTwoFrameAnimation("WalkRight", TilesetManager.PlayerTileSet.LINK_WALK_R1);
                    AddTwoFrameAnimation("WalkUp", TilesetManager.PlayerTileSet.LINK_WALK_U1);
                    AddTwoFrameAnimation("WalkLeft", TilesetManager.PlayerTileSet.LINK_WALK_L1);
                    AddTwoFrameAnimation("WalkDown", TilesetManager.PlayerTileSet.LINK_WALK_D1);
                    AddOneFrameAnimation("AttackRight", TilesetManager.PlayerTileSet.LINK_ATTACK_R);
                    AddOneFrameAnimation("AttackUp", TilesetManager.PlayerTileSet.LINK_ATTACK_U);
                    AddOneFrameAnimation("AttackLeft", TilesetManager.PlayerTileSet.LINK_ATTACK_L);
                    AddOneFrameAnimation("AttackDown", TilesetManager.PlayerTileSet.LINK_ATTACK_D);
                    AddOneFrameAnimation("HoldTrifoce", TilesetManager.PlayerTileSet.LINK_HOLD_TRIFORCE);
                    break;
            }
            Animation = AnimationsList[0];
        }
        private void AddOneFrameAnimation(string p_animName, TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
        }
        private void AddOneFrameAnimation(string p_animName, TilesetManager.ProjectileTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
        }
        private void AddOneFrameAnimation(string p_animName, TilesetManager.PlayerTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
        }
        private void AddTwoFrameAnimation(string p_animName, TilesetManager.ItemTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
        }
        private void AddTwoFrameAnimation(string p_animName, TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
        }
        private void AddTwoFrameAnimation(string p_animName, TilesetManager.PlayerTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
        }
        private void AddMultipleFrameAnimation(string p_animName, TilesetManager.ProjectileTileSet p_tile, int p_frameCount, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation(p_animName));
            for(int i = 0; i < p_frameCount; i ++)
                AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + i, p_frameDuration));
        }
        private void AddAllDirectionAnimation(TilesetManager.ProjectileTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation("Right"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList.Add(new Animation("Up"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
            AnimationsList.Add(new Animation("Left"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 2, p_frameDuration));
            AnimationsList.Add(new Animation("Down"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 3, p_frameDuration));
        }
        private void AddAllDirectionAnimation(TilesetManager.EnemyTileSet p_tile, float p_frameDuration = 0.2f)
        {
            AnimationsList.Add(new Animation("Right"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 1, p_frameDuration));
            AnimationsList.Add(new Animation("Up"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 2, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 3, p_frameDuration));
            AnimationsList.Add(new Animation("Left"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 4, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 5, p_frameDuration));
            AnimationsList.Add(new Animation("Down"));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 6, p_frameDuration));
            AnimationsList[AnimationsList.Count - 1].FramesList.Add(new AnimationFrame(p_tile + 7, p_frameDuration));
        }
        
        public void ChangeAnimation(string p_animationName)
        {
            foreach(Animation __anim in AnimationsList)
                if (__anim.name == p_animationName)
                {
                    Animation = __anim;
                    Animation.ResetAnimation();
                }
        }
        public void UpdateAnimationController(float p_deltaTime)
        {
            if (Animation == null) Animation = AnimationsList[0];

            Animation.UpdateAnimation(p_deltaTime);
        }

        public void DrawFrame(SpriteBatch p_spriteBatch, Rectangle p_pos, Color p_color)
        {
            if (Animation?.Frame == null) return;

            p_spriteBatch.Draw(TilesetManager.GetTileSet(Animation.Frame.frameType), p_pos, 
                               TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), p_color);
        }
        public void DrawFrame(SpriteBatch p_spriteBatch, Rectangle p_pos)
        {
            if (Animation?.Frame == null) return;

            p_spriteBatch.Draw(TilesetManager.GetTileSet(Animation.Frame.frameType), p_pos, 
                               TilesetManager.GetSourceRectangle(Animation.Frame.frameType, Animation.Frame.GetFrameIndex()), Color.White);
        }
    }
}
