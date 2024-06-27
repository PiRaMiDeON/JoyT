using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFader : MonoBehaviour
{
    private float alpha;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        alpha = 1;

    }

    public IEnumerator Fade(bool toVisible)
    {
        float step = toVisible ? 0.1f : -0.1f;
        int endValue = toVisible ? 1 : 0;

        while (alpha != endValue)
        {
            alpha += step;
            canvasGroup.alpha = alpha;

            if (alpha < 0)
            {
                alpha = 0;
            }

            if (alpha > 1)
            {
                alpha = 1;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
