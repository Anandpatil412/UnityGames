 using System;
using UnityEngine;

[Serializable]
public class FruitBreakerSessionInfo : SessionInfo
{

    /******************************** Global Variables & Constants ********************************/

    /************************************** Public Functions **************************************/

    public int decreaseScoreOnWrongMove;// = 2;
    public float boxSpawnTimeGap;// = 0.8f;
    public float nextBoxSetSpawnTimeGap;// = 1.5f;

    public override string ToString()
    {
        return String.Format("");
        
    }

}
