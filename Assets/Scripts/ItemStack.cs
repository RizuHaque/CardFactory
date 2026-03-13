using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public Item itemPrefab;
    public int count = 5;

    private List<Item> items = new List<Item>();
    private bool dispatched = false;

    public void Build(Transform parent, ConveyorBelt belt)
    {
        for (int i = 0; i < count; i++)
        {
            Item item = GameObject.Instantiate(itemPrefab, parent.position, Quaternion.identity);
            item.Initialize(this, belt);
            item.gameObject.SetActive(false);
            items.Add(item);
        }
    }

    public bool OwnsObject(GameObject obj) => items.Exists(i => i.gameObject == obj);

    public void Dispatch(ConveyorBelt belt)
    {
        if (dispatched || items.Count == 0) return;
        dispatched = true;

        foreach (var item in items)
        {
            item.gameObject.SetActive(true);
            belt.AddItem(item.transform);
        }

        items.Clear();
        dispatched = false;
    }
}