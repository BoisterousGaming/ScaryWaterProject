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
    List<eScoreType> mListOfElementsScoreDisplay = new List<eScoreType>();
    int miCurrentScore;
    int miTempCurrentScore;

    public delegate void ScoreEvent(eScoreType type);
    public static ScoreEvent _OnScoreEventCallback;

    void Awake()
    {
        mInstance = this;
        ScoreDictionaryDataInitializers();
        ElementsScoreOnScreenDisplayDataInitializers();
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

    void ElementsScoreOnScreenDisplayDataInitializers()
    {
        //mListOfElementsScoreDisplay.Add(eScoreType.HighAndLongJump);
        mListOfElementsScoreDisplay.Add(eScoreType.Butterfly);
        mListOfElementsScoreDisplay.Add(eScoreType.Firefly);
        mListOfElementsScoreDisplay.Add(eScoreType.Bug);
        mListOfElementsScoreDisplay.Add(eScoreType.Worm);
        mListOfElementsScoreDisplay.Add(eScoreType.Insect);
        mListOfElementsScoreDisplay.Add(eScoreType.Cattail);
        mListOfElementsScoreDisplay.Add(eScoreType.Pickerelweed);
        mListOfElementsScoreDisplay.Add(eScoreType.Iris);
        mListOfElementsScoreDisplay.Add(eScoreType.Horsetail);
        mListOfElementsScoreDisplay.Add(eScoreType.Kingfisher);
        mListOfElementsScoreDisplay.Add(eScoreType.Dragonfly);
        mListOfElementsScoreDisplay.Add(eScoreType.Spider);
        mListOfElementsScoreDisplay.Add(eScoreType.SnakeJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.SnakeKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.SnakeEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.GiantWaterBugJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.GiantWaterBugKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.GiantWaterBugEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.BlueHeronJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.BlueHeronKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.BlueHeronEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.CrabJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.CrabKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.CrabEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.VenusFlytrapJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.VenusFlytrapKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.VenusFlytrapEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.RaccoonJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.RaccoonKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.RaccoonEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.GreatEgretJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.GreatEgretKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.GreatEgretEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.BatJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.BatKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.BatEscaping);
        mListOfElementsScoreDisplay.Add(eScoreType.HawkJumpingOver);
        mListOfElementsScoreDisplay.Add(eScoreType.HawkKilling);
        mListOfElementsScoreDisplay.Add(eScoreType.HawkEscaping);
    }

    void ScoreUpdateHandler(eScoreType scoreType)
    {
        foreach(KeyValuePair<eScoreType, int> element in mDictOfScore)
        {
            if (element.Key.Equals(scoreType))
            {
                miCurrentScore = element.Value;
                break;
            }
        }

        DisplayScoreOnScreen(scoreType);
        DataManager.AddToHighScore(miCurrentScore);
        DataManager.AddToCSessionScore(miCurrentScore);

		GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
		if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayCurrentScore();
    }

    void DisplayScoreOnScreen(eScoreType scoreType)
    {
        for (int i = 0; i < mListOfElementsScoreDisplay.Count; i++)
        {
            eScoreType tScoreElement = mListOfElementsScoreDisplay[i];
            if (tScoreElement.Equals(scoreType))
            {
                foreach (KeyValuePair<eScoreType, int> element in mDictOfScore)
                {
                    if (tScoreElement.Equals(element.Key))
                    {
                        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
                        if (tCanvas != null)
                            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayOnScreenScore(element.Value);
                        break;
                    }
                }
            }
        }
    }
}
