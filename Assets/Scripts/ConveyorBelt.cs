using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;

    public Action<Transform> onItemDelivered;

    private Dictionary<Transform, float> items = new Dictionary<Transform, float>();

    public void AddItem(Transform item)
    {
        item.position = startPoint.position;
        items[item] = 0f;
    }

    public void RemoveItem(Transform item) => items.Remove(item);

    void Update()
    {
        float step = (speed / Vector3.Distance(startPoint.position, endPoint.position)) * Time.deltaTime;
        List<Transform> done = new List<Transform>();

        foreach (var key in new List<Transform>(items.Keys))
        {
            items[key] = Mathf.Min(items[key] + step, 1f);
            key.position = Vector3.Lerp(startPoint.position, endPoint.position, items[key]);
            if (items[key] >= 1f) done.Add(key);
        }

        foreach (var item in done)
        {
            items.Remove(item);
            onItemDelivered?.Invoke(item);
        }
    }
}