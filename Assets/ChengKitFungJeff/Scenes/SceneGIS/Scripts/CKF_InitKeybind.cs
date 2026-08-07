using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CKF_InitKeybind : MonoBehaviour
{
    [System.Serializable]
    public class Profile
    {
        public InputActionReference inputAction;
        public string file;
    }

    public TextMeshProUGUI error;

    public List<Profile> inputs;

    private void Awake()
    {
        foreach (var p in inputs)
        {
            string res = Rw.Read(p.file, out string err);
            if (err != null)
            {
                error.text += err+ ';';
                continue;
            }
            try
            { p.inputAction.action.ApplyBindingOverride(0, res); }
            catch (Exception e){ error.text += e.Message + ';'; }
        }
    }
}
