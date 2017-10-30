using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemAnimator : MonoBehaviour 
{
	public Animator _Animator;

	public void DoAnimation(string animationName)
	{
		_Animator.SetTrigger (animationName);
	}
}
