using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ePlayerStatus
{ 
    Normal,
    Poison,
    Double,
    Magnet,
}

public class CNewPlayer : MonoBehaviour
{


    ePlayerStatus meState = ePlayerStatus.Normal;
    float _Health;

    public void DoJump(Vector3 initialPosition, Vector3 finalPosition, float height , float time)
    {
        //do sin function
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject tObj = collision.gameObject;
        switch(tObj.tag )
        {
            case "Obstacle":
                DidCollideWithObstacle(tObj);
                break;
			case "Coin":
				break;
			case "Collectables":
				break;
            default:
                break;
        }
    }


    void DidCollideWithObstacle(GameObject tObj)
    {
		Obstacle tObstacle = tObj.GetComponent<Obstacle>();
		if (tObstacle != null)
		{

		}
    }


    // Update is called once per frame
    void Update () {
		
	}
}
