using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        Debug.Log("Load Scene " + name);
        SceneManager.LoadScene(name);
    }
}
