using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationScr : MonoBehaviour 
{
    public List<Animator> _listOfAnimator = new List<Animator>();

	void Start () 
    {
        SetAnimatorState(false);
	}

    void OnBecameVisible()
    { 
        SetAnimatorState(true);
    }

    void OnBecameInvisible()
    {
        SetAnimatorState(false);
    }

    void SetAnimatorState(bool state = false)
    {
		if (!_listOfAnimator.Any())
			return;

		for (int i = 0; i < _listOfAnimator.Count; i++)
            _listOfAnimator[i].enabled = state;
    }
}
