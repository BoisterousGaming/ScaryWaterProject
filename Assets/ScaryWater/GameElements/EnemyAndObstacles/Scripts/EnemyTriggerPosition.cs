using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerPosition : MonoBehaviour 
{
    public enum eTriggerPosition
    {
        Avobe = 0,
        Left,
        Right
    }

    bool mbSkipChecking;

    public eTriggerPosition _eTriggerPosition = eTriggerPosition.Avobe;
    public EnemyHandler _EnemyHandlerScr;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (mbSkipChecking)
                return;
            
			if (ScoreHandler._OnScoreEventCallback != null)
			{
				mbSkipChecking = true;

				if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.Snake))
                    ScoringBasedOnTriggerPosition(eScoreType.SnakeJumpingOver, eScoreType.SnakeEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.GiantWaterBug))
                    ScoringBasedOnTriggerPosition(eScoreType.GiantWaterBugJumpingOver, eScoreType.GiantWaterBugEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.BlueHeron))
                    ScoringBasedOnTriggerPosition(eScoreType.BlueHeronJumpingOver, eScoreType.BlueHeronEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.Crab))
                    ScoringBasedOnTriggerPosition(eScoreType.CrabJumpingOver, eScoreType.CrabEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.VenusFlytrap))
                    ScoringBasedOnTriggerPosition(eScoreType.VenusFlytrapJumpingOver, eScoreType.VenusFlytrapEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.Raccoon))
                    ScoringBasedOnTriggerPosition(eScoreType.RaccoonJumpingOver, eScoreType.RaccoonEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.GreatEgret))
                    ScoringBasedOnTriggerPosition(eScoreType.GreatEgretJumpingOver, eScoreType.GreatEgretEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.Bat))
                    ScoringBasedOnTriggerPosition(eScoreType.BatJumpingOver, eScoreType.BatEscaping);

				else if (_EnemyHandlerScr._eEnemyType.Equals(eEnemyType.Hawk))
                    ScoringBasedOnTriggerPosition(eScoreType.HawkJumpingOver, eScoreType.HawkEscaping);
			}
        }
    }

    void ScoringBasedOnTriggerPosition(eScoreType JumpOver, eScoreType Escaping)
    {
		if (_eTriggerPosition.Equals(eTriggerPosition.Avobe))
			ScoreHandler._OnScoreEventCallback(JumpOver);

		else if (_eTriggerPosition.Equals(eTriggerPosition.Left) | _eTriggerPosition.Equals(eTriggerPosition.Right))
			ScoreHandler._OnScoreEventCallback(Escaping);
    }
}
