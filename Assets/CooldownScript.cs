using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    public int id;
    public GameObject Controller;
    public GameObject KeyBoard;
    public void Update()
    {
        Controller.SetActive(!InputManager.GetInstance().GetPlayerControl(id).isKeyboard);
        KeyBoard.SetActive(InputManager.GetInstance().GetPlayerControl(id).isKeyboard);
    }
    public void SetCooldown(float current, float max)
    {
        GetComponent<Image>().fillAmount = current / max;
    }
}
