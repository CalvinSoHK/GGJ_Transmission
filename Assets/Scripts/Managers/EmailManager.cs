using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles emails for the game. Keeps a reference to every single one.
public class EmailManager : MonoBehaviour {

    //Make this an instance
    public static EmailManager instance = null;

    //Possible actions taken by the player
    public enum PlayerAction { Ignored, Accepted, Declined };

    //List of all emails
    public List<EmailController> EMAIL_LIST = new List<EmailController>();
    public List<PlayerAction> ACTION_LIST = new List<PlayerAction>();

    //State of email site. Either viewing all emails or the body of an email
    public enum EmailSiteState { AllEmails, OneEmail };
    public EmailSiteState STATE = EmailSiteState.AllEmails;

    //User's happiness value. Saved on the email manager.
    public int HAPPINESS = 0;

    //The two gameobjects that constitute our two different type of screens
    public GameObject ALL_EMAIL_SCREEN, ONE_EMAIL_SCREEN, DAY_BEGIN_SCREEN;

    //The selected email
    EmailController SELECTED_EMAIL;

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
            if(ACTION == PlayerAction.Ignored || ACTION == PlayerAction.Accepted)
            {
                //Add the accepted email to the next round.
                NEW_EMAIL_LIST.Add(EMAIL_LIST[index].ACCEPTED_NEXT);
            }
            else //If the email was rejected
            {
                //Add the rejected email to the next round
                NEW_EMAIL_LIST.Add(EMAIL_LIST[index].REJECTED_NEXT);
            }

            //Handle happiness for that email
            HandleHappiness(EMAIL_LIST[index]);
        }

        //Set the new email list
        EMAIL_LIST.Clear();
        foreach(EmailController EMAIL in NEW_EMAIL_LIST)
        {
            EMAIL_LIST.Add(EMAIL);
        }
        index++;
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
                else //If the email is accpeted or ignored
                {
                    HAPPINESS += EMAIL.VALUE;
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
                if(ACTION_LIST[GetIndex(EMAIL)] == PlayerAction.Declined)
                {
                    HAPPINESS += EMAIL.VALUE;
                }
                else
                {
                    HAPPINESS -= EMAIL.VALUE;
                }
                break;
            case EmailController.EmailType.BadAd:
                if(ACTION_LIST[GetIndex(EMAIL)] != PlayerAction.Declined)
                {
                    HAPPINESS -= EMAIL.VALUE;
                }
                break;
            default:
                Debug.Log("No idea what type of email that was.");
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
        //Debug.Log(GetIndex(SELECTED_EMAIL));
        ACTION_LIST[GetIndex(SELECTED_EMAIL)] = PlayerAction.Accepted;
    }

    //Function that sets the selected emails status to Declined
    public void RejectEmail()
    {
        //Sets the email to rejected
        ACTION_LIST[GetIndex(SELECTED_EMAIL)] = PlayerAction.Declined;
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

        foreach(EmailController EMAIL in EMAIL_LIST)
        {
            ACTION_LIST.Add(PlayerAction.Ignored);
        }
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
        //Debug input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ALL_EMAIL_SCREEN.GetComponent<AllEmailScreenManager>().InitList(EMAIL_LIST);
        }

        //Get the game state
        GameStateManager.GameState GAME_STATE = GameStateManager.STATE;

        //Handle each game state
        switch (GAME_STATE)
        {
            case GameStateManager.GameState.Init:
                //Init the actions list, set our day begin screen off, then change states.
                InitActionList();
                DAY_BEGIN_SCREEN.SetActive(false);
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
                            ONE_EMAIL_SCREEN.SetActive(false);
                            ALL_EMAIL_SCREEN.SetActive(true);
                            ALL_EMAIL_SCREEN.GetComponent<AllEmailScreenManager>().InitList(EMAIL_LIST);
                        }
                        break;
                    case EmailSiteState.OneEmail:
                        ONE_EMAIL_SCREEN.GetComponent<OneEmailScreenManager>().SetEmail(SELECTED_EMAIL);
                        //If this isn't on, turn it on, and turn the other one off.
                        if (!ONE_EMAIL_SCREEN.activeSelf)
                        {
                            ALL_EMAIL_SCREEN.SetActive(false);
                            ONE_EMAIL_SCREEN.SetActive(true);
                        }
                        break;
                    default:
                        Debug.Log("Error, shouldn't happen. Email site state is invalid.");
                        break;
                }
                break;
            case GameStateManager.GameState.Processing:
                break;
            case GameStateManager.GameState.Waiting:
                break;
            default:
                Debug.Log("Invalid game state!");
                break;
        }
    }
}
