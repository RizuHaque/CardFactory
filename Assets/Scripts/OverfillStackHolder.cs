using System.Collections.Generic;
using UnityEngine;

public class OverfillStackHolder : MonoBehaviour
{
    public ConveyorBelt belt;
    public Transform spawnPoint;
    public float gapBetweenStacks = 0.2f;

    private List<OverfillStack> stacks = new List<OverfillStack>();

    void Start() => belt.onItemDelivered += OnItemDelivered;

    void OnDestroy() => belt.onItemDelivered -= OnItemDelivered;

    Vector3 Origin => spawnPoint != null ? spawnPoint.position : transform.position;

    void OnItemDelivered(Transform itemTransform)
    {
        Item item = itemTransform.GetComponent<Item>();
        if (item == null) return;

        OverfillStack target = stacks.Find(s => s.colorType == item.colorType);

        if (target == null)
        {
            target = new OverfillStack(item.colorType, NextStackPosition(), belt, Arrange);
            stacks.Add(target);
        }

        target.AddItem(item);
    }

    Vector3 NextStackPosition()
    {
        float x = 0f;
        foreach (var s in stacks)
            x += s.TotalWidth + gapBetweenStacks;
        return Origin + Vector3.right * x;
    }

    void Arrange()
    {
        stacks.RemoveAll(s => s.Count == 0);
        float x = 0f;
        foreach (var s in stacks)
        {
            s.Reposition(Origin + Vector3.right * x);
            x += s.TotalWidth + gapBetweenStacks;
        }
    }
}