using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class ItemStack
{
    public Item itemPrefab;
    public int count = 5;
    public ColorType colorType;

    private const float thickness = 0.1f;
    private const float gap = 0.05f;
    private const float stride = thickness + gap;
    private const float jumpPower = 1.5f;
    private const float jumpDuration = 0.4f;
    private const float dispatchInterval = 0.15f;

    private List<Item> items = new List<Item>();
    private bool dispatched = false;

    public float TotalDepth => count * stride;

    public void Build(Vector3 basePosition, ConveyorBelt belt, StackHolder holder)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = basePosition + Vector3.forward * i * stride;
            Item item = GameObject.Instantiate(itemPrefab, pos, Quaternion.identity);
            item.Initialize(this, belt, holder, colorType);
            items.Add(item);
        }
    }

    public void Reposition(Vector3 basePosition)
    {
        for (int i = 0; i < items.Count; i++)
            items[i].transform.position = basePosition + Vector3.forward * i * stride;
    }

    public void Dispatch(ConveyorBelt belt, Action onDispatched = null)
    {
        if (dispatched || items.Count == 0) return;
        dispatched = true;

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            float delay = i * dispatchInterval;

            item.transform.DOJump(belt.startPoint.position, jumpPower, 1, jumpDuration).SetDelay(delay);
            item.transform.DORotateQuaternion(belt.BeltRotation, jumpDuration).SetDelay(delay);

            DOVirtual.DelayedCall(delay + jumpDuration, () =>
            {
                item.transform.DOKill();
                belt.AddItem(item.transform);
            });
        }

        float totalTime = (items.Count - 1) * dispatchInterval + jumpDuration;
        DOVirtual.DelayedCall(totalTime, () =>
        {
            items.Clear();
            dispatched = false;
            onDispatched?.Invoke();
        });
    }

    public bool IsEmpty() => items.Count == 0;
}
