using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayCanvas : MonoBehaviour
{
    public static ReplayCanvas Instance;
    public GameObject replayCanvasObj;
    public Button buttonReplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private IEnumerator ShowButtonReplay()
    {
        var startScale = buttonReplay.transform.localScale;
        buttonReplay.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1.2f);
        buttonReplay.transform.DOScale(startScale, 0.8f);
    }

    public void ShowReplayCanvas(int team)
    {
        replayCanvasObj.SetActive(true);
        replayCanvasObj.transform.rotation = Quaternion.Euler(0, 0, team == 0 ? 0 : 180);
        StartCoroutine(ShowButtonReplay());
    }

    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
