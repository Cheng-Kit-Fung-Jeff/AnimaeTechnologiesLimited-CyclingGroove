using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CKF_TextAltered : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI text;
    private string settedText;
    public UnityEvent eventShown, eventHidden, eventChange;

    public void Awake()
    {
        settedText = text.text;
    }

    public void Update()
    {
        if (text.text == settedText) return;
        string pretext = settedText;
        settedText = text.text;
        if (pretext.Length == 0 && text.text.Length != 0)
        {
            eventShown?.Invoke();
        }
        else if (pretext.Length != 0 && text.text.Length == 0)
        {
            eventHidden?.Invoke();
        }
        if (pretext != text.text)
        {
            settedText = text.text;
            eventChange?.Invoke();
        }
    }
}
