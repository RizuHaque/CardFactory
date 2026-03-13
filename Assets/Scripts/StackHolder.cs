using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    public ConveyorBelt belt;
    public Transform spawnPoint;
    public List<ItemStack> stacks = new List<ItemStack>();
    public float gapBetweenStacks = 0.2f;

    private const float thickness = 0.1f;

    void Start()
    {
        for (int i = 0; i < stacks.Count; i++)
            stacks[i].Build(StackPosition(i), belt, this);
    }

    Vector3 Origin => spawnPoint != null ? spawnPoint.position : transform.position;

    Vector3 StackPosition(int index)
    {
        float z = 0f;
        for (int i = 0; i < index; i++)
            z += stacks[i].TotalDepth + gapBetweenStacks;
        return Origin + Vector3.forward * z;
    }

    public void OnStackDispatched()
    {
        stacks.RemoveAll(s => s.IsEmpty());
        for (int i = 0; i < stacks.Count; i++)
            stacks[i].Reposition(StackPosition(i));
    }
}