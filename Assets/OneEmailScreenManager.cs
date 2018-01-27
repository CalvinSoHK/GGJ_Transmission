using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles everything for the screen where we have one email
public class OneEmailScreenManager : MonoBehaviour {

    //The text markers in our screen
    public Text SENDER_TEXT, SUBJECT_TEXT, DATE_TEXT;

    //The text for body
    public Text BODY_TEXT;

    //The image for the body image
    public Image BODY_IMAGE;

    //The string values we put in those text boxes
    public string SENDER, SUBJECT, DATE;

    

    void Update()
    {
        //Apply all our strings to the email box
        SENDER_TEXT.text = SENDER;
        SUBJECT_TEXT.text = SUBJECT;
        DATE_TEXT.text = DATE;
    }

    //Setter function for the email
    public void SetEmail(EmailController EMAIL)
    {
        SENDER = EMAIL.SENDER;
        SUBJECT = EMAIL.SUBJECT;
        if(EMAIL.BODY_IMG != null)
        {
            BODY_IMAGE.sprite = EMAIL.BODY_IMG;
        }
        else
        {
            string TEMP = EMAIL.BODY_TEXT;
            TEMP = TEMP.Replace("NEWLINE", "\n");
            BODY_TEXT.text = TEMP;
        }
     
        DATE = EMAIL.DATE;
    }



}
