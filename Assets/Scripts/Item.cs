using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ColorType colorType { get; private set; }
    public bool isOnBelt { get; set; }

    private Action onClicked;

    public void Initialize(ItemStack itemStack, ConveyorBelt conveyorBelt, StackHolder stackHolder, ColorType type)
    {
        colorType = type;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = ColorManager.Get(type);
        SetClickAction(() => itemStack.Dispatch(conveyorBelt, stackHolder.OnStackDispatched));
    }

    public void SetClickAction(Action action) => onClicked = action;

    void OnMouseDown() => onClicked?.Invoke();
}