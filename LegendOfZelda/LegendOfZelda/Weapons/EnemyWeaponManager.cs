using System;
using System.Collections.Generic;
using System.Linq;
using LegendOfZelda.Enemies;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    

    public class EnemyWeaponManager
    {
        public struct EnemyWeaponHolder
        {
            public EnemyWeaponHolder(Enemy p_enemy, Weapon p_weapon, WeaponType p_type)
            {
                enemy = p_enemy;
                weapon = p_weapon;
                type = p_type;
            }

            public Enemy enemy { get; }
            public Weapon weapon { get; }
            public WeaponType type { get; }
        }

        private readonly Dictionary<string, EnemyWeaponHolder> _weaponsCache;

        public List<Weapon> weapons { get; }

        public EnemyWeaponManager()
        {
            weapons = new List<Weapon>();

            _weaponsCache = new Dictionary<string, EnemyWeaponHolder>();
        }

        public void AddWeapon(Enemy p_enemy, WeaponType p_weaponType)
        {
            var __weap = p_weaponType == WeaponType.BOOMERANG ? (Weapon) new Boomerang(p_enemy) : new DirectionalProjectile(p_enemy);

            p_enemy.weapon = __weap;

            weapons.Add(__weap);
            _weaponsCache.Add(p_enemy.id, new EnemyWeaponHolder(p_enemy, __weap, p_weaponType));
        }

        public void RemoveWeapon(Enemy p_enemy)
        {
            var __holder = _weaponsCache[p_enemy.id];

            weapons.Remove(__holder.weapon);
            _weaponsCache.Remove(p_enemy.id);

            p_enemy.weapon = null;
        }

        public void Update(float p_delta, Collider p_collider)
        {
            weapons.RemoveAll(p_w => p_w.state != State.ACTIVE);

            foreach (var __w in weapons)
            {
                __w.Update(p_delta, p_collider);
            }
        }

        public void Draw(SpriteBatch p_spriteBatch)
        {
            foreach (var __w in weapons)
            {
                __w.Draw(p_spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch p_spriteBatch)
        {
            foreach (var __w in weapons)
            {
                __w.DebugDraw(p_spriteBatch);
            }
        }
    }
}
