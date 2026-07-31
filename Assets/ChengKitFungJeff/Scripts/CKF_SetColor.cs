using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_SetColor : MonoBehaviour
{
    public List<Profile> colors = new();
    public UnityEvent<Color> getColor;
    [System.Serializable]
    public class Profile
    {
        [Color4Field] public Color color = Color.black;
        [Min(0)]public float range;
    }

    public void GetColor(float value)
    {
        Color preColor = default, curColor = default;
        bool preSet = false, curSet = false ;
        float curRange = 0;

        foreach (var c in colors)
        {
            if (value < 0) break;
            value -= c.range;
            curRange = c.range;
            if (curSet) { preColor = curColor; preSet = true; }
            curColor = c.color;
            curSet = true;
        }
        if (curSet)
        {
            if (preSet)
            {
                if (curRange == 0)
                {
                    getColor?.Invoke(curColor);
                }
                else
                {   
                    float rate = (value + curRange) / curRange; //0.6 , 1 -> -0.4
                    getColor?.Invoke(Color.Lerp(preColor, curColor, Mathf.Clamp01(rate)));
                }
            }
            else
            {
                getColor?.Invoke(curColor);
            }
        }
    }
}
