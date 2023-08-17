﻿using HermoCommons;

namespace HermoItemManagers.Managers
{
    public sealed class ItemsManager
    {
        private class Entity
        {
            public Entity(ItemDatas? item = null)
            {
                Item = item ?? ItemDatas.DEFAULT_ITEM;
                RefCount = 1;
            }

            public ItemDatas Item;
            public int RefCount;

            public event EventHandler<ItemEditedEventArgs>? ItemEdited;
            public event EventHandler<EventArgs>? ItemDeleted;

            public void OnItemEdited(ItemEditedEventArgs e)
            {
                EventHandler<ItemEditedEventArgs>? handler = ItemEdited;
                handler?.Invoke(this, e);
            }

            public void OnItemDeleted(EventArgs e)
            {
                EventHandler<EventArgs>? handler = ItemDeleted;
                handler?.Invoke(this, e);
            }
        }

        private ItemsManager()
        {
            entities = new();
        }

        private static readonly Lazy<ItemsManager> lazy = new(() => new ItemsManager());
        private readonly Dictionary<int, Entity> entities;

        public static ItemsManager Instance { get { return lazy.Value; } }

        internal void AddFieldsFormToEvents(int itemHash, FieldsForm fieldsForm)
        {
            if (!entities.ContainsKey(itemHash)) return;

            entities[itemHash].ItemEdited += fieldsForm.ItemWasEdited;
            entities[itemHash].ItemDeleted += fieldsForm.ItemWasDeleted;
        }

        public ItemDatas[] AddReference(ItemDatas[] items)
        {
            ItemDatas[] ret = new ItemDatas[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                int itemHash = items[i].GetHashCode();

                if (entities.ContainsKey(itemHash))
                {
                    ret[i] = entities[itemHash].Item;
                    entities[itemHash].RefCount++;
                }
                else
                {
                    ret[i] = items[i];
                    entities.Add(itemHash, new(items[i]));
                }
            }

            return ret;
        }

        public void Edit(ItemDatas newItem)
        {
            int itemHash = newItem.GetHashCode();
            if (!entities.ContainsKey(itemHash)) return;

            entities[itemHash].Item = newItem;
            entities[itemHash].OnItemEdited(new(newItem));
        }

        public void RemoveReference(ItemDatas[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                int itemHash = items[i].GetHashCode();
                if (!entities.ContainsKey(itemHash)) continue;

                if (entities[itemHash].RefCount > 1)
                {
                    entities[itemHash].RefCount--;
                }
                else
                {
                    entities.Remove(itemHash);
                }
            }
        }

        public void Delete(ItemDatas item)
        {
            int itemHash = item.GetHashCode();
            if (!entities.ContainsKey(itemHash)) return;

            entities[itemHash].OnItemDeleted(new());
            entities.Remove(itemHash);
        }
    }

    internal class ItemEditedEventArgs : EventArgs
    {
        public ItemEditedEventArgs(ItemDatas newItem) : base()
        {
            NewItem = newItem;
        }

        public ItemDatas NewItem { get; }
    }
}