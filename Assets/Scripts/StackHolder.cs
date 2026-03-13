using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    public ConveyorBelt belt;
    public List<ItemStack> stacks = new List<ItemStack>();

    void Start()
    {
        foreach (var stack in stacks)
            stack.Build(transform, belt);
    }
}