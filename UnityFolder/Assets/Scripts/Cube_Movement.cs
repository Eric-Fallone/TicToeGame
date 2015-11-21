using UnityEngine;
using System.Collections;

public enum State{
	idle,
	moving,
	start
}

public class Cube_Movement : MonoBehaviour {
	public static Cube_Movement S;

	public State state;
	
	public Vector3 start;
	public Vector3 target;
	public float rotSpeed;

	public float percentComp;
	public float rotDir;

	public float delayForStartingGame;
	
	// Use this for initialization
	void Start () {
		S = this;
		this.transform.localEulerAngles = new Vector3 (0,0,0);
		start = new Vector3 (0,0,0);
	}

	// Update is called once per frame
	void Update () {
		if(state == State.idle){
			return;
		}
		if(state == State.moving){
			if(percentComp>90-rotSpeed){
				percentComp = 90;
				GameRunner.S.nextTurn();
				state=State.idle;
				return;
			}
			//transform.eulerAngles = Vector3.Lerp(start,target,percentComp); 
			this.transform.RotateAround(start, target,rotDir*(90/(90/rotSpeed)));
			percentComp+=rotSpeed;
		}
	}
	public void moveDir(string dir){
		if(state==State.moving){
			return;
		}
		percentComp = 0;

		state = State.moving;
		target = new Vector3 (0,0,0);
	
		switch (dir) {
		case "up":
			target = Vector3.left;
			rotDir=1;
			break;
		case "down":
			target = Vector3.left;
			rotDir=-1;
			break;
		case "left":
			target = Vector3.up;
			rotDir=-1;
			break;
		case "right":
			target = Vector3.up;
			rotDir=1;
			break;
		}
	}
}
