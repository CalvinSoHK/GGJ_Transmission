using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartupTextDisplayer : MonoBehaviour {
    public enum TypingState { Idle, Setup, Typing, PlayerTyping, Reset, Stop }; //The states that could occur for typing
    public TypingState currentTypingState;
    public string currentLine; //current line that needs to be displayed
    int currentLetterIndex = 0; //current letter that needs to be displayed       
    public Text UI_Text; //UI reference to the Text object
    public int[] stopIndex;
    public int[] lineBreakIndex;
    int currentLineBreak = 0;
    int currentStop = 0;

    int currentLength = 0, maxLength = 30;

    string[] possibleLC = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "space" }; //all possible lower case letters
    string[] possibleUC = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}; //all possible upper case letters

    public string AIDE_Name = "";

    float timeOnStateChange = 0.0f, waitTime = 0.01f;

    public GameObject UI_1, UI_2, UI_3;

    // Use this for initialization
    void Start()
    {
        setCurrentState(TypingState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentTypingState)
        {
            case TypingState.Idle: //if it needs to be idle

                break;

            case TypingState.Setup: //setup the line that needs to be displayed
                //call whatever function that will give us the right line to be displayed
                //call to get the stop index
                //call to get the delete index
                setCurrentState(TypingState.Typing); //move on to typing
                break;

            case TypingState.Typing: //when the text needs to be displayed letter by letter
                if (currentLetterIndex < currentLine.Length && getStateElapsed() > waitTime)
                {
                    if (currentStop < stopIndex.Length && currentLetterIndex == stopIndex[currentStop])
                    {
                        waitTime = 0.01f;
                        currentStop += 1;
                        setCurrentState(TypingState.Stop);
                        break;
                    }
                    else if (currentLineBreak < lineBreakIndex.Length && currentLetterIndex == lineBreakIndex[currentLineBreak])
                    {
                        currentLineBreak += 1;
                        UI_Text.text += "\n";
                    }
                    UI_Text.text += currentLine[currentLetterIndex];
                    currentLetterIndex += 1;
                    waitTime += 0.01f;
                }

                if(currentLetterIndex == currentLine.Length - 1)
                {
                    waitTime = 0.01f;
                    UI_Text.text += currentLine[currentLetterIndex];
                    UI_Text.text += "\n";
                    setCurrentState(TypingState.PlayerTyping);
                }
                

                break;

            case TypingState.PlayerTyping:
                if(currentLength < maxLength)
                {
                    if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) //if the player presses shift, it should be upper case
                    {
                        for(int i = 0; i < possibleUC.Length; i++)
                        {
                            if (Input.GetKeyDown(possibleLC[i])) //check if the player is pressing that specific key
                            {
                                AIDE_Name += possibleUC[i];
                                UI_Text.text += possibleUC[i]; // add the character associated to the key that was pressed to the end of the word
                                currentLength += 1;
                                break; //break out of the for loop                                                 

                            }
                        }
                    }
                    else //otherwise these should be lowercase letters.
                    {
                        for(int i = 0; i < possibleLC.Length; i++)
                        {
                            if (Input.GetKeyDown(possibleLC[i])) //check if the player is pressing that specific key
                            {
                                if (possibleLC[i].Trim().Equals("space")) //checks if the player pressed the space button
                                {
                                    AIDE_Name += " ";
                                    UI_Text.text += " "; //add a space to the current word
                                    currentLength += 1;
                                    break;
                                }
                                else //if the key pressed is not space
                                {
                                    AIDE_Name += possibleLC[i];
                                    UI_Text.text += possibleLC[i]; // add the character associated to the key that was pressed to the end of the word
                                    currentLength += 1;
                                    break; //break out of the for loop
                                }

                            }
                        }
                    }

                }
                if(currentLength > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        AIDE_Name = AIDE_Name.Substring(0, AIDE_Name.Length - 1);
                        UI_Text.text = UI_Text.text.Substring(0, UI_Text.text.Length - 1);
                        currentLength -= 1;
                    }
                }
                if(Input.GetKeyDown(KeyCode.Return) && currentLength > 0)
                {
                    NAME.SetPlayerName(AIDE_Name);
                    SceneManager.LoadScene(1);
                }
                break;
            case TypingState.Reset: //Once dialogue is no longer displayed and needs to be reset
                currentLetterIndex = 0; //set letter index to 0
                currentLine = ""; //reset the current line being displayed
                setCurrentState(TypingState.Idle);
                break;

            case TypingState.Stop: //if there needs to be a pause in the display of the text
                if (getStateElapsed() > 3f)
                {
                    setCurrentState(TypingState.Typing);
                }
                break;            
        }
    }

    /// <summary>
    /// update the current Typing State.
    /// </summary>
    /// <param name="newState"></param>
    public void setCurrentState(TypingState newState)
    {
        currentTypingState = newState;
        timeOnStateChange = Time.time;
    }

    /// <summary>
    /// check how much time has passed since the last state change.
    /// </summary>
    /// <returns></returns>
    public float getStateElapsed()
    {
        return Time.time - timeOnStateChange;
    }

    public void setToTyping()
    {
        setCurrentState(TypingState.Typing);
        DisableMenuUI();
    }

    public void DisableMenuUI()
    {
        UI_1.SetActive(false);
        UI_2.SetActive(false);
        UI_3.SetActive(false);
    }
}
