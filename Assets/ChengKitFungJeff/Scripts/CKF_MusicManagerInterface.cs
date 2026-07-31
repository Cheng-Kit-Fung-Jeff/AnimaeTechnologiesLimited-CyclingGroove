using UnityEngine;

public class CKF_MusicManagerInterface : MonoBehaviour
{
    public static void Play(string key)
    {
        CKF_MusicManager.Play(key);
    }

    public static void ForcePlay(string key)
    {
        CKF_MusicManager.ForcePlay(key);
    }

    public static void Pause(string key)
    {
        CKF_MusicManager.Pause(key);
    }
    public static void UnPause(string key)
    {
        CKF_MusicManager.UnPause(key);
    }
}
