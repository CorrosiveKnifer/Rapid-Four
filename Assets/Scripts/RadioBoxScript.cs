using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Michael Jordan
/// </summary>
public class RadioBoxScript : MonoBehaviour
{
    public int selected;

    public Toggle[] options;
    
    // Start is called before the first frame update
    void Start()
    {
        options = GetComponentsInChildren<Toggle>();
        
        for (int i = 0; i < options.Length; i++)
        {
            options[i].isOn = (i == selected);
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

    public void SetSelected(int index)
    {
        index = Mathf.Clamp(index, 0, options.Length);
        options[selected].isOn = false;
        options[selected].interactable = false;
        options[index].isOn = true;
        options[index].interactable = true;
        selected = index;
    }
}
