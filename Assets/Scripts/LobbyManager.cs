using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Image playerOneImg;
    public Image playerTwoImg;

    public GameObject[] instructTexts;

    public GameObject[] player1Option;
    public GameObject[] player2Option;
    
    public GameObject player1AssignedPanel;
    public GameObject player2AssignedPanel;

    public GameObject player1OptionPanel;
    public GameObject player2OptionPanel;

    public GameObject player1Ready;
    public GameObject player2Ready;

    public GameObject player1Selector;
    public GameObject player2Selector;

    public GameObject player1KeyPanel;
    public GameObject player2KeyPanel;

    public GameObject player1GamepadPanel;
    public GameObject player2GamepadPanel;


    public GameObject[] player1OptImage;
    public GameObject[] player2OptImage;
 

    

    public int P1_index = 0;

    public int P2_index = 0;

    public GameObject allReady;

    int shipOptions = 0;

    bool cancelp1ShipID = false;
    bool cancelp2ShipID = false;

    bool Lobbydone = true;

    // Start is called before the first frame update
    void Start()
    {
        shipOptions = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Lobbydone)
        {
            //if player one is assigned with a controller
            if (InputManager.GetInstance().IsPlayerAssigned(0))
            {
                ChosingPlayerShip(0, P1_index);
                PlayerOneSelecting();
            }
            //if player two is assigned to a controller
            if (InputManager.GetInstance().IsPlayerAssigned(1))
            {
                ChosingPlayerShip(1, P2_index);
                PlayerTwoSelecting();
            }

            InputManager.GetInstance().LobbyDetection();

            //Debug.Log("PlayerOne has " + InputManager.GetInstance().GetPlayerControl(0).shipID);
           // Debug.Log("PlayerTwo has " + InputManager.GetInstance().GetPlayerControl(1).shipID);

            //displayment on Lobby ---------------------------------------------------------------------------
            if (InputManager.GetInstance().PlayerChoseKeyBoard())
            {
                instructTexts[0].GetComponent<Text>().text = "Press A";
                instructTexts[1].GetComponent<Text>().text = "Press A";
            }
            else
            {
                instructTexts[0].GetComponent<Text>().text = "Press A/Press space";
                instructTexts[1].GetComponent<Text>().text = "Press A/Press space";
            }
            
            player1Selector.transform.position = player1OptImage[P1_index].transform.position;
            player2Selector.transform.position = player2OptImage[P2_index].transform.position;

            //check if player one assigned
            if (InputManager.GetInstance().IsPlayerAssigned(0))
            {
                instructTexts[0].SetActive(false);
                
                playerOneImg.color = Color.yellow;
                player1AssignedPanel.SetActive(false);
                player1OptionPanel.SetActive(true);

                player1Ready.SetActive(false);
                if (InputManager.GetInstance().GetPlayerControl(0).isKeyboard)
                {
                    player1KeyPanel.SetActive(true);
                    player1GamepadPanel.SetActive(false);
                }
                else
                {
                    player1KeyPanel.SetActive(false);
                    player1GamepadPanel.SetActive(true);

                }
               
            }
            else
            {
                instructTexts[0].SetActive(true);
                player1AssignedPanel.SetActive(true);
                player1OptionPanel.SetActive(false);
                playerOneImg.color = Color.red;
            }

            //check if player two assigned
            if (InputManager.GetInstance().IsPlayerAssigned(1))
            {
                instructTexts[1].SetActive(false);
                playerTwoImg.color = Color.yellow;
                player2AssignedPanel.SetActive(false);
                player2OptionPanel.SetActive(true);

                player2Ready.SetActive(false);

                if (InputManager.GetInstance().GetPlayerControl(1).isKeyboard)
                {
                    player2KeyPanel.SetActive(true);
                    player2GamepadPanel.SetActive(false);
                }
                else
                {
                    player2KeyPanel.SetActive(false);
                    player2GamepadPanel.SetActive(true);

                }
            }
            else
            {
                instructTexts[1].SetActive(true);
                player2AssignedPanel.SetActive(true);
                player2OptionPanel.SetActive(false);
                playerTwoImg.color = Color.red;
            }

            if(InputManager.GetInstance().IsPlayerReady(0))
            {
                playerOneImg.color = Color.green;
                player1Ready.SetActive(true);
            }
          
            if (InputManager.GetInstance().IsPlayerReady(1))
            {
                playerTwoImg.color = Color.green;
                player2Ready.SetActive(true);
            }

            /*

            if (InputManager.GetInstance().IsPlayerAssigned(0) && InputManager.GetInstance().IsPlayerAssigned(1))
            {
                playerOneImg.color = Color.green;
                playerTwoImg.color = Color.green;
            }
            */



            if (!InputManager.GetInstance().GetPlayerControl(0).isKeyboard && InputManager.GetInstance().GetPlayerControl(0).gamepad != null)
            {
                if (!InputManager.GetInstance().CheckGampadConnected(0))
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

            if (cancelp1ShipID == true)
            {
                //setting player one step to no ship id
                InputManager.GetInstance().SetShipToPlayer(0, 0);
                cancelp1ShipID = false;
            }
            if (cancelp2ShipID == true)
            {
                //setting player one step to no ship id
                InputManager.GetInstance().SetShipToPlayer(1, 0);
                cancelp2ShipID = false;
            }

            //Check if both player already selected a ship
            if (InputManager.GetInstance().IsPlayerReady(0) && InputManager.GetInstance().IsPlayerReady(1))
            {
                //displays the READY sign in the middle
                allReady.SetActive(true);

                //only response to the first player inputs 
                //if they use keyboard
                if (InputManager.GetInstance().GetPlayerControl(0).isKeyboard)
                {
                    //press start for the keyboard
                    if (InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_W, 0))
                    {
                        Debug.Log("GAMESTART");
                        Lobbydone = true;
                    }
                }
                //if they use gamepad
                else
                {
                    //press start to gamepad
                    if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_START, 0))
                    {
                        Lobbydone = true;
                        Debug.Log("GAMESTART");
                    }
                }
            }
            else
            {
                allReady.SetActive(false);
            }
        }


    }
    int MoveIndex(int direction,int CurrentIndex)
    {
        CurrentIndex += direction;

        //if it reaches the far left, move it to the far right
        if (CurrentIndex == -1)
        {
            return CurrentIndex = 0;
        }
        //if it reaches the far right, move it to the far left
        if (CurrentIndex == shipOptions)
        {
            return CurrentIndex = shipOptions-1;
        }
        return CurrentIndex;


    }
    void PlayerOneSelecting()
    {
        if (InputManager.GetInstance().GetPlayerControl(0).shipID == 0)
        {
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.RIGHT, 0))
            {
                Debug.Log("right");
                P1_index=MoveIndex(1, P1_index);

            }
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.LEFT, 0))
            {
                Debug.Log("left");
                P1_index=MoveIndex(-1, P1_index);

            }
        }
    }
    void PlayerTwoSelecting()
    {
        if (InputManager.GetInstance().GetPlayerControl(1).shipID == 0)
        {
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.RIGHT, 1))
            {
                Debug.Log("right");
                P2_index = MoveIndex(1, P2_index);

            }
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.LEFT, 1))
            {
                Debug.Log("left");
                P2_index = MoveIndex(-1, P2_index);
            }
        }
    }

    void ChosingPlayerShip(int playerID, int Shipindex)
    {
        //Debug.Log(InputManager.GetInstance().GetPlayerControl(playerID).shipID);
        if (InputManager.GetInstance().GetPlayerControl(playerID).shipID == 0)
        {
            if (InputManager.GetInstance().GetPlayerControl(playerID).isKeyboard)
            {
                if (InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_SPACE, playerID))
                {
                    //Debug.Log("player" + playerID + " ship has been confirmed with selection " + Shipindex);
                    InputManager.GetInstance().SetShipToPlayer(playerID, Shipindex+1);
                    return;
                }
                
            }
            else
            {
                if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, playerID))
                {
                    //Debug.Log("player" + playerID + " ship has been confirmed with selection " + Shipindex);
                    InputManager.GetInstance().SetShipToPlayer(playerID, Shipindex+1);
                    return;

                }
                
            }
            /*
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.RIGHT, playerID))
            {
                Debug.Log("right");
                MoveIndex(1, Shipindex);


                return;
            }
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.LEFT, playerID))
            {
                Debug.Log("left");
                MoveIndex(-1, Shipindex);
                return;
            }
            */
        }
        else
        {
            if(InputManager.GetInstance().GetPlayerControl(playerID).isKeyboard)
            {
                if(InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_ESC, playerID))
                {
                    if (playerID == 0)
                    {
                        cancelp1ShipID = true;
                    }
                    else
                    {
                        cancelp2ShipID = true;
                    }
                    //Debug.Log("player" + playerID + " ship has been cancel with selection " + Shipindex);
                }

            }
            else if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_EAST, playerID))
            {
                if(playerID ==0)
                {
                    cancelp1ShipID = true;
                }
                else
                {
                    cancelp2ShipID = true;
                }
                //Debug.Log("player" + playerID + " ship has been cancel with selection " + Shipindex);
            }
        }

    }




}
