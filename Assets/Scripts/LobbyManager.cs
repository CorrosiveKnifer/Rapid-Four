using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Rachael Colaco
/// </summary>
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

    public GameObject player1NonAvailable;
    public GameObject player2NonAvailable;

    public LevelTimer timer;

    int[] playerIndex = new int [2];

    public GameObject allReady;

    int shipOptions = 0;

    bool cancelp1ShipID = false;
    bool cancelp2ShipID = false;

    bool Lobbydone = true;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.GetInstance();
        shipOptions = 2;
        InputManager.GetInstance().setUpLog();
        playerIndex[0] = 0;
        playerIndex[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Lobbydone)
        {
            //if player one is assigned with a controller
            if (InputManager.GetInstance().IsPlayerAssigned(0))
            {
                PlayerSelector(0);
                ChosingPlayerShip(0, playerIndex[0]);
            }
            //if player two is assigned to a controller
            if (InputManager.GetInstance().IsPlayerAssigned(1))
            {
                PlayerSelector(1);
                ChosingPlayerShip(1, playerIndex[1]);
            }

            InputManager.GetInstance().LobbyDetection();

            //displayment on Lobby UI
            displayLobbyUI();

            // if any of the controller disconnected
            CheckControllerDisconnected();

            //canceling the ship Id after confirming they want to cancel their selection
            //for the player one 
            if (cancelp1ShipID == true)
            {
                //setting player one step to no ship id
                InputManager.GetInstance().SetShipToPlayer(0, -1);
                cancelp1ShipID = false;
            }
            //for the player two
            if (cancelp2ShipID == true)
            {
                //setting player two step to no ship id
                InputManager.GetInstance().SetShipToPlayer(1, -1);
                cancelp2ShipID = false;
            }

            //For when the players have both controllers and ship selected
            //Check if both player already selected a ship
            if (InputManager.GetInstance().IsPlayerReady(0) && InputManager.GetInstance().IsPlayerReady(1))
            {
                //displays the READY sign in the middle
                allReady.SetActive(true);

                //only response to the first player inputs 
                //if they use keyboard
                for (int i = 0; i < 2; i++)
                {
                    if (InputManager.GetInstance().GetPlayerControl(i).isKeyboard)
                    {
                        //press start for the keyboard
                        if (InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_W, i))
                        {
                            timer.StartAnim();
                            Destroy(this);
                        }
                    }
                    //if they use gamepad
                    else
                    {
                        //press start to gamepad
                        if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_START, i))
                        {
                            timer.StartAnim();
                            Destroy(this);
                        }
                    }
                }
            }
            else
            {
                allReady.SetActive(false);
            }
        }


    }

    /// <summary>
    /// check if the controller are disconnected
    /// </summary>
    void CheckControllerDisconnected()
    {
        //for the player 1
        if (!InputManager.GetInstance().GetPlayerControl(0).isKeyboard && InputManager.GetInstance().GetPlayerControl(0).gamepad != null)
        {
            if (!InputManager.GetInstance().CheckGampadConnected(0))
            {
                Debug.Log("player one disconnected");
            }
        }
        //for the player 2
        if (!InputManager.GetInstance().GetPlayerControl(1).isKeyboard && InputManager.GetInstance().GetPlayerControl(1).gamepad != null)
        {
            if (!InputManager.GetInstance().CheckGampadConnected(1))
            {
                Debug.Log("player two disconnected");
            }
        }
    }


    /// <summary>
    /// The UI for the lobby displayed
    /// </summary>
    void displayLobbyUI()
    {
        //text instruction displayment depending on control choice
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

        //Selector displayment
        player1Selector.transform.position = player1OptImage[playerIndex[0]].transform.position;
        player2Selector.transform.position = player2OptImage[playerIndex[1]].transform.position;

        player1Ready.transform.position = player1Selector.transform.position;
        player2Ready.transform.position = player2Selector.transform.position;

        //cross displayment
        player1NonAvailable.transform.position = player1OptImage[playerIndex[1]].transform.position;
        player2NonAvailable.transform.position = player2OptImage[playerIndex[0]].transform.position;

        //Show option ships panel FOR PLAYER 1
        if (InputManager.GetInstance().IsPlayerAssigned(0))
        {
            instructTexts[0].SetActive(false);

            playerOneImg.color = Color.yellow;
            player1AssignedPanel.SetActive(false);
            player1OptionPanel.SetActive(true);

            player1Ready.SetActive(false);

            //for keyboard confirmation
            if (InputManager.GetInstance().GetPlayerControl(0).isKeyboard)
            {
                player1KeyPanel.SetActive(true);
                player1GamepadPanel.SetActive(false);
            }
            else //gamepad comfirmation
            {
                player1KeyPanel.SetActive(false);
                player1GamepadPanel.SetActive(true);

            }

        }
        else //show assigned controller panel
        {
            instructTexts[0].SetActive(true);
            player1AssignedPanel.SetActive(true);
            player1OptionPanel.SetActive(false);
            playerOneImg.color = Color.red;
        }

        //Show option ships panel FOR PLAYER 2
        if (InputManager.GetInstance().IsPlayerAssigned(1))
        {

            instructTexts[1].SetActive(false);
            playerTwoImg.color = Color.yellow;
            player2AssignedPanel.SetActive(false);
            player2OptionPanel.SetActive(true);

            player2Ready.SetActive(false);

            //for keyboard confirmation
            if (InputManager.GetInstance().GetPlayerControl(1).isKeyboard)
            {
                player2KeyPanel.SetActive(true);
                player2GamepadPanel.SetActive(false);
            }
            else//gamepad comfirmation
            {
                player2KeyPanel.SetActive(false);
                player2GamepadPanel.SetActive(true);

            }
        }
        else//show assigned controller panel
        {
            instructTexts[1].SetActive(true);
            player2AssignedPanel.SetActive(true);
            player2OptionPanel.SetActive(false);
            playerTwoImg.color = Color.red;
        }

        //displays the READY sign
        //for  player 1
        if (InputManager.GetInstance().IsPlayerReady(0))
        {
            playerOneImg.color = Color.green;
            player1Ready.SetActive(true);

            player2NonAvailable.SetActive(true);

        }
        else
        {
            player2NonAvailable.SetActive(false);
        }
        //for player 2
        if (InputManager.GetInstance().IsPlayerReady(1))
        {
            playerTwoImg.color = Color.green;
            player2Ready.SetActive(true);

            player1NonAvailable.SetActive(true);
        }
        else
        {
            player1NonAvailable.SetActive(false);
        }
    }


    /// <summary>
    /// moves the selector of the player ships when controllers are assigned 
    /// </summary>
    /// <param name="playerID"></param>
    void PlayerSelector(int playerID)
    {
        if (InputManager.GetInstance().GetPlayerControl(playerID).shipID == -1)
        {
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.RIGHT, playerID))
            {
                Debug.Log("right");
                playerIndex[playerID] = Mathf.Clamp(playerIndex[playerID]+1, 0, 1);

            }
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.LEFT, playerID))
            {
                Debug.Log("left");
                playerIndex[playerID] = Mathf.Clamp(playerIndex[playerID]-1, 0, 1);

            }
        }
    }

    

    /// <summary>
    /// Goes through the function to confirm the ship selected
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="Shipindex"></param>
    void ChosingPlayerShip(int playerID, int Shipindex)
    {
        //if no ship have been selected then get confirmation to select ship
        if (InputManager.GetInstance().GetPlayerControl(playerID).shipID == -1)
        {
            //if they have a key controls
            if (InputManager.GetInstance().GetPlayerControl(playerID).isKeyboard)
            {
                //presing space
                if (InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_SPACE, playerID) && !InputManager.GetInstance().IsShipIdTaken(Shipindex))
                {
                    //Debug.Log("player" + playerID + " ship has been confirmed with selection " + Shipindex);
                    InputManager.GetInstance().SetShipToPlayer(playerID, Shipindex);
                    return;
                }
                
            }
            else //otherwise if its gamepad
            {
                if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, playerID) && !InputManager.GetInstance().IsShipIdTaken(Shipindex))
                {
                    //Debug.Log("player" + playerID + " ship has been confirmed with selection " + Shipindex);
                    InputManager.GetInstance().SetShipToPlayer(playerID, Shipindex);
                    return;

                }
                
            }
      
        }
        else //if they have a ship selected, get confirmation to cancel the ship
        {
            //if its a key controls
            if(InputManager.GetInstance().GetPlayerControl(playerID).isKeyboard)
            {
                //pressing escape key
                if(InputManager.GetInstance().GetKeyDown(InputManager.KeyType.KEY_ESC, playerID))
                {
                    if (playerID == 0)
                    {
                        cancelp1ShipID = true; //confirm the cancelation
                    }
                    else
                    {
                        cancelp2ShipID = true; //confirm the cancelation
                    }
                    //Debug.Log("player" + playerID + " ship has been cancel with selection " + Shipindex);
                }

            }
            //otherwise if its gamepad
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
