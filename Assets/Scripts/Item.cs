using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemStack stack;
    private ConveyorBelt belt;

    public void Initialize(ItemStack itemStack, ConveyorBelt conveyorBelt)
    {
        stack = itemStack;
        belt = conveyorBelt;
    }

    void OnMouseDown() => stack.Dispatch(belt);
}