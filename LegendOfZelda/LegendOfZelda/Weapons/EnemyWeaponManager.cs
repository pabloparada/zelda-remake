using System.Collections.Generic;
using LegendOfZelda.Enemies;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfZelda.Weapons
{
    public class EnemyWeaponManager
    {
        public struct EnemyWeaponHolder
        {
            public EnemyWeaponHolder(Weapon p_weapon, WeaponType p_type)
            {
                weapon = p_weapon;
                type = p_type;
                enemy = null;
            }

            public EnemyWeaponHolder(Enemy p_enemy, Weapon p_weapon, WeaponType p_type) : this(p_weapon, p_type)
            {
                enemy = p_enemy;
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

        public void AddWeapon(Enemy p_enemy, Weapon p_weapon)
        {
            p_enemy.weapon = p_weapon;

            weapons.Add(p_weapon);
            _weaponsCache.Add(p_enemy.id, new EnemyWeaponHolder(p_enemy, p_weapon, p_weapon.weaponType));
        }

        public void AddWeapons(List<Weapon> p_weapons)
        {
            weapons.AddRange(p_weapons);

            foreach (var __w in p_weapons)
            {
                _weaponsCache.Add(__w.id, new EnemyWeaponHolder(__w, __w.weaponType));
            }
        }

        public void RemoveWeaponFromManagerByWeaponId(string p_id)
        {
            var __holder = _weaponsCache[p_id];

            weapons.Remove(__holder.weapon);
            _weaponsCache.Remove(p_id);
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
