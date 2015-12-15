using UnityEngine;
using System.Collections;

public class Cube_Node : MonoBehaviour {
	public GameObject model;

	public int playNum;
	public float incrementsToTarget;
	public float smoothness;

	public bool isInputNode;

	float lerpSmoothness = .025f; 

	float rotSpeed = 2;
	
	float maxDistFromCenter =.1f; 

	bool isMoving = true;
	//float maxDistMove =.05f;

	void Start(){
		playNum = 2;
		if(isInputNode == false){
			StartCoroutine (startingMoveMent());
		}
	}

	void Update(){
		if(model == null){
			return;
		}
		model.transform.Rotate (Vector3.right * rotSpeed);
	}


	public void claimNode(int curPlayer){
		playNum = curPlayer;
		StartCoroutine (lerpColor (GameRunner.S.playerMat [curPlayer].color));
		StartCoroutine (lerpScale ( GameRunner.S.choiceSize , 3f));
	}
	public void scaleNode(float start, float end){
		StopCoroutine ("lerpScale");
		StartCoroutine (lerpScale (start,end));
	}
	public void changeColorNode(Color end){
		StartCoroutine ( lerpColor(end));
	}
	IEnumerator lerpColor(Color end){
		Color start = model.renderer.material.color;
		for(float c =0 ; c<1.05;c=c+.1f){
			model.renderer.material.color = Color.Lerp(start,end,c);
			yield return new WaitForSeconds(lerpSmoothness);
		}
	}

	IEnumerator lerpScale(float start, float end){
		isMoving = (end < start);
		float cur;
		for(float c =0 ; c<1.05;c=c+.1f){
			cur = Mathf.Lerp(start,end,c);
			model.transform.localScale = new Vector3(cur,cur,cur);
			yield return new WaitForSeconds(lerpSmoothness);
		}
	}

	public Vector3 endPos; 
	IEnumerator startingMoveMent(){
		if(model == null){
			yield break;
		}
		/*Vector3*/ endPos = this.transform.position;
		Vector3 startPos = new Vector3 (0,0,0);
		for(float i =0 ; i <=1 ; i = i + 1/incrementsToTarget){
			this.transform.position = Vector3.Lerp(startPos,endPos,i);
			yield return new WaitForSeconds(smoothness);
		}
		this.transform.position = endPos;
		StartCoroutine (restingMovement());
	}

	public Vector3 target;

	IEnumerator restingMovement(){
		while (true) {

			while(Cube_Movement.S.state == State.moving){
				yield return new WaitForSeconds(lerpSmoothness);
			}
			
			target = new Vector3 (
				Random.Range (-maxDistFromCenter, maxDistFromCenter),
				Random.Range (-maxDistFromCenter, maxDistFromCenter),
				Random.Range (-maxDistFromCenter, maxDistFromCenter)
			);
			if(isMoving== false){
				target= Vector3.zero;
			}
			target = target + this.transform.position;

			for(float c =0 ; c<1.05;c=c+.1f){
				Vector3 startPos = model.transform.position;
				Vector3 endPosW =target;
				model.transform.position = Vector3.Lerp(startPos,endPosW,c);
				yield return new WaitForSeconds(lerpSmoothness);
				if(Cube_Movement.S.state == State.moving){
					c=2;
				}
			}
		}
	}
}
