using UnityEngine;
using System.Collections;

public class Cube_Node : MonoBehaviour {
	public int playNum;

	void Start(){
		playNum = 2;
	}

	public void setColor(Material matIn){
		this.playNum = GameRunner.S.curPlayer;
		this.renderer.material = GameRunner.S.playerMat[playNum];
	}
}
