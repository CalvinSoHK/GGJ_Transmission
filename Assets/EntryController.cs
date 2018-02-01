using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls this entry in our email entries list
public class EntryController : MonoBehaviour {

    //Saves this email for displaying on click
    public EmailController EMAIL;

    //Holds the UI objects we are going to be sending our info to.
    public Text SENDER_TEXT, SUBJECT_TEXT, DATE_TEXT;

    // Update is called once per frame
    void Update() {
        SENDER_TEXT.text = EMAIL.SENDER;
        SUBJECT_TEXT.text = EMAIL.SUBJECT;
        DATE_TEXT.text = EMAIL.DATE;
    }

    //Function that displays the given email
    public void DisplayThisEmail()
    {
        GameObject.Find("GameManager").GetComponent<EmailManager>().DisplayEmail(EMAIL);
    }
}
