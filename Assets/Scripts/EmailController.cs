using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls emails
public class EmailController : MonoBehaviour {

    //Possible type of email
    //Rules for happiness for the below
    //Good A: + R: -
    //GoodAd A: + R: 0
    //Neutral A:0 R: 0
    //Bad: A: - R: +
    //BadAd A: - R: 0
    public enum EmailType { Good, Bad, Neutral, GoodAd, BadAd };
    public EmailType TYPE = EmailType.Neutral;

    //Possible happiness value from this email
    //For bad emails, this is considered negative that value. So set it higher for higher values.
    [Range(0,5)]
    public int VALUE = 0;

    //The email to show up if it is accepted
    public EmailController ACCEPTED_NEXT;

    //The email to show up if it is rejected
    public EmailController REJECTED_NEXT;

    //The date and time this email was sent
    public string DATE;

    //The subject line for this email
    public string SUBJECT;

    //The body of text for this email
    public Sprite BODY;

    //The sender for this email
    public string SENDER;

    //Key for this email
    public int KEY;
}
