using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {

	public enum TileType {
		Blue,
		PreRed,
		Red,
		Grey,
		White
	}

	public Material[] materials;
	public Renderer renderer;
	public TileType tileType;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer> ();
		renderer.enabled = true;
		setType (tileType);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.CompareTag ("Player")) {
			PlayerController playerController = other.gameObject.GetComponent<PlayerController> ();
			switch (tileType) {

				case TileType.Blue: {
					GameObject oldTile = playerController.getCurrentTile ();
					if (oldTile) {
						oldTile.GetComponent<TileController>().setType (TileType.Red);
					}
					playerController.setCurrentTile (gameObject);
					setType (TileType.PreRed);
					break;
				}
				case TileType.PreRed: {
					//do nothing. We be "Entering" this all de time them.
					break;

				}
				case TileType.Red: {
					//bounce back here?
					break;
				}
				case TileType.Grey: {
					//bounce back here?
					break;
				}
				case TileType.White: {
					//bounce back here?
					break;
				}

			}
		}

	}

	public void setType(TileType type) {
		tileType = type;
		Destroy (renderer.material);
		renderer.material = materials [(int)tileType];
	}

	public bool isType(TileType type) {
		return tileType == type;
	}
		
}
