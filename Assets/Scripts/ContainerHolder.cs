using System.Collections.Generic;
using UnityEngine;

public class ContainerHolder : MonoBehaviour
{
    public ConveyorBelt belt;
    public List<Container> containers = new List<Container>();
    public float containerSpacing = 1.5f;

    void Start()
    {
        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].Build(transform.position + Vector3.forward * i * containerSpacing);
            containers[i].onFull += OnContainerFull;
        }

        belt.onItemMoved += OnItemMoved;
    }

    void OnDestroy() => belt.onItemMoved -= OnItemMoved;

    void OnItemMoved(Transform item)
    {
        Container frontmost = null;
        float lowestZ = float.MaxValue;

        foreach (var container in containers)
        {
            if (!container.IsFull && container.ZPosition < lowestZ)
            {
                lowestZ = container.ZPosition;
                frontmost = container;
            }
        }

        if (frontmost != null && frontmost.MatchesColor(item) && item.position.x >= frontmost.XPosition)
        {
            frontmost.Absorb(item);
            belt.RemoveItem(item);
        }
    }

    void OnContainerFull(Container container) => containers.Remove(container);
}