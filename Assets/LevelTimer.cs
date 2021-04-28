using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float maxTime;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioAgent>().PlayBackground("CutSceneSpaceShips", false, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (maxTime > 0)
            maxTime -= Time.deltaTime;

        if (maxTime <= 0 || Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<LevelLoader>().LoadNextLevel();
            Destroy(this);
        }
    }
}
