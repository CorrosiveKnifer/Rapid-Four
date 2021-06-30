using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// William de Beer
/// </summary>
public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] sideCameras;
    public Camera mainCamera;
    public Image mainImage;
    public Image barrier;
    public RenderTexture render;

    private bool fadeInTrigger = false;
    private bool fadeOutTrigger = false;
    private bool fadeInOnce = false;
    private bool fadeOutOnce = false;

    void Start()
    {
        mainImage.enabled = false;
        render.width = Screen.currentResolution.width;
        render.height = Screen.currentResolution.height;
    }

    // Update is called once per frame
    void Update()
    {
        bool cond = true;
        foreach (var side in sideCameras)
        {
            if(Vector2.Distance(mainCamera.transform.position, side.transform.position) > 8.0f)
            {
                cond = false;
            }
        }

        if(cond)
        {
            fadeOutOnce = false;
            if (!fadeInOnce)
            {
                StartCoroutine(FadeIn());
                fadeInOnce = true;
            }
        }
        else
        {
            fadeInOnce = false;
            if(!fadeOutOnce)
            {
                StartCoroutine(FadeOut());
                fadeOutOnce = true;
            }
            
        }
    }

    private IEnumerator FadeIn()
    {
        if(fadeInTrigger)
            yield return null;

        fadeInTrigger = true;

        mainImage.enabled = true;
        mainImage.color = new Color(255f, 255f, 255f, 0.0f);
        barrier.color = new Color(255f, 255f, 255f, 1.0f);

        do
        {
            if(fadeOutTrigger)
            {
                fadeInTrigger = false;
                yield return null;
            }

            mainImage.color = Color.Lerp(mainImage.color, new Color(255, 255, 255, 1.0f), 0.05f);
            barrier.color = Color.Lerp(barrier.color, new Color(255, 255, 255, 0.0f), 0.05f);
            yield return new WaitForEndOfFrame();
        } while (mainImage.color.a < 0.95f);

        mainImage.enabled = false;
        mainCamera.enabled = true;
        foreach (var side in sideCameras)
        {
            side.enabled = false;
        }
        fadeInTrigger = false;
        yield return null;
    }

    private IEnumerator FadeOut()
    {
        if (fadeOutTrigger)
            yield return null;

        fadeOutTrigger = true;

        mainImage.enabled = true;
        mainImage.color = new Color(255f, 255f, 255f, 1.0f);
        barrier.color = new Color(255f, 255f, 255f, 0.0f);
        foreach (var side in sideCameras)
        {
            side.enabled = true;
        }

        do
        {
            if (fadeInTrigger)
            {
                fadeOutTrigger = false;
                yield return null;
            }

            mainImage.color = Color.Lerp(mainImage.color, new Color(255, 255, 255, 0.0f), 0.05f);
            barrier.color = Color.Lerp(barrier.color, new Color(255, 255, 255, 1.0f), 0.05f); ;
            yield return new WaitForEndOfFrame();
        } while (mainImage.color.a > 0.05f);

        mainImage.enabled = false;
        fadeOutTrigger = false;
        yield return null;
    }
}
