using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles everything for the screen where we have one email
public class OneEmailScreenManager : MonoBehaviour {

    //The text markers in our screen
    public Text SENDER_TEXT, SUBJECT_TEXT, BODY_TEXT, DATE_TEXT;

    //The string values we put in those text boxes
    public string SENDER, SUBJECT, BODY, DATE;

    void Update()
    {
        //Apply all our strings to the email box
        SENDER_TEXT.text = SENDER;
        SUBJECT_TEXT.text = SUBJECT;
        BODY_TEXT.text = BODY;
        DATE_TEXT.text = DATE;
    }

    //Setter function for the email
    public void SetEmail(EmailController EMAIL)
    {
        SENDER = EMAIL.SENDER;
        SUBJECT = EMAIL.SUBJECT;
        BODY = EMAIL.BODY;
        DATE = EMAIL.DATE;
    }



}
