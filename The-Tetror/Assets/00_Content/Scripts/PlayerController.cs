using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//I will set this up so that there is a separate Vector3 for the players position at the moment when they jump, so that,
	//if the player is in the air, this reference is a getter for that position
	public Vector3 GroundedPosition {
		get {
			if (isGrounded) return transform.position;
			else return Vector3.zero;
		}
	}

	private Vector3 lastGroundedPosition;

	private bool isGrounded = true;

	private Vector2 direction;

	private Rigidbody2D rigidBody2D;

	private float speed = 25;

	private void Start() {
		rigidBody2D = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void FixedUpdate() {
		MoveCharacter(direction);
	}

	private void MoveCharacter(Vector2 direction) {
		rigidBody2D.AddForce(direction * speed);
	}

}
