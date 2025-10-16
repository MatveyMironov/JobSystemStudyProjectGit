using Unity.Jobs;
using UnityEngine;

public struct LogarithmJob : IJobParallelFor
{
    public void Execute(int index)
    {
        System.Random random = new();
        int value = random.Next(1, 100);
        float logarithm = Mathf.Log(value);
        Debug.Log(logarithm);
    }
}
