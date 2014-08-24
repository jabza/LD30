using UnityEngine;
using System.Collections;

public class EntryShaft : MonoBehaviour {

	public DIRECTIONS 	exitDirection;
	private bool 		mContact;
	private float		mDelayInterval;
	private float		mTimer;

	// Use this for initialization
	void Start () {
		mContact = false;
		mDelayInterval = 2.0f;
		transform.rotation *= Quaternion.AngleAxis((-90)*(int)exitDirection, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
		mTimer -= Time.deltaTime;
	}

	public bool inUse() {
		if(mContact)
			return true;
		else if(mTimer > 0.0f)
			return true;

		return false;
	}

	public void setContact() {
		mContact = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag.Contains("Package"))
			mContact = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag.Contains("Package") || other.tag.Contains("DestroyedPackage"))
		{
			mTimer = mDelayInterval;
			mContact = false;
		}
	}
}
