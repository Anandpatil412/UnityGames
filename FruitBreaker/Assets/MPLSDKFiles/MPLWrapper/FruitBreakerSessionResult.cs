using System;

public class FruitBreakerSessionResult : SessionResult
{
    /******************************** Global Variables & Constants ********************************/
    
    public long GameplayDuration;
    public int score;
    
    /**************************************** Constructors ****************************************/
    public FruitBreakerSessionResult(string sessionId, long startTime, long endTime, int score, MPLGameEndReason.GameEndReasons gameEndReason, long gameplayDuration)
    : base(sessionId, startTime, endTime, score, gameEndReason)
    {
        GameplayDuration = gameplayDuration;
        this.score = score;

    }
}
