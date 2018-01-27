using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages all the things on the all emails screen
public class AllEmailScreenManager : MonoBehaviour {

    //List of all entries below us
    List<EntryController> ENTRY_LIST = new List<EntryController>();

    //Prefab of what an email entry should look like
    public GameObject ENTRY_OBJECT;

    //Header of the given email column. Uses as reference for position
    public GameObject HEADER;

    //The x position of all entries
    public float X_POS = 0;

    //The y position of the first entry, and the offset for each after
    public float Y_POS_INIT = 0, Y_POS_DIFF = 30;

	//Function called when starting that inits the list, given a list of all emails we need to display
    public void InitList(List<EmailController> LIST)
    {
        
        //Clear every entry on the list
        for(int i = 0; i < ENTRY_LIST.Count; i++)
        {
            //extract it so we can delete it.
            GameObject TEMP = ENTRY_LIST[0].gameObject;
            ENTRY_LIST.RemoveAt(0);
            Destroy(TEMP);
        }

        //Populate the list
        //Save position outside so they spawn in the right place everytime
        Y_POS_DIFF = -Screen.height / 14f;
        Vector3 POS = HEADER.transform.position + new Vector3(0, Y_POS_DIFF, 0);
        foreach(EmailController EMAIL in LIST)
        {
            //Instantiate a new entry
            GameObject TEMP = Instantiate(ENTRY_OBJECT);
            TEMP.transform.SetParent(transform, false);
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
}
