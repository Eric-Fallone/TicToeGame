using UnityEngine;
using System.Collections;

public class Cube_Node : MonoBehaviour {
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

	public void setColor(Material matIn){
		this.playNum = GameRunner.S.curPlayer;
		this.renderer.material = GameRunner.S.playerMat[playNum];
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
