using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// William de Beer
/// </summary>
public class CameraAgent : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();
    public Vector2 size;

    private Vector3 targetLoc;
    private bool isFollowingTarget = true;
    private bool isLocked = false;

    private float shakeTime;
    private float shakeTotal;
    private Vector3 shakeVector;
    private float lerpToTargetVal = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isFollowingTarget)
        {
            Vector3 pos = new Vector3();
            foreach (var target in targets)
            {
                pos += target.transform.position;
            }

            targetLoc = pos / targets.Count + new Vector3(0, 0, -45);
        }

        targetLoc += shakeVector;

        transform.position = Vector3.Lerp(transform.position, targetLoc, (isFollowingTarget) ? 1.0f : lerpToTargetVal);

        Vector3 lockBuffer = new Vector3(0.001f, 0.001f, 0.001f);
        if (transform.position + lockBuffer == targetLoc && transform.position == targetLoc + lockBuffer)
        {
            transform.position = targetLoc;
        }

        targetLoc -= shakeVector;
    }

    public void SetTargetLoc(Vector3 loc, bool blockReset = false, float lerp = 0.005f)
    {
        isFollowingTarget = false;
        targetLoc = loc;
        isLocked = blockReset;
        lerpToTargetVal = lerp;
    }

    public void ResetCamera()
    {
        isFollowingTarget = true && !isLocked;
    }

    public void Shake(float mag)
    {
        StartCoroutine(ShakeCam(1.0f, mag));
    }

    private IEnumerator ShakeCam(float time, float mag)
    {
        if(shakeTime > 0)
        {
            shakeTime += time;
            shakeTotal = mag;
            yield return null;
        }

        shakeTime += time;
        shakeTotal = mag;

        float dampenOverTime = 5.0f;
        
        do
        {
            if (shakeTotal <= 0)
                break;

            float dist = Vector2.Distance(transform.position, new Vector2(0.0f, 0.0f));
            float ratio = Mathf.Clamp(1.0f - dist / 100f, 0.0f, 1.0f);

            shakeVector = new Vector3(Random.Range(-0.5f, 0.5f) * shakeTotal * ratio, Random.Range(-0.5f, 0.5f) * shakeTotal * ratio, 0);
            shakeTotal -= dampenOverTime * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            shakeTime -= Time.deltaTime;
        } while (shakeTime > 0);

        yield return null;
    }

    public void AddTarget(GameObject focusObject)
    {
        targets.Add(focusObject);
    }
}
