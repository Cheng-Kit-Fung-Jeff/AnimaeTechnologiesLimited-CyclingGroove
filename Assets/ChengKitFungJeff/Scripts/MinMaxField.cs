using UnityEngine;
public class MinMaxField : PropertyAttribute
{
    public float min;
    public float max;
    public MinMaxField(float min, float max)
    {
        this.min = min; this.max = max;
    }
}
