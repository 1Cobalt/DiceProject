using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuAnimator : MonoBehaviour
{
    public CanvasGroup menuGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        menuGroup.alpha = 0;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            menuGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        menuGroup.alpha = 1;
    }
}