using System.Collections.Generic;
using UnityEngine;

public class ContainerHolder : MonoBehaviour
{
    public ConveyorBelt belt;
    public Transform spawnPoint;
    public List<Container> containers = new List<Container>();
    public float containerSpacing = 1.5f;

    Vector3 Origin => spawnPoint != null ? spawnPoint.position : transform.position;

    void Start()
    {
        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].Build(ContainerPosition(i));
            containers[i].onFull += OnContainerFull;
        }

        belt.onItemMoved += OnItemMoved;
    }

    void OnDestroy() => belt.onItemMoved -= OnItemMoved;

    Vector3 ContainerPosition(int index) => Origin + Vector3.forward * index * containerSpacing;

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

    void OnContainerFull(Container container)
    {
        containers.Remove(container);
        for (int i = 0; i < containers.Count; i++)
            containers[i].Reposition(ContainerPosition(i));
    }
}