using UnityEngine;
using System.Collections;



public class camera_Viewing : MonoBehaviour {
	public static camera_Viewing S;
	State state;

	public float perStarting;
	public float perIncrement;

	public Camera mainCamera;
	public Vector3 mainCameraPos;
	public Vector3 startingPos;

	public float speedX;
	public float speedY;

//	public Vector3 cubeHolderPos;
	
	// Use this for initialization
	void Start () {
		S = this;

		state = State.start;
		startingPos = this.transform.position;
		mainCameraPos = mainCamera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.idle){
			return;
		}
		if(state == State.moving){
		
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {

				// Get movement of the finger since last frame
				Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				touchDeltaPosition.x *= speedX;
				touchDeltaPosition.y *= speedY;
				touchDeltaPosition.z = 0;
				// Move object across XY plane
				transform.Translate(new Vector3(touchDeltaPosition.x,0,0) * Time.deltaTime);
				this.transform.LookAt ( Vector3.zero , Vector3.up);
				transform.Translate(new Vector3(0,touchDeltaPosition.y,0) * Time.deltaTime);
				this.transform.LookAt ( Vector3.zero , Vector3.up);
			}	
		}
		if(state == State.start){
			if(perStarting >= 1){
				perStarting = 1;
				GameRunner.S.changeCamera();
				state = State.idle;
			}
			this.transform.position = Vector3.Lerp(startingPos, mainCameraPos , perStarting );
			perStarting += perIncrement;
		}
		this.transform.LookAt ( Vector3.zero , Vector3.up);
		if(Vector3.Distance(this.transform.position,Vector3.zero)> Mathf.Abs( mainCameraPos.z)){
			transform.Translate(Vector3.forward * (Vector3.Distance(this.transform.position,Vector3.zero) - Mathf.Abs( mainCameraPos.z )));
		}
	}
	public void setStateMoving(){
		state = State.moving;
		GameRunner.S.changeCamera();
	}
	public void setStateDoneMoveing(){
		perStarting = 0;
		startingPos = this.transform.position;
		state = State.start;
	}
}
