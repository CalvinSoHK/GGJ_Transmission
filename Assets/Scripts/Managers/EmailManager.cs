using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles emails for the game. Keeps a reference to every single one.
public class EmailManager : MonoBehaviour {

    //Make this an instance
    public static EmailManager instance = null;

    //Possible actions taken by the player
    public enum PlayerAction { Ignored, Accepted, Declined, Important };

    //List of all emails
    //[HideInInspector]
    public List<EmailController> EMAIL_LIST = new List<EmailController>();
    public List<PlayerAction> ACTION_LIST = new List<PlayerAction>();

    //List of emails to add depending on the week
    public List<EmailController> WEEK_ONE_LIST = new List<EmailController>(), WEEK_TWO_LIST = new List<EmailController>(), WEEK_THREE_LIST = new List<EmailController>(), WEEK_FOUR_LIST = new List<EmailController>();

    //The week we are on
    public int WEEK = 0;

    //State of email site. Either viewing all emails or the body of an email
    public enum EmailSiteState { AllEmails, OneEmail };
    public EmailSiteState STATE = EmailSiteState.AllEmails;

    //User's happiness value. Saved on the email manager.
    public float HAPPINESS = 0;

    //The story ending bools
    public bool[] STORY_ENDING = new bool[3];

    //The two gameobjects that constitute our two different type of screens
    public GameObject ALL_EMAIL_SCREEN, ONE_EMAIL_SCREEN, DAY_BEGIN_SCREEN, PROCESS_SCREEN, STATUS_BAR, END_SCREEN;

    //The selected email
    EmailController SELECTED_EMAIL;

    //The name of the player
    public string PLAYER_NAME = "my dude";

    //The background manager
    public BackgroundManager BM;

	//Function to evaluate all emails
    public void EvaluateEmails()
    {
        //List of emails for the next phase
        List<EmailController> NEW_EMAIL_LIST = new List<EmailController>();

        //For all the emails below
        int index = 0;
        foreach(PlayerAction ACTION in ACTION_LIST)
        {
            //If the email was ignored or accepted.
            if(ACTION != PlayerAction.Declined)
            {
                //Add the accepted email to the next round.
                if(EMAIL_LIST[index].ACCEPTED_NEXT != null)
                {
                    NEW_EMAIL_LIST.Add(EMAIL_LIST[index].ACCEPTED_NEXT);
                }
             
            }
            else //If the email was rejected
            {
                //Add the rejected email to the next round
                if(EMAIL_LIST[index].REJECTED_NEXT != null)
                {
                    NEW_EMAIL_LIST.Add(EMAIL_LIST[index].REJECTED_NEXT);
                }            
            }

            //Handle happiness for that email
            HandleHappiness(EMAIL_LIST[index]);

            //Handle story consequences
            HandleStory(EMAIL_LIST[index]);

            //Increment index
            index++;
        }

        ALL_EMAIL_SCREEN.GetComponent<AllEmailScreenManager>().DestroyList();

        EMAIL_LIST.Clear();
        foreach(EmailController EMAIL in NEW_EMAIL_LIST)
        {
            EMAIL_LIST.Add(EMAIL);
        }    
    }

    //Helper function that handles story beats
    public void HandleStory(EmailController EMAIL)
    {
        int index = -1;
        switch (EMAIL.STORY_LINK)
        {
            case EmailController.StoryLink.Family:
                index = 0;
                break;
            case EmailController.StoryLink.Date:
                index = 1;
                break;
            case EmailController.StoryLink.Boss:
                index = 2;
                break;
            default: //Not related to any stories
                break;
        }

        //Index
        if(index != -1)
        {
            if (ACTION_LIST[GetIndex(EMAIL)] != PlayerAction.Declined)
            {
                STORY_ENDING[index] = EMAIL.ACCEPTED_EMAIL;
            }
            else
            {
                STORY_ENDING[index] = EMAIL.REJECTED_EMAIL;
            }
        }
     
    }

    //Helper function for evaluate that adds the right happiness value
    //Rules for happiness for the below
    //Good A: + R: -
    //GoodAd A: + R: 0
    //Neutral A:0 R: 0
    //Bad: A: - R: +
    //BadAd A: - R: 0
    public void HandleHappiness(EmailController EMAIL)
    {
        int index = GetIndex(EMAIL);
        //If it is a good email
        switch (EMAIL.TYPE)
        {   
            case EmailController.EmailType.Good:
                //If declined
                if (ACTION_LIST[index] == PlayerAction.Declined)
                {
                    HAPPINESS -= EMAIL.VALUE;
                }
                //If the email is accpeted or ignored
                {
                    if (EMAIL.EMAIL_IMPORTANT && ACTION_LIST[index] == PlayerAction.Important)
                    {
                        HAPPINESS += 2 * EMAIL.VALUE;
                    }
                    else
                    {
                        HAPPINESS += EMAIL.VALUE;
                    }
                }
                break;
            case EmailController.EmailType.GoodAd:
                //If it was not declined, as in accepted or ignored
                if(ACTION_LIST[index] != PlayerAction.Declined)
                {
                    HAPPINESS += EMAIL.VALUE;                 
                }
                break;
            case EmailController.EmailType.Bad:
                if(ACTION_LIST[GetIndex(EMAIL)] != PlayerAction.Declined)
                {
                    if (EMAIL.EMAIL_IMPORTANT && ACTION_LIST[index] == PlayerAction.Important)
                    {
                        HAPPINESS -= 0;
                    }
                    else
                    {
                        HAPPINESS -= EMAIL.VALUE;
                    }
                }
                else //If we declined it
                {
                    //If its important, take double the hit. Else take no hit
                    if (EMAIL.EMAIL_IMPORTANT)
                    {
                        HAPPINESS -= 2 * EMAIL.VALUE;
                    }
                }
                break;
            case EmailController.EmailType.BadAd:
                if(ACTION_LIST[GetIndex(EMAIL)] != PlayerAction.Declined)
                {
                    HAPPINESS -= EMAIL.VALUE;
                }
                break;
            default:
                //Debug.Log("No idea what type of email that was.");
                break;
        }
    }

    //Function that displays the given email
    public void DisplayEmail(EmailController EMAIL)
    {
        //Keep track of which email we want to show
        SELECTED_EMAIL = EMAIL;

        //Set the state to just one email
        SetCurrentState(EmailSiteState.OneEmail);
    }

    //Function that sets the state of the email
    public void SetCurrentState(EmailSiteState TEMP)
    {
        STATE = TEMP;
    }

    //Function that overrides current state with a number
    public void SetCurrentState(int INT_TEMP)
    {
        STATE = (EmailSiteState)(INT_TEMP);
    }

    //Function that sets the selected emails status to Accepted
    public void AcceptEmail()
    {
        //Sets the email to accepted
        ACTION_LIST[GetIndex(SELECTED_EMAIL)] = PlayerAction.Accepted;
    }

    //Function that sets the selected emails status to Declined
    public void RejectEmail()
    {
        //Sets the email to rejected
        ACTION_LIST[GetIndex(SELECTED_EMAIL)] = PlayerAction.Declined;
    }

    //Function that sets the selected emails status to Important
    public void AcceptAndMarkImportantEmail()
    {
        ACTION_LIST[GetIndex(SELECTED_EMAIL)] = PlayerAction.Important;
    }

    //Helper function that gets the index of a given email
    public int GetIndex(EmailController EMAIL)
    {
        for(int i = 0; i < EMAIL_LIST.Count; i++)
        {
            if(EMAIL.KEY == EMAIL_LIST[i].KEY)
            {
                return i; 
            }
        }
        return -1;
    }

    //Init function for the action list
    public void InitActionList()
    {
        ACTION_LIST.Clear();
        int count = 0;
        foreach(EmailController EMAIL in EMAIL_LIST)
        {
            //Debug.Log("Count: " + count);
            ACTION_LIST.Add(PlayerAction.Ignored);
            count++;
        }
    }

    //Load in extra emails for a given week
    public void LoadEmails()
    {
        //Add emails for a given week to the list
        switch (WEEK)
        {
            case 1:
                foreach(EmailController EMAIL in WEEK_ONE_LIST)
                {
                    EMAIL_LIST.Add(EMAIL);
                }
                break;
            case 2:
                foreach (EmailController EMAIL in WEEK_TWO_LIST)
                {
                    EMAIL_LIST.Add(EMAIL);
                }
                break;
            case 3:
                foreach (EmailController EMAIL in WEEK_THREE_LIST)
                {
                    EMAIL_LIST.Add(EMAIL);
                }
                break;
            case 4:
                foreach (EmailController EMAIL in WEEK_FOUR_LIST)
                {
                    EMAIL_LIST.Add(EMAIL);
                }
                break;
        }
    }

    //Load in a possible status
    public void LoadStatus()
    {
        STATUS_BAR.GetComponent<StatusController>().LoadStatus(WEEK, HAPPINESS);
    }

    //Set the game state manager
    public void SetGameStateManager(int i)
    {
        //Set the state to the given state
        GameStateManager.SetCurrentState((GameStateManager.GameState)i);
    }

    //Make this script a singleton
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    //Handle states
    private void Update()
    {
        //Get the game state
        GameStateManager.GameState GAME_STATE = GameStateManager.STATE;

        //Handle each game state
        switch (GAME_STATE)
        {
            case GameStateManager.GameState.Init:
                //Increment our week number
                WEEK++;

                //Load any emails taht start on this week
                LoadEmails();

                //Init the actions list, set our day begin screen off, then change states.
                InitActionList();

                //Load in a status and turn it on
                LoadStatus();
                STATUS_BAR.SetActive(true);

                DAY_BEGIN_SCREEN.SetActive(false);
                ALL_EMAIL_SCREEN.GetComponent<AllEmailScreenManager>().InitList(EMAIL_LIST);
                GameStateManager.SetCurrentState(GameStateManager.GameState.PlayerInput);
                break;
            case GameStateManager.GameState.PlayerInput:
                //Handle the state of player input
                switch (STATE)
                {
                    case EmailSiteState.AllEmails:
                        //If this isn't on, turn it on.
                        if (!ALL_EMAIL_SCREEN.activeSelf)
                        {
                            DAY_BEGIN_SCREEN.SetActive(false);
                            PROCESS_SCREEN.SetActive(false);
                            ONE_EMAIL_SCREEN.SetActive(false);
                            BM.ApplyColors(BackgroundManager.BackgroundStates.Inbox);
                            ALL_EMAIL_SCREEN.SetActive(true);
                        }

                        //Update the list
                        ALL_EMAIL_SCREEN.GetComponent<AllEmailScreenManager>().UpdateList(ACTION_LIST);
                        break;
                    case EmailSiteState.OneEmail:
                        ONE_EMAIL_SCREEN.GetComponent<OneEmailScreenManager>().SetEmail(SELECTED_EMAIL);
                        //If this isn't on, turn it on, and turn the other one off.
                        if (!ONE_EMAIL_SCREEN.activeSelf)
                        {
                            DAY_BEGIN_SCREEN.SetActive(false);
                            PROCESS_SCREEN.SetActive(false);
                            ALL_EMAIL_SCREEN.SetActive(false);
                            BM.ApplyColors(BackgroundManager.BackgroundStates.Email);
                            ONE_EMAIL_SCREEN.SetActive(true);
                        }
                        break;
                    default:
                        Debug.Log("Error, shouldn't happen. Email site state is invalid.");
                        break;
                }
                break;
            case GameStateManager.GameState.Processing:
                //Disable relevent screens and enable the right one.
                if (!PROCESS_SCREEN.activeSelf)
                {
                    DAY_BEGIN_SCREEN.SetActive(false);
                    ALL_EMAIL_SCREEN.SetActive(false);
                    ONE_EMAIL_SCREEN.SetActive(false);
                    STATUS_BAR.SetActive(false);
                    BM.ApplyColors(BackgroundManager.BackgroundStates.Sleep);
                    PROCESS_SCREEN.SetActive(true);
                }
                break;
            case GameStateManager.GameState.Waiting:
                if (!DAY_BEGIN_SCREEN.activeSelf)
                {
                    //If we just finished week 4, we should end the game, else do the default
                    if(WEEK != 4)
                    {
                        ALL_EMAIL_SCREEN.SetActive(false);
                        ONE_EMAIL_SCREEN.SetActive(false);
                        PROCESS_SCREEN.SetActive(false);
                        BM.ApplyColors(BackgroundManager.BackgroundStates.Message);
                        DAY_BEGIN_SCREEN.SetActive(true);          
                        DAY_BEGIN_SCREEN.GetComponent<DayBeginScreenManager>().InitScreen();
                    }
                    else
                    {
                        //Do the final message thing
                        ALL_EMAIL_SCREEN.SetActive(false);
                        ONE_EMAIL_SCREEN.SetActive(false);
                        PROCESS_SCREEN.SetActive(false);
                        DAY_BEGIN_SCREEN.SetActive(false);
                        BM.ApplyColors(BackgroundManager.BackgroundStates.End);
                        END_SCREEN.SetActive(true);

                        END_SCREEN.GetComponent<EndScreenManager>().EndGame();
                    }
                   
                }
                break;
            default:
                Debug.Log("Invalid game state!");
                break;
        }
    }
}
