using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour {

    //Text element we are related to
    public Text STATUS_TEXT;

    //Our initial status in the first week
    public string WEEK_ONE_STATUS;

    //The possible status values we have
    public List<string> WEEK_TWO_STATUS = new List<string>(), WEEK_THREE_STATUS = new List<string>(), WEEK_FOUR_STATUS = new List<string>();


    public void LoadStatus(int WEEK, float HAPPINESS)
    {
        switch (WEEK)
        {
            case 1: STATUS_TEXT.text = WEEK_ONE_STATUS;
                break;
            case 2:
                if (HAPPINESS < 0)
                {
                    STATUS_TEXT.text = WEEK_TWO_STATUS[0];
                }
                else if (HAPPINESS == 0)
                {
                    STATUS_TEXT.text = WEEK_TWO_STATUS[1];
                }
                else
                {
                    STATUS_TEXT.text = WEEK_TWO_STATUS[2];
                }
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                Debug.Log("Invalid week number: " + WEEK);
                break;
        }
    }
}
