using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Manages all the things on the all emails screen
public class AllEmailScreenManager : MonoBehaviour {

    //List of all entries below us
    List<EntryController> ENTRY_LIST = new List<EntryController>();

    //Prefab of what an email entry should look like
    public GameObject ENTRY_OBJECT;

    //Header of the given email column. Uses as reference for position
    public GameObject HEADER;

    //Content panel to add our entries into
    public GameObject CONTENT;

    //The x position of all entries
    public float X_POS = 0;

    //The y position of the first entry, and the offset for each after
    public float Y_POS_INIT = 0, Y_POS_DIFF = 30;

    //Color used for when it is accepted
    public Color ACCEPTED, REJECTED, IMPORTANT;

	//Function called when starting that inits the list, given a list of all emails we need to display
    public void InitList(List<EmailController> LIST)
    {

        //Populate the list
        //Save position outside so they spawn in the right place everytime
        Y_POS_DIFF = -Screen.height / 14f;
        Vector3 POS = HEADER.transform.position + new Vector3(0, Y_POS_DIFF, 0);
        foreach(EmailController EMAIL in LIST)
        {
            //Instantiate a new entry
            GameObject TEMP = Instantiate(ENTRY_OBJECT);
            TEMP.transform.SetParent(CONTENT.transform, false);
            TEMP.transform.position = POS;

            //Add the values to the entry
            EntryController ENTRY = TEMP.GetComponent<EntryController>();
            ENTRY.EMAIL = EMAIL;

            //Add this email to our list
            ENTRY_LIST.Add(ENTRY);

            //Add the offset to the pos for the next entry
            POS += new Vector3(0, Y_POS_DIFF, 0);
        }
    }

    //Function called to destroy all entries on display
    public void DestroyList()
    {
        int COUNT = ENTRY_LIST.Count - 1;
        for(int i = COUNT; i >= 0; i--)
        {
            //Debug.Log("destroying index: " + i);
            GameObject TEMP = ENTRY_LIST[i].gameObject;
            ENTRY_LIST.RemoveAt(i);
            Destroy(TEMP);
        }
        //Debug.Log("Entry list has a count of: " + ENTRY_LIST.Count);
    }

    //Function called to update the list, showing the email's current status
    public void UpdateList(List<EmailManager.PlayerAction> ACTION_LIST)
    {
        if(ACTION_LIST.Count == ENTRY_LIST.Count)
        {
            int index = 0;
            foreach (EntryController ENTRY in ENTRY_LIST)
            {
                switch (ACTION_LIST[index])
                {
                    case EmailManager.PlayerAction.Accepted:
                        ENTRY.GetComponent<Image>().color = ACCEPTED;
                        break;
                    case EmailManager.PlayerAction.Declined:
                        ENTRY.GetComponent<Image>().color = REJECTED;
                        break;
                    case EmailManager.PlayerAction.Important:
                        ENTRY.GetComponent<Image>().color = IMPORTANT;
                        break;
                    default:
                        break;
                }
                index++;
            }
        }
        else
        {
            Debug.Log("For some reason action and entry list don't have the same count. Entry: " + ENTRY_LIST.Count + " Action: " + ACTION_LIST.Count);
        }
       
    }
}
