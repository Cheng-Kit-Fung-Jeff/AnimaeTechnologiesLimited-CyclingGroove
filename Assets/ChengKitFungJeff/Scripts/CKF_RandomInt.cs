using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_RandomInt : MonoBehaviour
{
    public int start;
    [Min(1)]public int range = 1;
    public bool allowDuplicates;
    public UnityEvent<int> getValue;
    private List<int> pool;
    public void GetValue()
    {
        if (allowDuplicates)
        {
            getValue?.Invoke(UnityEngine.Random.Range(start, start + range));
        }
        else
        {
            pool ??= new(range);
            if (pool.Count == 0)
            {
                for (int i = 0, j = start; i < range; i++)
                {
                    pool.Add(j);
                    j++;
                }
                Fn.ShuffleFisherYates(pool);
            }
            int next = pool[^1];
            pool.RemoveAt(pool.Count - 1);
            getValue?.Invoke(next);
        }
    }
}
