using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles emails for the game. Keeps a reference to every single one.
public class EmailManager : MonoBehaviour {

    //List of all emails
    public List<EmailController> EMAIL_LIST = new List<EmailController>();

    //User's happiness value. Saved on the email manager.
    public int HAPPINESS = 0;

	//Function to evaluate all emails
    public void EvaluateEmails()
    {
        //List of emails for the next phase
        List<EmailController> NEW_EMAIL_LIST = new List<EmailController>();

        //For all the emails below
        foreach(EmailController EMAIL in EMAIL_LIST)
        {
            //If the email was ignored or accepted.
            if(EMAIL.ACTION == EmailController.PlayerAction.Ignored || EMAIL.ACTION == EmailController.PlayerAction.Accepted)
            {
                //Add the accepted email to the next round.
                NEW_EMAIL_LIST.Add(EMAIL.ACCEPTED_NEXT);
            }
            else //If the email was rejected
            {
                //Add the rejected email to the next round
                NEW_EMAIL_LIST.Add(EMAIL.REJECTED_NEXT);
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
        //If it is a good email
        switch (EMAIL.TYPE)
        {
            case EmailController.EmailType.Good:
                if (EMAIL.ACTION == EmailController.PlayerAction.Declined)
                {
                    HAPPINESS -= EMAIL.VALUE;
                }
                else //If the email is accpeted or ignored
                {
                    HAPPINESS += EMAIL.VALUE;
                }
                break;

        }
    }
}
