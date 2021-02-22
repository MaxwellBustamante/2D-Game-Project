using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboyCameraController : MonoBehaviour {
	Transform playerTransform;

	public Vector3 offsetToPlayer = new Vector3(0.0f, -15f, -25f);
	Vector3 targetPosition;
	public float maxSpeed = 20.0f;

	void LateUpdate() {
		if (CheckForPlayerTransform() == false) {
			return;
		}
		SetTargetPosition();
		MoveToPositionIfPlayer();
	}

	private void SetTargetPosition() {
		if (playerTransform != null) {
			targetPosition = playerTransform.position + offsetToPlayer;
		}


	}

	private void MoveToPositionIfPlayer() {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxSpeed * Time.smoothDeltaTime);
	}

	private bool CheckForPlayerTransform() {
		if (playerTransform == null) {
			playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
		}
		return playerTransform != null;
	}
}
