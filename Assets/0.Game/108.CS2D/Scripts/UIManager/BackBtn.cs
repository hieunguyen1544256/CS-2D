using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("HomeScene");
    }
}


