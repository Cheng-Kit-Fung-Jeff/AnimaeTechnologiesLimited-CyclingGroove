using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CKF_GetFile : MonoBehaviour
{
    public string file;
    public TextMeshProUGUI error;
    public bool checkString = false;
    public UnityEvent<string> getString = new();
    public bool checkFloat = false;
    public UnityEvent<float> getFloat = new();
    public bool checkInt = false;
    public UnityEvent<int> getInt = new();

    public void GetFile()
    {
        GetFile(file);
    }
    public void GetFile(string file)
    {
        string res = Rw.Read(file, out string err);
        if (err != null)
        {
            if(error != null)
                error.text += err + ';';
            return;
        }
        if(checkString)
            getString?.Invoke(res);
        if (error == null)
        {
            if (checkFloat && float.TryParse(res, out float f))
                getFloat?.Invoke(f);
            if (checkFloat && int.TryParse(res, out int i))
                getInt?.Invoke(i);
        }
        else
        {
            if(checkFloat)
                try { getFloat?.Invoke(float.Parse(res)); }
                catch (System.Exception e){ error.text += $"{System.IO.Path.Combine(Application.dataPath,file)}, {e.Message};"; }
            if (checkInt)
                try { getInt?.Invoke(int.Parse(res)); }
                catch (System.Exception e) { error.text += $"{System.IO.Path.Combine(Application.dataPath, file)}, {e.Message};"; }
        }
    }
}
