using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	public GameObject light1;
	public GameObject light2;
	public GameObject light3;

	public float light1rot;
	public float light2rot;
	public float light3rot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		light1.transform.Rotate (0.0F, light1rot * Time.deltaTime, 0.0F, Space.World);
		light2.transform.Rotate (0.0F, light2rot * Time.deltaTime, 0.0F, Space.World);
		light3.transform.Rotate (0.0F, light3rot * Time.deltaTime, 0.0F, Space.World);
	
	}
}
