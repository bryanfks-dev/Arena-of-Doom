using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool FadeOnStart = true;
    public float FadeDuration = 2f;
    public Color FadeColor;
    private Renderer Render;

    public static bool SceneChanged = true;

    // Start is called before the first frame update
    void Start()
    {
        Render = GetComponent<Renderer>();

        if (FadeOnStart)
        {
            FadeIn();
        }

        SceneChanged = false;
    }

    void Update()
    {
        if (SceneChanged)
        {
            Render = GetComponent<Renderer>();

            if (FadeOnStart)
            {
                FadeIn();
            }

            SceneChanged = false;
        }
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float AlphaIn, float AlphaOut)
    {
        StartCoroutine(FadeRoutine(AlphaIn, AlphaOut));
    }

    public IEnumerator FadeRoutine(float AlphaIn, float AlphaOut)
    {
        float Timer = 0;

        while (Timer <= FadeDuration)
        {
            Color NewColor = FadeColor;
            NewColor.a = Mathf.Lerp(AlphaIn, AlphaOut, Timer / FadeDuration);

            Render.material.SetColor("_BaseColor", NewColor);

            Timer += Time.deltaTime;

            yield return null;
        }

        Color NewColor2 = FadeColor;
        NewColor2.a = AlphaOut;

        Render.material.SetColor("_BaseColor", NewColor2);
    }
}
