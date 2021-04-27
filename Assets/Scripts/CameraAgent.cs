using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAgent : MonoBehaviour
{
    public GameObject[] targets;
    public Vector2 size;

    private Vector3 targetLoc;
    private bool isFollowingTarget = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFollowingTarget)
        {
            Vector3 pos = new Vector3();
            foreach (var target in targets)
            {
                pos += target.transform.position;
            }

            targetLoc = pos / targets.Length + new Vector3(0, 0, -45);
        }

        transform.position = Vector3.Lerp(transform.position, targetLoc, (isFollowingTarget) ? 1.0f : 0.005f );

        if(transform.position + new Vector3(0.05f, 0.05f, 0.05f) == targetLoc && transform.position == targetLoc + new Vector3(0.05f, 0.05f, 0.05f))
        {
            transform.position = targetLoc;
        }
    }

    public void SetTargetLoc(Vector3 loc)
    {
        isFollowingTarget = false;
        targetLoc = loc;
        targetLoc = new Vector3(targetLoc.x, targetLoc.y, -45);
    }

    public void ResetCamera()
    {
        isFollowingTarget = true;
    }
}
