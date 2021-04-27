using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    public float lifetime = 1.0f;
    AudioAgent audioAgent;

    private void Start()
    {
        audioAgent = GetComponent<AudioAgent>();
        if (audioAgent != null)
            audioAgent.PlaySoundEffect(audioAgent.AudioClips[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
