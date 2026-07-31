using UnityEngine;
public class SliderField : PropertyAttribute {
    public object min;
    public object max;
    public SliderField(float min, float max) {
        this.min = min; this.max = max; 
    }
    public SliderField(float minX, float minY, float maxX, float maxY)
    {
        min = new Vector2(minX, minY); max = new Vector2(maxX, maxY);
    }
    public SliderField(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
    {
        min = new Vector3(minX,minY,minZ); max = new Vector3(maxX, maxY, maxZ);
    }
    public SliderField(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
    {
        min = new Vector4(minX, minY, minZ, minW); max = new Vector4(maxX, maxY, maxZ, maxW);
    }
}