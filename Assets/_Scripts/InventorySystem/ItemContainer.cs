using UnityEngine;

namespace _Scripts.InventorySystem
{
    public abstract class ItemContainer : MonoBehaviour, IItemContainer
    {
        [SerializeField] protected ItemSlot[] itemSlots;

        public virtual bool AddItem(Item item)
        {
            Debug.Log(item + " Added to the inventory");
            foreach (var slot in itemSlots)
            {
                if (slot.Item == null || slot.CanAddStack(item))
                {
                    slot.Item = item;
                    slot.Amount++;
                    return true;
                }
            }

            return false;
        }

        public virtual bool AddItems(Item[] itemList)
        {
            if (!IsThereEnoughSpace(itemList.Length)) return false; //Todo add them to a seperate bag or a tab?
            foreach (var item in itemList)
            {
                if(item!=null) AddItem(item);
            }

            return true;
        }

        private bool IsThereEnoughSpace(int itemListLength)
        {
            var spaceCount = 0;
            foreach (var slot in itemSlots)
            {
                if (slot.Item == null)
                {
                    spaceCount++;
                }
            }

            return spaceCount >= itemListLength;
        }


        public virtual void RemoveInventory()
        {
            foreach (var slot in itemSlots)
            {
                slot.Amount = 0;
                slot.Item = null;
            }
        }

        public virtual bool RemoveItem(Item item)
        {
            foreach (var slot in itemSlots)
            {
                if (slot.Item == item)
                {
                    slot.Amount--;
                    return true;
                }
            }

            return false;
        }

        public virtual Item RemoveItem(string itemID)
        {
            foreach (var slot in itemSlots)
            {
                var item = slot.Item;
                if (item != null && item.ID == itemID)
                {
                    slot.Amount--;
                    return item;
                }
            }

            return null;
        }

        public virtual bool IsFull()
        {
            foreach (var slot in itemSlots)
            {
                if (slot.Item == null)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual int ItemCount(string itemID)
        {
            var itemCount = 0;
            foreach (var slot in itemSlots)
            {
                if (slot.Item.ID == itemID)
                {
                    itemCount++;
                }
            }

            return itemCount;
        }
    }
}