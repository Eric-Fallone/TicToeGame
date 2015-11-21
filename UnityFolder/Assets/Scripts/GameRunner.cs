using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameRunner : MonoBehaviour {
	public static GameRunner S;
	//player info variables
	public int curPlayer;
	//third slot is base material
	public Material[] playerMat;
	public string[] playerNames;
	// // // // // // //

	//Player Choices variables
	public int playerSel;
	public Cube_Node[] availableChoices;

	public string playerChoiceDir;
	public string prevDir;
	public int prevChoiceInputNode;
	public int prevChoiceDirection;
	// // // // // // //

	//From Editor 
	public Cube_Node[] inputNodes;
	public Button[] butNodeInputs;
	public Button[] butDirInputs;
	public Button butConfirm;
	public float delayForPhase;
	public float delayForPhaseSmoothness;
	public float delayForStartingGame;

	void Start () {
		S = this;
		curPlayer = 0;
		//error checking
		if(playerMat.Length != 3){
			Debug.Log("Error with player Material Array size");
		}
		if(butNodeInputs.Length!=9){
			Debug.Log("Error with button node inputs length");
		}
		if(butDirInputs.Length!=4){
			Debug.Log("Error with button direction inputs length");
		}
		//load player inputs if avalible
		playerNames=new string[4];
		playerNames[2]="It is a draw";
		playerNames[3]="Both players win...so its a draw";
		//dont destroy on load object from menu
		playerNames [0] += "Player One has won";
		playerNames [1] += "Player Two has won";

		StartCoroutine (waitToStart());
	}

	IEnumerator waitToStart(){
		gameStart(false);
		yield return new WaitForSeconds (delayForStartingGame);
		gameStart(true);
	}

	public void gameStart(bool stateI){
		foreach(Button but in butDirInputs){
			but.gameObject.SetActive(stateI);
		}
		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(stateI);
		}
	}

	public void nextTurn(){
		if (curPlayer == 0) {
			curPlayer = 1;
		} else {
			curPlayer = 0;
		}
		//populates tic tac toe board
		StartCoroutine( PopulateBoard ());
	}
	IEnumerator PopulateBoard(){
		bool validMove = false;
		for (int i=0;i<inputNodes.Length;i++) {
			inputNodes[i].playNum=2;
			foreach (Transform child in inputNodes[i].transform){
				Cube_Node temp = findClosestCubeNode(child);
				if(temp==null){
					Debug.Log("Error Finding closest node");
				}
				if(temp.playNum!=2){
					inputNodes[i].playNum=temp.playNum;
					break;
				}
			}
			if(inputNodes[i].playNum==2){
				validMove = true;
			}
		}
		if (validMove == false) {
			playerVictory (2);
		} 

		PhaseInTicTacToeBoard (true);
		yield return new WaitForSeconds (delayForPhase);
		if(CheckPlayerWin()==true){
			yield break;
		}
		for (int i=0;i<inputNodes.Length;i++){
			butNodeInputs[i].gameObject.SetActive(true);

			ColorBlock cb = butNodeInputs[i].colors;

			if(inputNodes[i].playNum!=2){
				cb.disabledColor = playerMat[inputNodes[i].playNum].color;

				butNodeInputs[i].colors = cb;
				butNodeInputs[i].interactable = false;
			}else{
				cb.disabledColor = playerMat[2].color;
				butNodeInputs[i].interactable = true;
			}
		}
	}
	Cube_Node findClosestCubeNode(Transform tran){
		GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node"); 

		foreach (GameObject node in allNodes) {
			Vector3 diff = node.transform .position - tran.position;
			float curDistance = diff.sqrMagnitude;
			if(curDistance < .1f){
				return node.GetComponent<Cube_Node>();
			}
		}

		return null;
	}
	void PhaseInTicTacToeBoard(bool phaseIn){
		foreach (Cube_Node node in inputNodes) {
			StartCoroutine (PhaseInTicTacToeBoardHelper (node , phaseIn));
		}
	}

	IEnumerator PhaseInTicTacToeBoardHelper(Cube_Node node, bool phaseIn){
		Color startingCol;
		Color endingCol;
		if (phaseIn == true) {
			startingCol = playerMat [2].color;
			startingCol.a = 0;
			node.renderer.material.color = startingCol;
			endingCol = playerMat [node.playNum].color; 
		} else {
			endingCol = playerMat [2].color;
			endingCol.a = 0;
			node.renderer.material.color = endingCol;
			startingCol = playerMat [node.playNum].color; 
		}
		float delayforPhaseInc= delayForPhase/delayForPhaseSmoothness;
			for(float i=0;i <= delayForPhase;i=i+delayforPhaseInc){
				node.renderer.material.color = Color.Lerp(startingCol,endingCol,i);
				yield return new WaitForSeconds(delayforPhaseInc);
			}
		node.renderer.material.color = endingCol;
	}

	bool CheckPlayerWin(){
		int winningPlayer=2;
		bool bothWin = false;
		if(inputNodes[4].playNum != 2 && inputNodes[4].playNum == inputNodes[0].playNum && inputNodes[4].playNum == inputNodes[8].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[4].playNum;
			}else{
				if(winningPlayer!=inputNodes[4].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[4].playNum != 2 && inputNodes[4].playNum == inputNodes[1].playNum && inputNodes[4].playNum == inputNodes[7].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[4].playNum;
			}else{
				if(winningPlayer!=inputNodes[4].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[4].playNum != 2 && inputNodes[4].playNum == inputNodes[2].playNum && inputNodes[4].playNum == inputNodes[6].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[4].playNum;
			}else{
				if(winningPlayer!=inputNodes[4].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[4].playNum != 2 && inputNodes[4].playNum == inputNodes[3].playNum && inputNodes[4].playNum == inputNodes[5].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[4].playNum;
			}else{
				if(winningPlayer!=inputNodes[4].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[1].playNum != 2 && inputNodes[1].playNum == inputNodes[0].playNum && inputNodes[1].playNum == inputNodes[2].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[1].playNum;
			}else{
				if(winningPlayer!=inputNodes[1].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[3].playNum != 2 && inputNodes[3].playNum == inputNodes[0].playNum && inputNodes[3].playNum == inputNodes[6].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[3].playNum;
			}else{
				if(winningPlayer!=inputNodes[3].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[5].playNum != 2 && inputNodes[5].playNum == inputNodes[2].playNum && inputNodes[5].playNum == inputNodes[8].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[5].playNum;
			}else{
				if(winningPlayer!=inputNodes[5].playNum){
					bothWin=true;
				}
			}
		}
		if(inputNodes[7].playNum != 2 && inputNodes[7].playNum == inputNodes[6].playNum && inputNodes[7].playNum == inputNodes[8].playNum){
			if(winningPlayer == 2){
				winningPlayer=inputNodes[7].playNum;
			}else{
				if(winningPlayer!=inputNodes[7].playNum){
					bothWin=true;
				}
			}
		}

		if(winningPlayer==2){
			return false;
		}
		if(bothWin==true){
			playerVictory(3);
		}else{
			playerVictory (winningPlayer);
		}
		return true;
	}

	void playerVictory(int winningPlayer){
		foreach(Button but in butDirInputs){
			but.gameObject.SetActive(false);
		}
		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(false);
		}
		butConfirm.gameObject.SetActive (false);

		Debug.Log (playerNames[winningPlayer]);
	}

	public void chooseNode(int go){
		playerSel = go;

		//highlight selection
		ColorBlock cb;
		if(prevChoiceInputNode != -1){
			cb = butNodeInputs[prevChoiceInputNode].colors;
			cb.highlightedColor = playerMat [2].color;
			cb.normalColor = playerMat [2].color;
			butNodeInputs[prevChoiceInputNode].colors = cb;
		}
		prevChoiceInputNode = go;
		cb = butNodeInputs [go].colors;
		cb.highlightedColor = playerMat [curPlayer].color;
		cb.normalColor = playerMat [curPlayer].color;
		butNodeInputs[go].colors = cb;

		switch (prevDir) {
		case "up":
			butDirInputs[0].interactable=true;
			butDirInputs[2].interactable=true;
			butDirInputs[3].interactable=true;
			break;
		case "down":
			butDirInputs[1].interactable=true;
			butDirInputs[2].interactable=true;
			butDirInputs[3].interactable=true;
			break;
		case "left":
			butDirInputs[0].interactable=true;
			butDirInputs[1].interactable=true;
			butDirInputs[2].interactable=true;
			break;
		case "right":
			butDirInputs[0].interactable=true;
			butDirInputs[1].interactable=true;
			butDirInputs[3].interactable=true;
			break;
		case"":
			butDirInputs[0].interactable=true;
			butDirInputs[1].interactable=true;
			butDirInputs[2].interactable=true;
			butDirInputs[3].interactable=true;
			break;
		}
	}
	public void chooseDir(string go){
		playerChoiceDir = go;
		//highlight selevetion
		ColorBlock cb;
		if(prevChoiceDirection != -1){
			cb = butDirInputs[prevChoiceDirection].colors;
			cb.normalColor = playerMat[2].color;
			cb.highlightedColor = playerMat[2].color;
			butDirInputs[prevChoiceDirection].colors = cb;
		}

		switch(go){
		case "up":
			prevChoiceDirection = 0;
			break;
		case "down":
			prevChoiceDirection = 1;
			break;
		case "left":
			prevChoiceDirection = 2;
			break;
		case "right":
			prevChoiceDirection = 3;
			break;
		case"":
			
			break;
		}
		cb = butDirInputs[prevChoiceDirection].colors;
		cb.normalColor = playerMat[curPlayer].color;
		cb.highlightedColor = playerMat[curPlayer].color;
		butDirInputs[prevChoiceDirection].colors = cb;
	}

	public void ConfirmSel(){
		StartCoroutine (ConfirmSelHelper());
	}

	IEnumerator ConfirmSelHelper(){
		//cube is updated
		findClosestCubeNode (inputNodes[playerSel].transform.GetChild(0).transform).setColor(playerMat[curPlayer]);
		inputNodes [playerSel].playNum = curPlayer;
		inputNodes [playerSel].renderer.material=playerMat[curPlayer];

		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(false);
		}

		if(CheckPlayerWin()==true){
			yield break;
		}
		ColorBlock cb;
		if(prevChoiceInputNode != -1){
			cb = butNodeInputs [prevChoiceInputNode].colors;
			cb.normalColor = playerMat [2].color;
			cb.highlightedColor = playerMat [2].color;
			butNodeInputs [prevChoiceInputNode].colors = cb;
		}
		if(prevChoiceDirection != -1){
			cb = butDirInputs [prevChoiceDirection].colors;
			cb.normalColor = playerMat [2].color;
			cb.highlightedColor = playerMat [2].color;
			butDirInputs [prevChoiceDirection].colors = cb;
		}
		prevChoiceInputNode = -1;
		prevChoiceDirection = -1;
		PhaseInTicTacToeBoard (false);
		yield return new WaitForSeconds(delayForPhase);
		//board moves
		prevDir = playerChoiceDir;
		Cube_Movement.S.moveDir (playerChoiceDir);
	}

}
