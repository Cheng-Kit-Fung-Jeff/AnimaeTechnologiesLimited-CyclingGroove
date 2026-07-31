using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_SceneGame : MonoBehaviour
{
    public List<VoiceProfile> voiceProfiles = new();
    private Fn.NestedToken voiceTokeniser = new();
    //private HashSet<string> trackKeysHashset = new();
    public List<string> censorCode = new();

    [ReadonlyField] public string playerCode;


    [System.Serializable]
    public class VoiceProfile
    {
        public static Dictionary<string,VoiceProfile> VoiceProfileMap = new();
        public string key;
        [Min(0)] public float delay = 0, duration = 0;
        public VoiceProfile(string key, float delay, float duration)
        {
            this.key = key;
            this.delay = delay;
            this.duration = duration;
            VoiceProfileMap.Add(key,this);
        }
    }

    private void Awake()
    {
        foreach (var v in voiceProfiles)
        {
            VoiceProfile.VoiceProfileMap.Add(v.key,v);
            voiceTokeniser.Add(v.key);
        }
        //foreach (var v in VoiceProfile.VoiceProfileMap.Keys) { Debug.Log(v); }
        StartSequence();
    }

    public void SetPlayerCode(string code)
    {
        playerCode = code;
    }

    public void StartSequence()
    {
        string dialog = "";

        bool playerCodeIsNumber = int.TryParse(playerCode, out _);

        if (playerCodeIsNumber)
        {
            dialog = "number";
            foreach (char c in playerCode) { dialog += " " + c; }
            dialog += " are you ready?";
        }
        else
        {
            dialog = "contestant";
            foreach (char c in playerCode) { dialog += " " + c; }
            dialog += " ready?";
        }
        Debug.Log(dialog);

        var dialogTokens = dialog.Split();
        int start = 0;
        int deb = 0;
        while ((start = voiceTokeniser.FindBest(dialogTokens, start, voiceTokeniser, out string key)) != -1)
        {
            Debug.Log(key);
            deb++;
            if (deb > 100) break;
        }
    }

    public void GoSequence()
    {
    }
}
