using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

	private int mCredits;

	// Use this for initialization
	void Start () {
		mCredits = 1000;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int getCredits() {
		return mCredits;
	}

	public void addCredits(int credits) {
		mCredits += credits;
	}

	public void deductCredits(int credits) {
		mCredits -= credits;
	}
}
