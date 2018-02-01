using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls this entry in our email entries list
public class EntryController : MonoBehaviour {

    //Saves this email for displaying on click
    public EmailController EMAIL;
    public ScrollRect emailScroll;
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
        FindScrollView(GameObject.Find("Desktop"), "Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;        
    }

    //Helper function to find the object since they are inactive
    public GameObject FindScrollView(GameObject parent, string name)
    {
        //Searches through all the children of the given parent and gives us the one with right name
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name && t.tag == "ScrollView2")
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
