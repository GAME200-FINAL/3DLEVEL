using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id { get; set; }
    public string title { get; set; }
    public bool stackable { get; set; }
    public string description { get; set; }
    public int initialAmount { get; set; }
    public int currentAmount { get; set; }
    public Sprite itemIcon { get; set; }
    public Item(int id, string title, bool stackable, string description, int initialAmount, Sprite itemIcon)
    {
        this.id = id;
        this.title = title;
        this.stackable = stackable;
        this.description = description;
        this.initialAmount = initialAmount;
        this.itemIcon = itemIcon;
    }

    // If an item is not been initialized, this item will have -1 index in case been read.
    public Item()
    {
        id = -1;
    }
}
