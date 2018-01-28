using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour {

    //The text we are manipulating
    public Text MESSAGE;

    //The email Manager
    public EmailManager EM;

    //The thresholds for different endings
    public float BAD_ENDING_THRESHOLD = -30f, GOOD_ENDING_THRESHOLD = 40f;

    private void Start()
    {
        //EM = EmailManager.instance;
    }

    //End the game
    public void EndGame()
    {
        //Handle all three endings and the happiness
        MESSAGE.text = FullEnd();
    }

    //Get the entire string we want to display
    public string FullEnd()
    {
        string RET_STRING = "";
        //Handle Family story
        if (EM.STORY_ENDING[0]) //Good ending
        {
            RET_STRING += "Losing Mom was... a bit much for me. But I'm glad I got to talk to her before she left. My relationship with my brother has also gotten better. We'll be heading out to E3 together next month!\n";
        }
        else //Bad ending
        {
            RET_STRING += "Mom's death was a huge surprise. I wasn't even able to meet her before she left. I regret yelling at her before I left... Those were the last words I ever said to her...\n";
        }

        //Handle Date story
        if (EM.STORY_ENDING[1])
        {
            RET_STRING += "Mike was... quite a character. He was a bit creepy... but I'm glad that ordeal is over. Won't ever have to talk to him again.\n";
        }
        else
        {
            RET_STRING += "Mike just kept on messaging me the entire month. He's been saying the most offensive, insensitive drivel I've heard in years. Hopefully he'll lose interest after a while...\n";
        }

        //Handle Job story
        if(EM.STORY_ENDING[2])
        {
            RET_STRING += "My boss was just... too much. He pushed himself onto me despite all my refusal, and while I got away it seems he got me fired at work. There really wasn't anyway it was going to end well. At least I might find a job with a better boss!\n";
        }
        else
        {
            RET_STRING += "I hate my life. My boss is just pushing me around and doing whatever he wants around me. HR won't help, and no one believes me. But if I leave, he said things could get much worse...\n";
        }

        //Handle happiness
        //Worst ending
        if (EM.HAPPINESS < BAD_ENDING_THRESHOLD)
        {
            RET_STRING += "This SmartScreen bot doesn't really seem to do much. Life hasn't gotten any better and I still see all the junk I normally do. It really doesn't work at all. I'm gonna remove it right now, it's almost like vaporware anyway.\n";
        }//Bad ending
        else if(EM.HAPPINESS < GOOD_ENDING_THRESHOLD)
        {
            RET_STRING += "The SmartScreen did a bit of work. I noticed a bit less ads but it still seems like I get scary messages once in a while. I'll probably uninstall it when I remember to.\n";
        }//Good ending
        else
        {
            RET_STRING += "SmartScreen has been doing good work! My emails were mostly good, or at the very least, relevant to my life. Some bad things happened but I can't fault the filter bot for that. Some trash fell through the cracks but hopefully future upgrades will make it better! I love this software.\n";
        }


        return RET_STRING;
    }
}
