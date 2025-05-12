using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPLDirector : MonoBehaviour
{
    public int lives, speed, health;

    private static MPLDirector instance;

    public static MPLDirector Instance
    {
        get { return instance; }
    }

    public event Action initGG;

    public bool roomOwner;

    private void Awake()
    {
        //if(Instance == null)
        //{
            instance = this;
            DontDestroyOnLoad(instance);
        //}
        //else
        //    Destroy(gameObject);

        Debug.Log("Awake------- ");

    }

    void OnEnable()
    {
        Debug.Log("MPL DIRECTR Enable");
        Invoke("WaitInitialize", 0.1f);
    }

    void WaitInitialize()
    {
        MPLSdkBridgeController.Instance.StartGameScene += StartGamePlay;
        Session.Instance.Restart += RestartGame;
        Session.Instance.GameEnd += EndGame;
        Session.Instance.OnCreateSessionResult += GetSessionResult;
        Session.Instance.OnDataUpdate += GetData;
    }

    public int currentSet;

    public void GetData(string mplUpdatedData)
    {
        BoxManager.instance.levelIndex = int.Parse(mplUpdatedData);
        Debug.Log("333333 "+ BoxManager.instance.levelIndex);

        initGG?.Invoke();
    }

    private SessionResult GetSessionResult(MPLGameEndReason.GameEndReasons gameEndReason)
    {
        return CreateResult(gameEndReason);
    }

    public void InformSessionEnd(MPLGameEndReason.GameEndReasons gameEndReason, string msg)
    {
        Session.Instance.SubmitResult(CreateResult(gameEndReason));
    }

    private void EndGame(MPLNotificationEventArgs args)
    {
        Debug.Log("MPLDirector : EndGame " + args.message);
        SubmitScore(args.notification);
    }

    public void SubmitScore(MPLGameEndReason.GameEndReasons gameEndReason)
    {
        Debug.Log("MPLDirector : SubmitScore " + gameEndReason.ToString());
        Session.Instance.SubmitResult(CreateResult(gameEndReason));
    }

    private SessionResult CreateResult(MPLGameEndReason.GameEndReasons gameEndReason)
    {
        Debug.Log("MPLDirector : CreateResult " + gameEndReason.ToString());

        SessionInfo sessionInfo = new SessionInfo();

        FruitBreakerSessionResult result = new FruitBreakerSessionResult(
                 sessionInfo.SessionId, sessionInfo.StartTime, 120, Session.Instance.GetScore(), gameEndReason, 120

        );

        return result;

    }

    void StartGamePlay(List<UserProfile> profiles, string roomName)
    {
        Debug.Log("MPLDirectors roomName" + roomName);
        Session.Instance.UpdateScore(0, 0);

        FruitBreakerSessionInfo fruitBreakerGameSessionInfo = Session.Instance.GetSessionInfo<FruitBreakerSessionInfo>();
        
        BoxManager.instance.boxSpawnTimeGap = fruitBreakerGameSessionInfo.boxSpawnTimeGap;
        BoxManager.instance.nextBoxSetSpawnTimeGap = fruitBreakerGameSessionInfo.nextBoxSetSpawnTimeGap;
        BoxManager.instance.decreaseScoreOnWrongMove = fruitBreakerGameSessionInfo.decreaseScoreOnWrongMove;

        Debug.Log(" iiiiii "+ fruitBreakerGameSessionInfo.decreaseScoreOnWrongMove+" "+ fruitBreakerGameSessionInfo.boxSpawnTimeGap+" "+ fruitBreakerGameSessionInfo.nextBoxSetSpawnTimeGap);

        StartGame(profiles, roomName); //Start your gameplay inside this function

        //if(Session.Instance.isRoomOwner)
        //{
        //    roomOwner = true;

        //    BoxManager.instance.levelIndex = UnityEngine.Random.Range(0, BoxManager.instance.level.Length);

        //    Debug.Log("2222222  "+ BoxManager.instance.levelIndex);

        //    Session.Instance.UpdateData(BoxManager.instance.levelIndex.ToString());
        //}


        BoxManager.instance.levelIndex = UniqueRandomNumber.GetRandomNumber(0, BoxManager.instance.level.Length, 1);
    }


    void StartGame(List<UserProfile> profiles, string roomName)
    {
        //If you show the information of the players like their names and profile pictures in your game then you can do the steps below.
        for (int i = 0; i < profiles.Count; i++)
        {
            SetPlayerInfo(profiles[i]);
        }

    }

    void SetPlayerInfo(UserProfile profile)
    {
        //Create the game object for the playerinfo. Let DisplayName(Text) and DisplayPicture(Image) be two child objects of the player object
        Debug.Log("MPLDirector : " + profile.displayName);
        MPLSdkBridgeController.Instance.GetDisplayImage(profile.avatar, (Sprite sprite) =>
        {
            Debug.Log("MPLDirector : " + sprite.name);
            //DisplayPicture.sprite = sprite;
        });
    }

    public void UpdateScore(int prevScore,int totalScore)
    {
        Session.Instance.UpdateScore(prevScore, totalScore);
    }

    void RestartGame()
    {
        Debug.Log("MPL DIRECTR Restart");

        BoxManager.instance.resetToRestart();
        Session.Instance.Restart -= RestartGame;

        //UnityEngine.SceneManagement.SceneManager.LoadScene(MPLController.Instance.sceneName);
    }

    void OnDisable()
    {
        Debug.Log("MPL DIRECTR Disable");

        try
        {
            SubmitScore(MPLGameEndReason.GameEndReasons.APPLICATION_QUIT);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        if(MPLSdkBridgeController.Instance != null)
            MPLSdkBridgeController.Instance.StartGameScene -= StartGamePlay;

        Session.Instance.Restart -= RestartGame;
        Session.Instance.GameEnd -= EndGame;
        Session.Instance.OnCreateSessionResult -= GetSessionResult;
        Session.Instance.OnDataUpdate -= GetData;

    }
}
