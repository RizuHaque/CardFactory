using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;

    public Action<Transform> onItemDelivered;
    public Action<Transform> onItemMoved;

    private Dictionary<Transform, float> items = new Dictionary<Transform, float>();
    private Quaternion beltRotation;

    void Start()
    {
        beltRotation = Quaternion.LookRotation(endPoint.position - startPoint.position);
    }

    public void AddItem(Transform item)
    {
        item.position = startPoint.position;
        item.rotation = beltRotation;
        items[item] = 0f;

        var i = item.GetComponent<Item>();
        if (i != null) i.isOnBelt = true;
    }

    public void RemoveItem(Transform item)
    {
        items.Remove(item);
        var i = item.GetComponent<Item>();
        if (i != null) i.isOnBelt = false;
    }

    void Update()
    {
        float step = speed / Vector3.Distance(startPoint.position, endPoint.position) * Time.deltaTime;
        List<Transform> done = new List<Transform>();

        foreach (var key in new List<Transform>(items.Keys))
        {
            if (!items.ContainsKey(key)) continue;

            items[key] = Mathf.Min(items[key] + step, 1f);
            key.position = Vector3.Lerp(startPoint.position, endPoint.position, items[key]);

            onItemMoved?.Invoke(key);

            if (!items.ContainsKey(key)) continue;
            if (items[key] >= 1f) done.Add(key);
        }

        foreach (var item in done)
        {
            items.Remove(item);
            var i = item.GetComponent<Item>();
            if (i != null) i.isOnBelt = false;
            onItemDelivered?.Invoke(item);
        }
    }
}