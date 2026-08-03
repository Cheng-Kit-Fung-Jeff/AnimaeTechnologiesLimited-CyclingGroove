using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gf : MonoBehaviour
{
    public static void SceneLoad(string scene) { SceneManager.LoadScene(scene, LoadSceneMode.Single); }
}
