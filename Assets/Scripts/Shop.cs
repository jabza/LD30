using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	
	public GameObject conveyorPrefab;
	public Transform conveyorSpawn;

	private PlayerState player;
	private int mConveyorCost;

	// Use this for initialization
	void Start () {
		mConveyorCost = 100;

		player = GetComponent<PlayerState>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void buyConveyor() {
		if(player != null)
		{
			if(player.getCredits() >= mConveyorCost)
			{
				player.deductCredits(mConveyorCost);
				Instantiate(conveyorPrefab, conveyorSpawn.position, Quaternion.identity);
			}
		}
	}
}
