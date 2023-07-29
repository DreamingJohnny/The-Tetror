using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropper : MonoBehaviour {

	[SerializeField] private LayerMask targetLayer;

	[SerializeField] private GameObject[] blocks;

	[Tooltip("Position where blocks are instantiated")]
	private Transform dropSpot;

	[Tooltip("Time that BlockDropper spends motionless after instantiating a block.")]
	[Range(0.0f, 10.0f)][SerializeField] private float recoilTime;
	[Tooltip("Maximum time the Blockdropper waits before instantiating block.")]
	[Range(0f, 60f)][SerializeField] private float maxWaitTime;
	private float sinceInstantiatingBlock = 0f;

	[SerializeField] private float speed;

	[Tooltip("Offset between the target and the BlockDroppers position")]
	[SerializeField] private Vector3 destinationOffset;

	public Vector3 Destination {
		get { return playerController.GroundedPosition + destinationOffset; }
	}

	[SerializeField] private PlayerController playerController;

	private Rigidbody2D rigidBody2D;

	private void Start() {
		rigidBody2D = GetComponent<Rigidbody2D>();
		rigidBody2D.bodyType = RigidbodyType2D.Dynamic;

		dropSpot = GetComponentInChildren<Transform>();
		Debug.Assert(dropSpot != null);

		Debug.Assert(playerController != null);

		if (maxWaitTime <= recoilTime) { Debug.Log($"{name} has a longer recoile time than the maxWaitTime, the BlockDropper will be unable to move,"); }
	}

	void Update() {

		if (sinceInstantiatingBlock >= recoilTime) CheckForTarget();

		if (sinceInstantiatingBlock <= maxWaitTime) {
			sinceInstantiatingBlock += Time.deltaTime;
		}
		else { DropBlock(); }
	}

	private void CheckForTarget() {
		// Cast a ray downwards from the current position of the object
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, targetLayer);

		if (hit && hit.collider != null) { DropBlock(); }
	}

	private void DropBlock() {
		StopBlockDropper(); //Stops the Blockdropper since a new block is about to be instantiated;

		Instantiate(GetRandomBlock(), dropSpot.transform.position, Quaternion.identity);
		sinceInstantiatingBlock = 0f;
	}

	private void FixedUpdate() {
		if (sinceInstantiatingBlock <= recoilTime) return;

		if (Destination != null) {
			// Calculate the direction towards the target
			Vector2 direction = Destination - transform.position;

			// Move towards the target using MovePosition
			rigidBody2D.AddForce(speed * direction.normalized);
		}
	}

	private void StopBlockDropper() {
		rigidBody2D.velocity = Vector2.zero;
	}

	private GameObject GetRandomBlock() {
		return blocks[UnityEngine.Random.Range(0, blocks.Length)];
	}
}