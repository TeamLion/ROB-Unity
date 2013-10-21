using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	Vector3 mouseTemp;
	Vector3 MousePos;
	float mTime;
	Vector3 BallPos;
	float fDelay;
	public TrailScript trail;
	public Vector3 Direction;
	public float Force;
	public bool bCanShoot;
	public bool bCanReset;
	public GameObject reticle;
	
	private IEnumerator FollowMouse(Vector3 ObjectPos, Vector3 MousePos){
	
		float t = 0.0f;
		Vector3 originalPos = ObjectPos;
		
		while(t < 0.3f){
			
			rigidbody.position = Vector3.Lerp(originalPos, transform.position, t);
			t += Time.deltaTime;
			yield return null;
		}
	}

	// Use this for initialization
	void Start () {
		mTime = 0;
		fDelay = 0;
		BallPos.x = 0;
		BallPos.y = 2;
		BallPos.z = 0;
		rigidbody.useGravity = false;
		bCanShoot = true;
		bCanReset = true;		
		reticle.collider.enabled = false;
		reticle.transform.position = new Vector3(rigidbody.position.x, rigidbody.position.y, rigidbody.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
		//Pull
		if(Input.GetMouseButton(0) && bCanShoot){
			rigidbody.useGravity = false;
			mTime += Time.deltaTime;
			fDelay += Time.deltaTime;
			
			if(mTime >= 2)
				mTime = 2;
			
			mouseTemp = Input.mousePosition;
			mouseTemp.z = mTime;
			MousePos = Camera.main.ScreenToWorldPoint(mouseTemp);
 			MousePos.z = -1*mTime;	

						
			Direction = BallPos - MousePos;
			Direction.Normalize();	

				
			//Set boundries to how much you can pull
			if(MousePos.x > 3)
				MousePos.x = 3;
			if(MousePos.x < -3)
				MousePos.x = -3;
			
			if(MousePos.y > 5)
				MousePos.y = 5;
			if(MousePos.y < 0)
				MousePos.y = 0;
			
			
			StartCoroutine(FollowMouse(transform.position, MousePos));
			//transform.position = MousePos;
			Force = 13 * mTime;
			
			reticle.transform.position =  new Vector3(Direction.x * 10, Direction.y * 10, Direction.z * 10);
				
			/*if(fDelay >= 0.1f) {
				TrailScript Trail;
				Trail = (TrailScript)Instantiate(trail, rigidbody.position, rigidbody.rotation);
				Trail.LaunchTrail(Direction, Force, BallPos);
				fDelay = 0;
				Destroy(Trail);
			}*/
		}
				
		//Return player to original position
		if(Input.GetKey (KeyCode.R)){
			mTime = 0;
			Force = 0;
			rigidbody.useGravity = false;
			rigidbody.rotation = new Quaternion(0,0,0,0);
			rigidbody.freezeRotation = true;
			rigidbody.velocity = new Vector3(0,0,0);
			transform.position = new Vector3(0,2,0);
			bCanShoot = true;
			bCanReset = true;
		}
		
		//FIRE!
		if(Input.GetMouseButtonUp(0) && bCanShoot){
			rigidbody.useGravity = true;
			rigidbody.velocity = Direction * Force;
			mTime = 0;
			Force = 0;
			bCanShoot = false; 
			bCanReset = false;
		}

	}
}
