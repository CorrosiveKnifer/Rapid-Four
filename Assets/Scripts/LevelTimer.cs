using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Michael Jordan
/// </summary>
public class LevelTimer : MonoBehaviour
{
    public float maxTime;
    public Camera camera;
    public Camera camera2;

    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartAnim()
    {
        camera.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
        UI.SetActive(false);
        foreach (var item in GameObject.FindObjectsOfType<Animator>())
        {
            item.SetTrigger("Start");
        }
    }
}
