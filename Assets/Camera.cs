using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject target;

	private Vector3 goal;

	void Start() {

		goal = target.transform.position - gameObject.transform.position;

	}

	void LateUpdate () {

		Vector3 offset = target.transform.position - gameObject.transform.position;

		Vector3 difference = goal - offset;
		difference /= 2;

		gameObject.transform.Translate (offset);


	}
}
