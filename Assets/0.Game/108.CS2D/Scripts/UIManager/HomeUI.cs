using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUI : MonoBehaviour
{
    public string pveSceneName;
    public string pvpSceneName;
    public string coopSceneName;

    public void TrasferScene(int index)
    {
        if (index == 0)
        {
            SceneManager.LoadScene(pveSceneName);
        }
        else if (index == 1)
        {
            SceneManager.LoadScene(pvpSceneName);
        }
        else if (index == 2)
        {
            SceneManager.LoadScene(coopSceneName);
        }
    }
}
