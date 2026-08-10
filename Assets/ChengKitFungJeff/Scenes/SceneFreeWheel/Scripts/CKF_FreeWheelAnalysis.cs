using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CKF_FreeWheelAnalysis : MonoBehaviour
{
    private readonly List<double> dataTime = new(2), dataInterval = new(3);
    private double startTime;
    private float startAcceleration, lowerAcceleration = float.PositiveInfinity, upperAcceleration = float.NegativeInfinity;
    private readonly List<(float time, float acceleration)> dataAcceleration = new();
    private readonly List<GameObject> objectsPlot = new();


    public RectTransform layerLine, layerPoint, layerRefPoint;
    public CKF_RectTransform scalerRect, translateRect;
    public CKF_RectRefHeight scalerHeight;
    public CKF_RectRefWidth scalerWidth;
    public Vector2 baseScaler = new(100, 100);
    public GameObject PlotLine, PlotPoint, PlotRefPoint;


    public void AddDataPoint(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        Debug.Log("Logging " + context.startTime.ToString());

        if(dataTime.Count == 2) dataTime.RemoveAt(0);
        dataTime.Add(context.startTime);
        if (dataTime.Count == 1) return;

        if (dataInterval.Count == 3) dataInterval.RemoveAt(0);
        dataInterval.Add(dataTime[1] - dataTime[0]);
        if (dataInterval.Count == 3)
        {
            double acceleration = EstimateHalfAcceleration(dataInterval);
            if (acceleration >= 0)
            {
                if(dataAcceleration.Count > 0)
                {
                    dataAcceleration.Clear();
                    foreach (var obj in objectsPlot)
                        Destroy(obj);
                    objectsPlot.Clear();
                    lowerAcceleration = float.PositiveInfinity; upperAcceleration = float.NegativeInfinity;
                    scalerRect.SetWidth(baseScaler.x);
                    scalerRect.SetHeight(baseScaler.y);
                    scalerHeight.Apply();
                    scalerWidth.Apply();
                    translateRect.SetAnchoredPositionX(-0.5f * baseScaler.x);
                }
            }
            else
            {
                if(dataAcceleration.Count == 0)
                {
                    startTime = context.startTime;
                    startAcceleration = (float)acceleration;
                }
                dataAcceleration.Add(new((float)(context.startTime - startTime), (float)acceleration));
                objectsPlot.Add(Instantiate(PlotPoint, layerPoint));
                if (dataAcceleration[^1].acceleration < lowerAcceleration)
                    lowerAcceleration = dataAcceleration[^1].acceleration;
                if (dataAcceleration[^1].acceleration > upperAcceleration)
                    upperAcceleration = dataAcceleration[^1].acceleration;
                CKF_RectTransform newRect = objectsPlot[^1].GetComponent<CKF_RectTransform>();
                newRect.SetAnchoredPosition(new(dataAcceleration[^1].time, dataAcceleration[^1].acceleration - startAcceleration));
                objectsPlot.Add(Instantiate(PlotRefPoint, layerRefPoint));
                objectsPlot[^1].GetComponent<CKF_RectRefPosition>().refRect = objectsPlot[^2].transform as RectTransform;
                if(dataAcceleration.Count > 1)
                {
                    scalerRect.SetWidth(dataAcceleration[^1].time);
                    if(upperAcceleration != lowerAcceleration)
                        scalerRect.SetHeight(2 * (upperAcceleration - lowerAcceleration));
                    scalerHeight.Apply();
                    scalerWidth.Apply();
                    translateRect.SetAnchoredPositionX(-0.5f * dataAcceleration[^1].time);
                    objectsPlot.Add(Instantiate(PlotLine, layerLine));
                    CKF_PathRectSprite newPath = objectsPlot[^1].GetComponent<CKF_PathRectSprite>();
                    newPath.nodeA = (dataAcceleration.Count == 2 ? objectsPlot[^4] : objectsPlot[^5]).transform as RectTransform;
                    newPath.nodeB = objectsPlot[^2].transform as RectTransform;
                }
            }
        }
    }

    private double EstimateHalfAcceleration(List<double> dataInterval)
    {
        double
            b1 = (1 / dataInterval[1] - 1 / dataInterval[0]) / (dataInterval[1] + dataInterval[0]),
            b2 = (1 / dataInterval[2] - 1 / dataInterval[1]) / (dataInterval[2] + dataInterval[1]);
        return b1 + (b2 - b1) / (dataInterval[2] + dataInterval[1] + dataInterval[0]);
    }
}
