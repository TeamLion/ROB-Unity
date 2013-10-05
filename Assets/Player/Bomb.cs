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

	// Use this for initialization
	void Start () {
		mTime = 0;
		fDelay = 0;
		BallPos.x = 0;
		BallPos.y = 3;
		BallPos.z = 0;
		rigidbody.useGravity = false;
		bCanShoot = true;
		bCanReset = true;		
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
			
			
			transform.position = MousePos;
			Force = 13 * mTime;
			
			if(fDelay >= 0.1f) {
				TrailScript Trail;
				Trail = (TrailScript)Instantiate(trail, rigidbody.position, rigidbody.rotation);
				Trail.LaunchTrail(Direction, Force, BallPos);
				fDelay = 0;
				Destroy(Trail);
			}
		}
				
		//Return player to original position
		if(Input.GetKey (KeyCode.R)){
			mTime = 0;
			Force = 0;
			rigidbody.useGravity = false;
			rigidbody.rotation = new Quaternion(0,0,0,0);
			rigidbody.freezeRotation = true;
			rigidbody.velocity = new Vector3(0,0,0);
			transform.position = new Vector3(0,3,0);
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
