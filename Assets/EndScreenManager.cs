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
        string RET_STRING = NAME.GetPlayerName() + "'s Evaluation:\n";
        //Handle Family story
        if (EM.STORY_ENDING[0]) //Good ending
        {
            RET_STRING += "Mom was always the happiest person in the family. She might not have agreed with every way I lived my life, but she always loved me... I see that now. She always wanted me to be a mother... Maybe I'll adopt.\n\n";
        }
        else //Bad ending
        {
            RET_STRING += "Mom was the one to ruin our relationship. She said so many despicable things about Mary, and while I'm not with Mary anymore, it's crazy how backwards she was. I'm a bit sad she's gone, but it's not like she was a big part of my life anyways... I miss her...\n\n";
        }

        //Handle Date story
        if (EM.STORY_ENDING[1])
        {
            RET_STRING += "Turns out, it's hard to really know a person in the beginning. Mike ended up being really creepy, messaging me all the time and trying to go on dates. His rude and inconsiderate behaviour was unacceptable and I am glad to have cut him out of my life.\n\n";
        }
        else
        {
            RET_STRING += "Mike just wouldn't stop messaging me this whole month. He's been saying the most offensive, insensitive drivel I've heard in years; he's almost just as bad as my boss. I sould have stopped messaging him earlier on. His actions have gone too far. Hopefully he'll lose interest after a while...\n\n";
        }

        //Handle Job story
        if(EM.STORY_ENDING[2])
        {
            RET_STRING += "My boss was just... too much. He pushed himself onto me despite all my refusal, and while I finally managed to push him away it seems he got me fired at work. There really wasn't anyway it was going to end well. At least I might find a job with a better boss!\n\n";
        }
        else
        {
            RET_STRING += "What am I going to do? My boss is just pushing me around and doing whatever he wants around me. HR won't help, and no one believes me. But if I leave, he said things could get much worse...\n\n";
        }

        //Handle happiness
        //Worst ending
        if (EM.HAPPINESS < BAD_ENDING_THRESHOLD)
        {
            RET_STRING += "I think this bot did more harm than good. My life has gotten so much worse. I'm not sure if I missed some important emails but the way life has been going its like a storm.";
        }//Bad ending
        else if(EM.HAPPINESS < GOOD_ENDING_THRESHOLD)
        {
            RET_STRING += "The AIDE System did a bit of work. I noticed a bit less ads but it still seems like I get scary messages once in a while. I'll keep this software a while longer.";
        }//Good ending
        else
        {
            RET_STRING += "AIDE System has been doing good work! My emails were mostly good, or at the very least, relevant to my life. I love this software.";
        }


        return RET_STRING;
    }
}
