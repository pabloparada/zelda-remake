using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfZelda
{
    public class SaveState
    {
        public List<RoomState> rooms;
        public SaveState ()
        {
            rooms = new List<RoomState>();
        }
        public void AddRoom(string p_room)
        {
            if (GetRoom(p_room) == null)
                rooms.Add(new RoomState(p_room));
        }
        public void AddEnemyToRoom(string p_enemy, string p_room)
        {
            AddRoom(p_room);
            GetRoom(p_room).AddEnemy(p_enemy);
        }
        public void AddItemToRoom(string p_item, string p_room)
        {
            AddRoom(p_room);
            GetRoom(p_room).AddItem(p_item);
        }
        public void AddDoorToRoom(string p_door, string p_room)
        {
            AddRoom(p_room);
            GetRoom(p_room).AddDoor(p_door);
        }
        public bool HasEnemy(string p_enemy, string p_room)
        {
            if (GetRoom(p_room) == null)
                return false;
            else
                return GetRoom(p_room).HasEnemy(p_enemy);
        }
        public bool HasItem(string p_item, string p_room)
        {
            if (GetRoom(p_room) == null)
                return false;
            else
                return GetRoom(p_room).HasItem(p_item);
        }
        public bool HasDoor(string p_door, string p_room)
        {
            if (GetRoom(p_room) == null)
                return false;
            else
                return GetRoom(p_room).HasDoor(p_door);
        }
        public RoomState GetRoom(string p_name)
        {
            foreach (RoomState __rs in rooms)
                if (__rs.name == p_name)
                    return __rs;
            return null;
        }
    }
    public class RoomState
    {
        public string name;
        public List<string> items;
        public List<string> enemies;
        public List<string> doors;

        public RoomState(string p_name)
        {
            name = p_name;
            items = new List<string>();
            enemies = new List<string>();
            doors = new List<string>();
        }
        public void AddEnemy(string p_enemy)
        {
            if (!HasEnemy(p_enemy))
                enemies.Add(p_enemy);
        }
        public bool HasEnemy(string p_enemy)
        {
            return enemies.Contains(p_enemy);
        }
        public void AddItem(string p_item)
        {
            if (!HasItem(p_item))
                items.Add(p_item);
        }
        public bool HasItem(string p_item)
        {
            return items.Contains(p_item);
        }
        public void AddDoor(string p_door)
        {
           if (!HasDoor(p_door))
                doors.Add(p_door);
        }
        public bool HasDoor(string p_door)
        {
            return doors.Contains(p_door);
        }
    }
}
