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
    private const float jumpDuration = 0.35f;

    private Vector3 position;
    private int absorbed = 0;
    private GameObject visual;

    public bool IsFull => absorbed >= size;
    public float XPosition => position.x;
    public float ZPosition => position.z;

    public void Build(Vector3 pos)
    {
        position = pos;
        visual = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        visual.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = ColorManager.Get(colorType);
    }

    public void Reposition(Vector3 newPos)
    {
        position = newPos;
        visual.transform.DOMove(newPos, 0.3f);
    }

    public bool MatchesColor(Transform item)
    {
        var itemComp = item.GetComponent<Item>();
        return itemComp != null && itemComp.isOnBelt && itemComp.colorType == colorType;
    }

    public void Absorb(Transform item)
    {
        Vector3 target = position + Vector3.forward * absorbed * itemStride;
        absorbed++;
        item.DOKill();
        item.transform.DOJump(target, 1f, 1, jumpDuration)
            .OnComplete(() =>
            {
                item.transform.SetParent(visual.transform);
                if (IsFull) PlayFullAnimation();
            });
        item.transform.DORotateQuaternion(Quaternion.identity, jumpDuration);
    }

    private void PlayFullAnimation()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(visual.transform.DOMove(visual.transform.position + Vector3.up * 0.8f, 0.4f).SetEase(Ease.OutQuad));
            seq.Join(visual.transform.DOScale(new Vector3(1f, 1.3f, 1f), 0.4f).SetEase(Ease.OutQuad));
            seq.Append(visual.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
            seq.OnComplete(Destroy);
        });
    }

    private void Destroy()
    {
        GameObject.Destroy(visual);
        onFull?.Invoke(this);
    }
}