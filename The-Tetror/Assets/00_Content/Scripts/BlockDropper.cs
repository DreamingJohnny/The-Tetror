using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropper : MonoBehaviour {

	[SerializeField] private GameObject[] blocks;

	//Where the blocks are instantiated
	[SerializeField] private GameObject dropSpot;

	//Timespan between which each block is spawned
	[SerializeField][Range(0, 60)] private float spawnSpan;
	private float sinceSpawn;

	//Speed of movement
	[SerializeField] private float speed;

	[SerializeField] private Vector2 destinationOffset; 

	private Vector2 destination;

	public Vector2 Destination {
		get { return destination; }
		set {
			destination = (value + destinationOffset);
		}
	}

	private Rigidbody2D rigidBody2D;

	private void Start() {
		rigidBody2D = GetComponent<Rigidbody2D>();
		rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
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
		if (destination != null) {
			// Calculate the direction towards the target
			Vector2 direction = Destination - (Vector2)transform.position;

			// Move towards the target using MovePosition
			Vector2 newPosition = rigidBody2D.position + speed * Time.deltaTime * direction.normalized;
			rigidBody2D.MovePosition(newPosition);
		}
	}

	private GameObject GetRandomBlock() {
		return blocks[UnityEngine.Random.Range(0, blocks.Length)];
	}
}