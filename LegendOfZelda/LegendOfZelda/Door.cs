using System;
using Microsoft.Xna.Framework;

namespace LegendOfZelda
{
    public class Door : Entity
    {
        public event Action<Door, bool> OnDoorOpen;
        public enum DoorType
        {
            NONE,
            ALL_DEAD,
            KEY
        }
        public int  doorSide;
        public int  tileToOpen;
        public DoorType  doorType;
        public bool isOpen = false;
        public bool canOpen = true;
        public bool closeOnEnter;

        public Door(Object p_obj)
        {
            type = EntityType.ITEM;
            tag = "Door";
            name = p_obj.name;
            closeOnEnter = p_obj.properties.CloseOnEnter;
            tileToOpen = p_obj.properties.TileToOpen;
            doorType = (DoorType)p_obj.properties.DoorType;
            doorSide = p_obj.properties.DoorSide;
            isOpen = false;
            if (doorType == DoorType.KEY)
                canOpen = false;
            position = new Vector2(p_obj.x, p_obj.y);
            size = new Vector2(16f, 16f);
            state = State.ACTIVE;
            hitboxSize = new Vector2(16f, 16f);
            hitboxOffset = new Vector2(0f, 0f);
            UpdateAABB();
        }

        public void AllDead()
        {
            if (isOpen) return;

            if (doorType == DoorType.ALL_DEAD)
            {
                isOpen = true;
                OnDoorOpen(this, true);
            }
            else if (doorType == DoorType.KEY)
                canOpen = true;
        }
        public override void OnCollide(Entity p_entity)
        {
            base.OnCollide(p_entity);
            if (isOpen) return;

            if (p_entity.type == EntityType.PLAYER && doorType == DoorType.KEY 
                && canOpen && Inventory.Instance.keyCount > 0)
            {
                Inventory.Instance.keyCount--;
                isOpen = true;
                OnDoorOpen(this, true);
            }
        }
    }
}
