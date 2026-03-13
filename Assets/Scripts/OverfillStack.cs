using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverfillStack
{
    public ColorType colorType { get; private set; }

    private const float stride = 0.15f;
    private const float jumpPower = 1f;
    private const float jumpDuration = 0.3f;
    private const float dispatchInterval = 0.1f;

    private List<Item> items = new List<Item>();
    private Vector3 basePosition;
    private ConveyorBelt belt;
    private System.Action onDispatched;

    public int Count => items.Count;
    public float TotalWidth => items.Count * stride;

    public OverfillStack(ColorType type, Vector3 position, ConveyorBelt conveyorBelt, System.Action onDispatchedCallback)
    {
        colorType = type;
        basePosition = position;
        belt = conveyorBelt;
        onDispatched = onDispatchedCallback;
    }

    public void AddItem(Item item)
    {
        Vector3 pos = basePosition + Vector3.right * items.Count * stride;
        item.transform.DOKill();
        item.transform.DOJump(pos, jumpPower, 1, jumpDuration);
        items.Add(item);
        item.SetClickAction(Dispatch);
    }

    public void Reposition(Vector3 newBase)
    {
        basePosition = newBase;
        for (int i = 0; i < items.Count; i++)
            items[i].transform.DOMove(basePosition + Vector3.right * i * stride, 0.2f);
    }

    public void Dispatch()
    {
        if (items.Count == 0) return;

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            float delay = i * dispatchInterval;

            item.transform
                .DOJump(belt.startPoint.position, jumpPower, 1, jumpDuration)
                .SetDelay(delay);

            DOVirtual.DelayedCall(delay + jumpDuration, () =>
            {
                item.transform.DOKill();
                belt.AddItem(item.transform);
            });
        }

        items.Clear();
        onDispatched?.Invoke();
    }
}