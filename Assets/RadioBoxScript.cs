using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioBoxScript : MonoBehaviour
{
    public int selected = 0;

    private Toggle[] options;
    
    // Start is called before the first frame update
    void Start()
    {
        options = GetComponentsInChildren<Toggle>();
        
        for (int i = 0; i < options.Length; i++)
        {
            options[i].isOn = (i == 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < options.Length; i++)
        {   
            if(options[i].isOn && i != selected)
            {
                options[selected].isOn = false;
                options[selected].interactable = false;
                selected = i;
            }
            options[i].interactable = !options[i].isOn;
        }
    }
}
