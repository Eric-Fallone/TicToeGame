using UnityEngine;
using System.Collections;

public class Cube_Node : MonoBehaviour {
	public GameObject model;

	public int playNum;
	public float incrementsToTarget;
	public float smoothness;

	public bool isInputNode;

	void Start(){
		playNum = 2;
		if(isInputNode == false){
			StartCoroutine (startingMoveMent());
		}
	}

	public void claimNode(int curPlayer){
		playNum = curPlayer;
		StartCoroutine (lerpColor (curPlayer));
		StartCoroutine (lerpScale ( GameRunner.S.choiceSize , 3f));
	}
	public void scaleNode(float start, float end){
		StopAllCoroutines ();
		StartCoroutine (lerpScale (start,end));
	}
	IEnumerator lerpColor(int CurPlayer){
		Color start = model.renderer.material.color;
		Color end = GameRunner.S.playerMat [CurPlayer].color;
		for(float c =0 ; c<1.05;c=c+.1f){
			model.renderer.material.color = Color.Lerp(start,end,c);
			yield return new WaitForSeconds(.1f);
		}
	}

	IEnumerator lerpScale(float start, float end){
		float cur;
		for(float c =0 ; c<1.05;c=c+.1f){
			cur = Mathf.Lerp(start,end,c);
			model.transform.localScale = new Vector3(cur,cur,cur);
			yield return new WaitForSeconds(.05f);
		}
	}

	public Vector3 endPos; 
	IEnumerator startingMoveMent(){
		/*Vector3*/ endPos = this.transform.position;
		Vector3 startPos = new Vector3 (0,0,0);
		for(float i =0 ; i <=1 ; i = i + 1/incrementsToTarget){
			this.transform.position = Vector3.Lerp(startPos,endPos,i);
			yield return new WaitForSeconds(smoothness);
		}
		this.transform.position = endPos;
	}
}
