using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public FadeScreen FadeScreen;

    public void GoToScene(int SceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(SceneIndex));
    }

    IEnumerator GoToSceneRoutine(int SceneIndex)
    {
        FadeScreen.FadeOut();

        yield return new WaitForSeconds(FadeScreen.FadeDuration);

        AsyncOperation Operation = SceneManager.LoadSceneAsync(SceneIndex);

        Operation.allowSceneActivation = false;

        float Timer = 0;

        while (Timer <= FadeScreen.FadeDuration && !Operation.isDone)
        {
            Timer += Time.deltaTime;
            yield return null;
        }

        Operation.allowSceneActivation = true;

        FadeScreen.SceneChanged = true;
    }
}
