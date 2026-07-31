using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class CKF_Textbox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int lineCharLimit = 40, lineLimit = 10;
    [SerializeField] List<string> lines = new();
    [SerializeField] KeyCode buttonPrevious, buttonNext;
    [SerializeField] float holdDelay = 0.5f, holdWait = 0.2f;
    float holdTime = -1;
    [SerializeField] TMP_Text display;
    int showingLine = -1;
    void Awake()
    {
        if(!display) display = GetComponent<TMP_Text>();
        lines = lines.Select(e => e + "\n").ToList();
        int i = 0;
        foreach (string str in lines) {
            DisplayIncrement();
            ++i;
            if (i == 10) break;
        }
        holdTime = holdDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(buttonPrevious) != Input.GetKeyDown(buttonNext))
            if (Input.GetKeyDown(buttonPrevious)) DisplayDecrement(); else DisplayIncrement();
        if (Input.GetKey(buttonPrevious) || Input.GetKey(buttonNext))
        {
            if (holdTime > Time.deltaTime) holdTime -= Time.deltaTime;
            else if (holdTime > 0) holdTime = 0;
            else if (holdTime + holdWait > Time.deltaTime) holdTime -= Time.deltaTime;
            else {
                holdTime = 0;
                if (Input.GetKey(buttonPrevious) != Input.GetKey(buttonNext))
                    if (Input.GetKey(buttonPrevious)) DisplayDecrement(); else DisplayIncrement();
            }

        }
        else holdTime = holdDelay;
    }
    void DisplayRemoveFirstLine() {
        display.text = display.text.Substring(display.text.IndexOf("\n") + 1);
    }
    void DisplayRemoveLastLine()
    {
        display.text = display.text.Substring(0, display.text.LastIndexOf('\n', display.text.Length-2)+1);
    }

    public void AddLine(string str) {
        foreach (string substr in str.Split('\n',System.StringSplitOptions.RemoveEmptyEntries))
            for (int i = 0; i < substr.Length; i+= lineCharLimit)
            {
                lines.Add(substr.Substring(i, Mathf.Min(i+lineCharLimit, substr.Length)-i) + "\n");
                DisplayIncrement();
            }
    }
    public void DisplayIncrement() {
        if (showingLine >= lines.Count - 1) return;
        showingLine++;
        display.text += lines[showingLine];
        if (showingLine < lineLimit) return;
        DisplayRemoveFirstLine();
    }
    public void DisplayDecrement() {
        if (showingLine < lineLimit) return;
        DisplayRemoveLastLine();
        display.text = lines[showingLine - lineLimit] + display.text;
        showingLine--;
    }
}
