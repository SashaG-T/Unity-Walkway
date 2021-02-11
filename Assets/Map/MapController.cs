using UnityEngine;
using AssemblyCSharp;
using System.Collections;

public class MapController : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject player;

	public int width, height;
	public Vector2 startPos;

	public int biteCount;
	public int backBiteCount;
	public float fillPercentage;
	public int slaughterBiteFrequency;
	public int mergeBiteFrequency;

	private GameObject[] tiles;
	private System.Random rand = new System.Random ();

	private HamiltonianPathGeneratorInterface pathGenerator;
	private GraphData graphData;
	private int endPoint;

	// Use this for initialization
	void Start () {
		endPoint = -1;
		pathGenerator = new HamiltonianPathGenerator ();

		generateMap (width, height);
	
	}

	void OnDestroy() {

		foreach (GameObject tile in tiles) {
			Destroy (tile);
		}

	}
	
	// Update is called once per frame
	void Update () {

		//new map
		if (Input.GetKey ("n")) {

			OnDestroy ();
			Start ();

		}

		//try again
		if (Input.GetKey ("r")) {

			OnDestroy ();
			instantiateMap (graphData);

		}
	
	}

	void generateMap (int width, int height) {
		
	
		graphData = pathGenerator.generateBase (width, height);

		//pre-shuffle
		for (int i = 0; i < biteCount; i++) {
			pathGenerator.backbite (graphData);
		}

		int fillAmount = (int)(width * height * fillPercentage);

		//stuff - this comment is utterly useless.
		for (int j = 0; j < fillAmount; j++) {

			for (int i = 0; i < backBiteCount; i++) {
				pathGenerator.backbite (graphData);
			}

			pathGenerator.slaughterbite (graphData);
				

		}

		instantiateMap (graphData);

	}

	void instantiateMap(GraphData graphData) {

		int width = graphData.width;
		int height = graphData.height;

		width += 2;
		height += 2;
		tiles = new GameObject[width * height];
		int next = 0;
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				GameObject newTile = Instantiate (tilePrefab);
				newTile.transform.position = new Vector3 (x-1, 0.0F, y-1);
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					newTile.GetComponent<TileController> ().setType (TileController.TileType.Grey);
				} else {
					newTile.GetComponent<TileController> ().setType (pathGenerator
						.getTileType(graphData, (x-1) + (y-1) * graphData.width));
				}
				if (endPoint < 0) {
					endPoint = pathGenerator.selectEnd (graphData);
				}
				Vector2 playerVec = graphData.convertIndexToVector2 (endPoint);
				player.transform.position = new Vector3 (playerVec.x, 0.55F , playerVec.y);

				tiles [next++] = newTile;
			}
		}

	}

}
