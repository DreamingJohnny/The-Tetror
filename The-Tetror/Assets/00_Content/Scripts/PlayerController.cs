using System;
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
			if (stateOfMovement != StateOfMovement.JUMPING) return transform.position;
			else return latestGroundedPosition;
		}
	}

	readonly string groundLayerName = "Ground";
	readonly string ladderLayerName = "Ladder";

	private StateOfMovement stateOfMovement;

	[SerializeField] private float jumpForce;

	private Vector3 latestGroundedPosition;

	private Vector2 moveInput;

	private Rigidbody2D rigidBody2D;

	[SerializeField] private float speed = 15;

	private void Start() {

		stateOfMovement = StateOfMovement.RUNNING;

		rigidBody2D = GetComponent<Rigidbody2D>();
	}

	private void Update() {

	}

	private void FixedUpdate() {
		Move(moveInput);
	}

	private void Move(Vector2 direction) {
		if (stateOfMovement == StateOfMovement.CLIMBING) rigidBody2D.AddForce(new(0f, direction.y * speed));
		else rigidBody2D.AddForce(new(direction.x * speed, 0f));
	}

	private void OnMove(InputValue value) {
		moveInput = value.Get<Vector2>();
	}

	private void OnJump(InputValue value) {
		if (value.isPressed && stateOfMovement != StateOfMovement.JUMPING) {
			stateOfMovement = StateOfMovement.JUMPING;
			latestGroundedPosition = transform.position;

			rigidBody2D.gravityScale = 1f;
			rigidBody2D.AddForce(new(0f, jumpForce));
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (stateOfMovement == StateOfMovement.RUNNING) return;
		else if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayerName)) {
			stateOfMovement = StateOfMovement.CLIMBING;

			rigidBody2D.gravityScale = 0f;
			rigidBody2D.velocity = Vector2.zero;
			transform.position = new(collision.transform.position.x, transform.position.y);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName)) {
			stateOfMovement = StateOfMovement.RUNNING;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayerName)) {
			rigidBody2D.gravityScale = 1f;
		}
	}
}
