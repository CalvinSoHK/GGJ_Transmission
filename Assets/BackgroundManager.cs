using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Changes colors 
public class BackgroundManager : MonoBehaviour {

    //The images we are manipulating
    public Image BACKGROUND, OVERLAY;

    //Color for each state
    public Color WAKE_COLOR, INBOX_COLOR, EMAIL_COLOR, SLEEP_COLOR, MESSAGE_COLOR, END_COLOR;

    //Opacity for each state for the overlay
    [Range(0, 1)]
    public float WAKE_OP, INBOX_OP, EMAIL_OP, SLEEP_OP, MESSAGE_OP, END_OP;

    //Enum for the states we can use
    public enum BackgroundStates { Waking, Inbox, Email, Sleep, Message, End };
    
    //Applies a color based on the given state
    public void ApplyColors(BackgroundStates STATE)
    {
        //Get the right color and opacity
        Color APPLIED = new Color();
        float OPACITY = 0f;
        switch (STATE)
        {
            case BackgroundStates.Waking:
                APPLIED = WAKE_COLOR;
                OPACITY = WAKE_OP;
                break;
            case BackgroundStates.Inbox:
                APPLIED = INBOX_COLOR;
                OPACITY = INBOX_OP;
                break;
            case BackgroundStates.Email:
                APPLIED = EMAIL_COLOR;
                OPACITY = EMAIL_OP;
                break;
            case BackgroundStates.Sleep:
                APPLIED = SLEEP_COLOR;
                OPACITY = SLEEP_OP;
                break;
            case BackgroundStates.Message:
                APPLIED = MESSAGE_COLOR;
                OPACITY = MESSAGE_OP;
                break;
            case BackgroundStates.End:
                APPLIED = END_COLOR;
                OPACITY = END_OP;
                break;
            default:
                Debug.Log("Invalid state");
                break;
        }

        //Apply the color and opacity
        BACKGROUND.color = APPLIED;
        OVERLAY.color = new Color(OVERLAY.color.r, OVERLAY.color.g, OVERLAY.color.b, OPACITY);
    }

    //The only one we can't call from our state machine
    public void ApplyWakingColor()
    {
        ApplyColors(BackgroundStates.Waking);
    }
}
