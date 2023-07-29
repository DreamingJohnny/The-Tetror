using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropper : MonoBehaviour {

	[SerializeField] private GameObject[] blocks;

	//The position where the blocks are instantiated
	private Transform dropSpot;

	//Timespan between which each block is spawned
	[SerializeField][Range(0, 60)] private float spawnSpan;
	private float sinceSpawn = 0f;

	//Speed of movement
	[SerializeField] private float speed;

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
	}

	void Update() {
		if (sinceSpawn <= spawnSpan) {
			sinceSpawn += Time.deltaTime;
		}
		else {
			sinceSpawn = 0f;
			Instantiate(GetRandomBlock(), dropSpot.transform.position, Quaternion.identity);
		}
	}

	private void FixedUpdate() {
		if (Destination != null) {
			// Calculate the direction towards the target
			Vector2 direction = Destination - transform.position;

			// Move towards the target using MovePosition
			rigidBody2D.AddForce(speed * direction.normalized);
		}
	}

	private GameObject GetRandomBlock() {
		return blocks[UnityEngine.Random.Range(0, blocks.Length)];
	}
}