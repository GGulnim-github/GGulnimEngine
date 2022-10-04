using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 Move;
	public float Zoom;
	public bool Jump;
	public bool Sprint;

	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
		if (Move.magnitude > 0.7f)
		{
			Sprint = true;
		}
		else
        {
			Sprint = false;
        }
	}

	public void OnZoom(InputValue value)
    {
		ZoomInput(value.Get<float>());
    }

	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void MoveInput(Vector2 newMoveDirection)
	{
		Move = newMoveDirection;
	}

	public void ZoomInput(float newZoomValue)
    {
		Zoom = newZoomValue;
    }

	public void JumpInput(bool newJumpState)
	{
		Jump = newJumpState;
	}

	public void SprintInput(bool newSprintState)
	{
		Sprint = newSprintState;
	}
}
