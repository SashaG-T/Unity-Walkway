using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject child;
	public float positionThreshold;
	public float deltaThreshold;
	public float decay;
	public float damping;
	public float acceleration;
	public float speed;

	private bool playerControlLocked;
	private Vector2 targetPosition;
	private Vector3 delta;
	private Vector3 goodPosition;
	private BoxCollider collider;
	private GameObject lastTile;


	// Use this for initialization
	void Start () {

		playerControlLocked = false;
		delta = new Vector3 ();
		goodPosition = gameObject.transform.position;
		collider = GetComponent<BoxCollider> ();
		collider.enabled = true;
		lastTile = null;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

		float horz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		if (playerControlLocked) {
			movePlayerToTarget ();
		} else {
			movePlayer (horz, vert);
		}

	}

	void movePlayer (float horz, float vert) {

		Vector2 goal = new Vector2 ();
		Vector3 playerVec = gameObject.transform.position;
		Vector3 oldPlayerVec = playerVec;

		goal.x = Mathf.Floor (playerVec.x);
		if (playerVec.x - goal.x > 0.5F) {
			goal.x += 1.0F;
		}

		goal.y = Mathf.Floor (playerVec.z);
		if (playerVec.z - goal.y > 0.5F) {
			goal.y += 1.0F;
		}

		if (playerVec.z == goal.y && delta.z == 0.0F) {
			delta.x += horz * speed;
		}

		if (playerVec.x == goal.x && delta.x == 0.0F) {
			delta.z += vert * speed;
		}

		if (Mathf.Abs (delta.x) < deltaThreshold) {
			delta.x = 0.0F;
		}

		if (Mathf.Abs (delta.z) < deltaThreshold) {
			delta.z = 0.0F;
		}
			
		if (Mathf.Abs (playerVec.x - goal.x) > positionThreshold) {
			if (playerVec.x < goal.x) {
				delta.x += acceleration;
				if (playerVec.x + delta.x > goal.x) {
					//we passed our goal!
					delta.x *= damping;

				}
			} else if (playerVec.x > goal.x) {
				delta.x -= acceleration;
				if (playerVec.x + delta.x < goal.x) {
					//we passed our goal!
					delta.x *= damping;
				}
			}
		} else {
			playerVec.x = goal.x;
		}

		if (Mathf.Abs (playerVec.z - goal.y) > positionThreshold) {
			if (playerVec.z < goal.y) {
				delta.z += acceleration;
				if (playerVec.z + delta.z > goal.y) {
					//we passed our goal!
					delta.z *= damping;
				}
			} else if (playerVec.z > goal.y) {
				delta.z -= acceleration;
				if (playerVec.z + delta.z < goal.y) {
					//we passed our goal!
					delta.z *= damping;
				}
			}
		} else {
			playerVec.z = goal.y;
		}

		delta *= decay;

		playerVec += delta * Time.deltaTime;

		//check if we've passed an integer position. (or are on one)
		if (playerVec.x == goal.x && playerVec.z == goal.y) {
			collider.enabled = true;
		} else {
			float min = Mathf.Min (playerVec.x, oldPlayerVec.x);
			float max = Mathf.Max (playerVec.x, oldPlayerVec.x);
			if (min < goal.x && goal.x < max) {
				collider.enabled = true;
			} else {
				min = Mathf.Min (playerVec.z, oldPlayerVec.z);
				max = Mathf.Max (playerVec.z, oldPlayerVec.z);
				if (min < goal.y && goal.y < max) {
					collider.enabled = true;
				}
			}

		}

		//apply rotation and new position to mesh.
		float xDecimal = (playerVec.x - Mathf.Floor (playerVec.x));
		float zDecimal = (playerVec.z - Mathf.Floor (playerVec.z));

		float lift = Mathf.Sin (Mathf.PI / 4 + xDecimal * Mathf.PI / 2) / 1.4142F
			+ Mathf.Sin (Mathf.PI / 4 + zDecimal * Mathf.PI / 2) / 1.4142F - 1.0F;

		gameObject.transform.position = new Vector3(playerVec.x, playerVec.y, playerVec.z);

		float xRot = xDecimal * 90.0F;
		float zRot = zDecimal * 90.0F;

		child.transform.rotation = Quaternion.Euler (zRot, 0.0F, -xRot);
		child.transform.position = new Vector3 (playerVec.x, playerVec.y + lift, playerVec.z);
	
	}

	void setTargetPosition(float x, float y) {
		targetPosition = new Vector2 (x, y);
		playerControlLocked = true;
	}

	public void setCurrentTile(GameObject target) {
		if (target) {
			Vector3 pos = target.transform.position;
			setGoodPosition (pos.x, pos.z);
			lastTile = target;
		}
	}

	public GameObject getCurrentTile() {
		return lastTile;
	}

	public void setGoodPosition(float x, float y) {
		goodPosition.x = x;
		goodPosition.z = y;
	}

	void movePlayerToTarget() {

		if(playerControlLocked) {
			
			Vector3 playerVec = gameObject.transform.position;

			if (Mathf.Abs (playerVec.x - targetPosition.x) > positionThreshold) {
				//move closer to y.
				if (playerVec.x < targetPosition.x) {
					movePlayer (1.0F, 0.0F);
				} else {
					movePlayer (-1.0F, 0.0F);
				}

			} else if(Mathf.Abs (playerVec.z - targetPosition.y) > positionThreshold) {
				//move close to x.
				if (playerVec.z < targetPosition.y) {
					movePlayer (0.0F, 1.0F);
				} else {
					movePlayer (0.0F, -1.0F);
				}

			} else {
				playerControlLocked = false;
			}
		}

	}

	void OnTriggerEnter(Collider other) {

		//if we hit a red tile then bounce back.
		if (other.CompareTag ("Tile")) {
			TileController tileController = other.gameObject.GetComponent<TileController> ();
			switch (tileController.tileType) {
				case TileController.TileType.Red:
				case TileController.TileType.Grey:
				case TileController.TileType.White: {
						setTargetPosition (goodPosition.x, goodPosition.z);
					break;
				}
			}
		}
	}

	void OnTriggerStay(Collider other) {

		//wait till the tile detects us before turn ourselves off.
		if (other.CompareTag ("Tile")) {
			collider.enabled = false;
		}

	}
		
}
