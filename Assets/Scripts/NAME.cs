using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NAME {

    public static string name = "my dude";

    public static void SetPlayerName(string Name)
    {
        name = Name;
    }

    public static string GetPlayerName()
    {
        return name;
    }
}
