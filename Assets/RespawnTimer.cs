using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour
{
    public GameObject[] Info;
    public Text[] Timers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableTimer(int _index, bool _enable)
    {
        Info[_index].SetActive(_enable);
    }

    public void UpdateTimer(int _index, float _time)
    {
        int time = (int)Mathf.Ceil(_time);
        Timers[_index].text = time.ToString();
    }
}
