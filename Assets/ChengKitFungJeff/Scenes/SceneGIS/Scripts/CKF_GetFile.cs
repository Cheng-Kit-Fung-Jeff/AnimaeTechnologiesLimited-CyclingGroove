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
    public UnityEvent<string> getError;

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
            {
                error.text += err + '\n';
            }
            getError?.Invoke(err);
            return;
        }
        if(checkString)
            getString?.Invoke(res);
        if (checkFloat)
            try { getFloat?.Invoke(float.Parse(res)); }
            catch (System.Exception e)
            {
                err = $"{System.IO.Path.Combine(Application.dataPath, file)}, {e.Message}\n";
                if (error == null)
                    error.text += err;
                getError?.Invoke(err);
            }
        if (checkInt)
            try { getInt?.Invoke(int.Parse(res)); }
            catch (System.Exception e)
            {
                err = $"{System.IO.Path.Combine(Application.dataPath, file)}, {e.Message}\n";
                if (error == null)
                    error.text += err;
                getError?.Invoke(err);
            }
    }
}
