using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private string mainSceneName = "Main";
    [SerializeField] private string resaultSceneName = "Resault";
    [SerializeField] private FadeManager fadeManager = null;

    public void LoadTitleScene()
    {
        StartCoroutine(LoadScene(titleSceneName));
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadScene(mainSceneName));
    }
    public void LoadTutorialScene()
    {
        StartCoroutine(LoadScene(tutorialSceneName));
    }
    public void LoadResaultScene()
    {
        StartCoroutine(LoadScene(resaultSceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        //フェードアウト完了まで待つ
        yield return StartCoroutine(fadeManager.FadeOut());
        SceneManager.LoadScene(sceneName);
    }
}