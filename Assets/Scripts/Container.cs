using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class Container
{
    public GameObject prefab;
    public int size = 5;
    public ColorType colorType;

    public Action<Container> onFull;

    private const float itemStride = 0.15f;

    private Vector3 position;
    private int absorbed = 0;
    private GameObject visual;
    private List<GameObject> absorbedItems = new List<GameObject>();

    public bool IsFull => absorbed >= size;
    public float XPosition => position.x;
    public float ZPosition => position.z;

    public void Build(Vector3 pos)
    {
        position = pos;
        visual = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        visual.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = colorType.ToColor();
    }

    public bool MatchesColor(Transform item)
    {
        var itemComp = item.GetComponent<Item>();
        return itemComp != null && itemComp.isOnBelt && itemComp.colorType == colorType;
    }

    public void Absorb(Transform item)
    {
        Vector3 target = position + Vector3.up * absorbed * itemStride;
        absorbed++;
        absorbedItems.Add(item.gameObject);
        item.DOKill();
        item.DOJump(target, 1f, 1, 0.35f)
            .OnComplete(() => { if (IsFull) Destroy(); });
    }

    private void Destroy()
    {
        foreach (var item in absorbedItems)
            GameObject.Destroy(item);
        absorbedItems.Clear();
        GameObject.Destroy(visual);
        onFull?.Invoke(this);
    }
}