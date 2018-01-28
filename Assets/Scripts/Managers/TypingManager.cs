using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TypingManager : MonoBehaviour {

    public enum TypingState { Idle, Setup, Typing, Reset, Stop, Delete }; //The states that could occur for typing
    public TypingState currentTypingState;
    public string currentLine, //current line that needs to be displayed based on their happiness
        currentWeekLine; //current line that needs to be displayed based on the week
    public int currentLetterIndex = 0, //current letter that needs to be displayed
        stopIndex = 0, //number of letters that can get displayed before stoping the displaying of text
        deleteIndex = 0, //number of letters that need to be deleted before resuming text display
        currentDeleteCount = 0; //current number of deleted letters
    public Text UI_Text; //UI reference to the Text object

    

    float timeOnStateChange = 0.0f, waitTime = 0.03f;

    public List<string> WeeklyHappinessMessages = new List<string>();
    public List<string> WeeklyMessages = new List<string>();
    public List<int> BadThreshold = new List<int>();
    public List<int> GoodThreshold = new List<int>();

    float happinessThreshold = 50f;

    // Use this for initialization
    void Start () {
        setCurrentState(TypingState.Setup);
	}
	
	// Update is called once per frame
	void Update () {
        
        switch (currentTypingState)
        {
            case TypingState.Idle: //if it needs to be idle
                
                break;

            case TypingState.Setup: //setup the line that needs to be displayed
                //call whatever function that will give us the right line to be displayed
                //call to get the stop index
                //call to get the delete index
                
                float HAPPINESS = EmailManager.instance.HAPPINESS;
                int WEEK = EmailManager.instance.WEEK;
                
                if (WEEK != 0)
                {

                    currentLine = WeeklyMessages[WEEK];
                    if (HAPPINESS <= BadThreshold[WEEK])
                    {
                        currentLine += WeeklyHappinessMessages[WEEK];
                    }
                    else if (HAPPINESS >= GoodThreshold[WEEK])
                    {
                        currentLine += WeeklyHappinessMessages[WEEK + 1];
                    }
                    else
                    {
                        currentLine += WeeklyHappinessMessages[WEEK + 2];
                    }
                    currentLine = currentLine.Replace("NAME", NAME.GetPlayerName());
                }
                else
                {
                    currentLine = "Hey there NAME. Let's get to work ok? I need you to make sure I receive emails from work and my family. If you think some are important, tag them as such. Anything you think is not relevant to me, you can just delete. \n Oky Thanks :)";
                    currentLine = currentLine.Replace("NAME", NAME.GetPlayerName());
                }

                waitTime = 0.03f;
                setCurrentState(TypingState.Typing); //move on to typing
                break;

            case TypingState.Typing: //when the text needs to be displayed letter by letter
                if (currentLetterIndex < currentLine.Length && currentLetterIndex != stopIndex && getStateElapsed() > waitTime)
                {
                    UI_Text.text += currentLine[currentLetterIndex];
                    currentLetterIndex += 1;
                    waitTime += 0.03f;
                }
                else if(stopIndex != -1 && currentLetterIndex == stopIndex)
                {
                    waitTime = 0.03f;
                    setCurrentState(TypingState.Stop);
                }
                
                break;

            case TypingState.Reset: //Once dialogue is no longer displayed and needs to be reset
                currentLetterIndex = 0; //set letter index to 0
                currentLine = ""; //reset the current line being displayed
                if(UI_Text != null)
                {
                    UI_Text.text = "";
                }             
                setCurrentState(TypingState.Idle);
                break;

            case TypingState.Stop: //if there needs to be a pause in the display of the text
                if (getStateElapsed() > 1f)
                {
                    setCurrentState(TypingState.Delete);
                }     
                break;

            case TypingState.Delete: //if there needs a part of the current text to be deleted and replaced 
                if(currentDeleteCount != deleteIndex)
                {
                    UI_Text.text = UI_Text.text.Substring(0, UI_Text.text.Length - 1);
                    currentDeleteCount += 1;                   
                }
                else
                {
                    currentDeleteCount = 0;
                    UI_Text.text += currentLine[currentLetterIndex];
                    currentLetterIndex += 1;
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

    public void resetTyping()
    {
        setCurrentState(TypingState.Reset);
    }

    public void Init()
    {
        setCurrentState(TypingState.Setup);
    }
}
