using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Image playerOneImg;
    public Image playerTwoImg;

    public GameObject[] instructTexts;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.GetInstance().LobbyDetection();

        //Detecting if the
        if(InputManager.GetInstance().PlayerChoseKeyBoard())
        {
            instructTexts[0].GetComponent<Text>().text = "Press A";
            instructTexts[1].GetComponent<Text>().text = "Press A";
        }
        else
        {
            instructTexts[0].GetComponent<Text>().text = "Press A/Press space";
            instructTexts[1].GetComponent<Text>().text = "Press A/Press space";
        }

        //check if player one assigned
        if(InputManager.GetInstance().IsPlayerAssigned(0))
        {
            instructTexts[0].SetActive(false);
            playerOneImg.color = Color.yellow;
        }
        else
        {
            instructTexts[0].SetActive(true);
            playerOneImg.color = Color.red;
        }

        //check if player two assigned
        if (InputManager.GetInstance().IsPlayerAssigned(1))
        {
            instructTexts[1].SetActive(false);
            playerTwoImg.color = Color.yellow;
        }
        else
        {
            instructTexts[1].SetActive(true);
            playerTwoImg.color = Color.red;
        }

        //check both player ready
        /*
        if (InputManager.GetInstance().IsPlayerAssigned(0) && InputManager.GetInstance().IsPlayerAssigned(1))
        {
            playerOneImg.color = Color.green;
            playerTwoImg.color = Color.green;
            if(InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_START, 0))
            {
                Debug.Log("GAMESTART");
            }
        }
        */

        if(!InputManager.GetInstance().GetPlayerControl(0).isKeyboard && InputManager.GetInstance().GetPlayerControl(0).gamepad !=null)
        {
            if(!InputManager.GetInstance().CheckGampadConnected(0))
            {
                Debug.Log("player one disconnected");
            }
        }

        if (!InputManager.GetInstance().GetPlayerControl(1).isKeyboard && InputManager.GetInstance().GetPlayerControl(1).gamepad != null)
        {
            if (!InputManager.GetInstance().CheckGampadConnected(1))
            {
                Debug.Log("player two disconnected");
            }
        }







    }
}
