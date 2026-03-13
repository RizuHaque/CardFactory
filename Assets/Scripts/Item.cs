using UnityEngine;

public class Item : MonoBehaviour
{
    public ColorType colorType { get; private set; }

    private ItemStack stack;
    private ConveyorBelt belt;
    private StackHolder holder;

    public void Initialize(ItemStack itemStack, ConveyorBelt conveyorBelt, StackHolder stackHolder, ColorType type)
    {
        stack = itemStack;
        belt = conveyorBelt;
        holder = stackHolder;
        colorType = type;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = type.ToColor();
    }

    void OnMouseDown() => stack.Dispatch(belt, holder.OnStackDispatched);
}