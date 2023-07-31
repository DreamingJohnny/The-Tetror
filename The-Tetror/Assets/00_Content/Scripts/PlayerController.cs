using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

	//Just realized that this does not work if the player falls of something, will want to think about what to do in that situation later.
	//Will need some way for the BlockDropper to find a rough position in that case, have it be updated every two seconds otherwise or such?

	/// <summary>
	/// Returns a Vector3 for where the player is positioned if they are touching object in the "Ground" layer.
	/// Otherwise, it returns their latest position from before they jumped.
	/// </summary>
	public Vector3 GetGroundedPosition {
		get {
			if (IsGrounded()) return transform.position;
			else return latestGroundedPosition;
		}
	}

	private CapsuleCollider2D capsuleCollider2D;

	readonly string groundLayerName = "Ground";

	[SerializeField] private float jumpForce;

	private Vector3 latestGroundedPosition;

	private Vector2 moveInput;

	private Rigidbody2D rigidBody2D;

	[SerializeField] private float speed = 15;

	private void Start() {
		rigidBody2D = GetComponent<Rigidbody2D>();
		capsuleCollider2D = GetComponent<CapsuleCollider2D>();
	}

	private void Update() {

	}

	private void FixedUpdate() {
		Move(moveInput);
	}

	private void Move(Vector2 direction) {
		rigidBody2D.AddForce(new(direction.x * speed,0f));
	}

	private void OnMove(InputValue value) {
		moveInput = value.Get<Vector2>();
	}

	private void OnJump(InputValue value) {
		if(value.isPressed && IsGrounded()) {
			Debug.Log(value);
			latestGroundedPosition = transform.position;

			rigidBody2D.AddForce(new(0f, jumpForce));
		}
	}

	private bool IsGrounded() {
		if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask(groundLayerName))) return true;
		else return false;
	}
}
