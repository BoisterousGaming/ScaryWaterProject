using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum eInputType
{
    None = 0,
    SwipeUp,
    SwipeDown,
    SwipeLeft,
    SwipeRight,
    SingleTap
}

public class TouchInputHandler
{
    public delegate void InputTypeChanged(eInputType type);
    public InputTypeChanged _InputTypeChangedCallback;

    Vector2 mvInputStartPosition = Vector2.zero;
    Vector2 mvInputCurrentPosition = Vector2.zero;
	bool mbInputActive;
	float mfInputTime;

    public PlayerHandler _playerHandler;
    public eInputType _eInputType = eInputType.None;
    public bool _bPointerOverBtn;

	public void CustomUpdate()
	{
		if (PlayerManager.Instance._playerHandler._eControlState == eControlState.Active)
            CaptureInput();

		if (PlayerManager.Instance._playerHandler._eControlState == eControlState.Deactive)
			_eInputType = eInputType.None;
	}

	void CaptureInput()
	{
        InputControl();
        KeyboardControl();
	}

	void InputControl()
	{
		if (Input.GetMouseButtonDown(0) & !mbInputActive & !_bPointerOverBtn)
		{
			mvInputStartPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			mfInputTime = Time.time;
			mbInputActive = true;
		}

		if (mbInputActive)
		{
			mvInputCurrentPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector3 diff = new Vector3(mvInputCurrentPosition.x - mvInputStartPosition.x, mvInputCurrentPosition.y - mvInputStartPosition.y, 0);

			if (Input.GetMouseButtonUp(0))
			{
				if (diff.magnitude <= 0.01f)
				{
					if (Input.GetMouseButtonUp(0) && Time.time - mfInputTime < 0.15f)
						SetInputType(eInputType.SingleTap);
				}

				mbInputActive = false;
			}

			else if (diff.magnitude > 0.01f)
			{
				diff = Vector3.Normalize(diff);
				float angle = Mathf.Atan2(diff.y, diff.x);
				angle *= Mathf.Rad2Deg;

				if (angle < 0)
					angle = 360 + angle;

				if (angle <= 45 || angle > 315)
					SetInputType(eInputType.SwipeRight);
                
				else if (angle <= 135 && angle > 45)
					SetInputType(eInputType.SwipeUp);
                
				else if (angle <= 225 && angle > 135)
					SetInputType(eInputType.SwipeLeft);
                
				else if (angle <= 315 && angle > 225)
					SetInputType(eInputType.SwipeDown);

				mbInputActive = false;
			}
		}
	}

	void KeyboardControl()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			SetInputType(eInputType.SwipeLeft);

		else if (Input.GetKeyDown(KeyCode.RightArrow))
			SetInputType(eInputType.SwipeRight);

		else if (Input.GetKeyDown(KeyCode.UpArrow))
			SetInputType(eInputType.SwipeUp);

		else if (Input.GetKeyDown(KeyCode.DownArrow))
			SetInputType(eInputType.SwipeDown);
	}

	void SetInputType(eInputType inputType)
	{
		if (PlayerManager.Instance._playerHandler._eControlState == eControlState.Deactive)
		{
			_eInputType = eInputType.None;
			return;
		}

		else if (_eInputType != inputType)
		{
			_eInputType = inputType;

            if (_InputTypeChangedCallback != null)
                _InputTypeChangedCallback(_eInputType);

            else if (_InputTypeChangedCallback == null)
                Debug.Log(_InputTypeChangedCallback);
            
			_eInputType = eInputType.None;
		}
	}
}
