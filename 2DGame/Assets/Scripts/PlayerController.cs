using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanplumSDK;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody2D rb2d;
	int pickupCount;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		rb2d.AddForce (movement * speed);	
	}
		
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag ("PickUp")) 
		{
			pickupCount++;
			other.gameObject.SetActive (false);

			if (Leanplum.HasStarted) {
				Dictionary<string, object> attributes = new Dictionary<string, object>();
				attributes.Add("pickUp Count", pickupCount);
				Leanplum.SetUserAttributes (attributes);
				Leanplum.Track ("PickUp");

				string userName = "Sasho3-" + (pickupCount).ToString ();
				Leanplum.SetUserId (userName);
				Leanplum.ForceContentUpdate ();
			}
		}
	}
}