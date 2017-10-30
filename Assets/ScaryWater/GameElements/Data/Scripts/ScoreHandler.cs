using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoreType
{
    None = 0,
    NormalJump,
    HighAndLongJump,
    StartCoin,
    Butterfly,
    Firefly,
    Bug,
    Worm,
    Insect,
    Cattail,
    Pickerelweed,
    Iris,
    Horsetail,
    Kingfisher,
    Dragonfly,
    Spider,
    Duck,
    SnakeJumpingOver,
    SnakeKilling,
    SnakeEscaping,
    GiantWaterBugJumpingOver,
    GiantWaterBugKilling,
    GiantWaterBugEscaping,
    BlueHeronJumpingOver,
    BlueHeronKilling,
    BlueHeronEscaping,
    CrabJumpingOver,
    CrabKilling,
    CrabEscaping,
    VenusFlytrapJumpingOver,
    VenusFlytrapKilling,
    VenusFlytrapEscaping,
    RaccoonJumpingOver,
    RaccoonKilling,
    RaccoonEscaping,
    GreatEgretJumpingOver,
    GreatEgretKilling,
    GreatEgretEscaping,
    BatJumpingOver,
    BatKilling,
    BatEscaping,
    HawkJumpingOver,
    HawkKilling,
    HawkEscaping
}

public class ScoreHandler : MonoBehaviour 
{
    static ScoreHandler mInstance;
    eScoreType meScoreType = eScoreType.None;
    Dictionary<eScoreType, int> mDictOfScore = new Dictionary<eScoreType, int>();
    int miCurrentScore;
    int miTempCurrentScore;

    public delegate void ScoreEvent(eScoreType type);
    public static ScoreEvent _OnScoreEventCallback;

    void Awake()
    {
        mInstance = this;
        ScoreDictionaryDataInitializers();
    }

    void OnEnable()
    {
        _OnScoreEventCallback += ScoreUpdateHandler;    
    }

    void OnDisable()
    {
        if (_OnScoreEventCallback != null)
            _OnScoreEventCallback -= ScoreUpdateHandler;    
    }

    void ScoreDictionaryDataInitializers()
    {
        mDictOfScore.Add(eScoreType.None, 0);
        mDictOfScore.Add(eScoreType.NormalJump, 5);
        mDictOfScore.Add(eScoreType.HighAndLongJump, 10);
        mDictOfScore.Add(eScoreType.StartCoin, 10);
        mDictOfScore.Add(eScoreType.Butterfly, 25);
        mDictOfScore.Add(eScoreType.Firefly, 10);
        mDictOfScore.Add(eScoreType.Bug, 10);
        mDictOfScore.Add(eScoreType.Worm, 10);
        mDictOfScore.Add(eScoreType.Insect, 10);
        mDictOfScore.Add(eScoreType.Cattail, 50);
        mDictOfScore.Add(eScoreType.Pickerelweed, 50);
        mDictOfScore.Add(eScoreType.Iris, 50);
        mDictOfScore.Add(eScoreType.Horsetail, 50);
        mDictOfScore.Add(eScoreType.Kingfisher, 25);
        mDictOfScore.Add(eScoreType.Dragonfly, 25);
        mDictOfScore.Add(eScoreType.Spider, 25);
        mDictOfScore.Add(eScoreType.Duck, 25);
        mDictOfScore.Add(eScoreType.SnakeJumpingOver, 50);
        mDictOfScore.Add(eScoreType.SnakeKilling, 150);
        mDictOfScore.Add(eScoreType.SnakeEscaping, 100);
        mDictOfScore.Add(eScoreType.GiantWaterBugJumpingOver, 50);
        mDictOfScore.Add(eScoreType.GiantWaterBugKilling, 150);
        mDictOfScore.Add(eScoreType.GiantWaterBugEscaping, 100);
        mDictOfScore.Add(eScoreType.BlueHeronJumpingOver, 50);
        mDictOfScore.Add(eScoreType.BlueHeronKilling, 150);
        mDictOfScore.Add(eScoreType.BlueHeronEscaping, 100);
        mDictOfScore.Add(eScoreType.CrabJumpingOver, 50);
        mDictOfScore.Add(eScoreType.CrabKilling, 150);
        mDictOfScore.Add(eScoreType.CrabEscaping, 100);
        mDictOfScore.Add(eScoreType.VenusFlytrapJumpingOver, 50);
        mDictOfScore.Add(eScoreType.VenusFlytrapKilling, 150);
        mDictOfScore.Add(eScoreType.VenusFlytrapEscaping, 100);
        mDictOfScore.Add(eScoreType.RaccoonJumpingOver, 50);
        mDictOfScore.Add(eScoreType.RaccoonKilling, 250);
        mDictOfScore.Add(eScoreType.RaccoonEscaping, 200);
        mDictOfScore.Add(eScoreType.GreatEgretJumpingOver, 50);
        mDictOfScore.Add(eScoreType.GreatEgretKilling, 150);
        mDictOfScore.Add(eScoreType.GreatEgretEscaping, 100);
        mDictOfScore.Add(eScoreType.BatJumpingOver, 75);
        mDictOfScore.Add(eScoreType.BatKilling, 200);
        mDictOfScore.Add(eScoreType.BatEscaping, 150);
        mDictOfScore.Add(eScoreType.HawkJumpingOver, 50);
        mDictOfScore.Add(eScoreType.HawkKilling, 150);
        mDictOfScore.Add(eScoreType.HawkEscaping, 100);
    }

    void ScoreUpdateHandler(eScoreType Type)
    {
        //Debug.Log("ScoreType: " + Type);
        foreach(KeyValuePair<eScoreType, int> elemet in mDictOfScore)
        {
            if (elemet.Key.Equals(Type))
            {
                //Debug.Log("Element Key: " + elemet.Key + ", Value: " + elemet.Value);
                miCurrentScore = elemet.Value;
                break;
            }
        }

        DataManager.AddToHighScore(miCurrentScore);
        DataManager.AddToCSessionScore(miCurrentScore);

		GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
		if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayScore();
    }
}
