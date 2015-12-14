using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	public List<Cube_Node> availableChoices = new List<Cube_Node>();
	public float choiceSize;
	public float restingSize;

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
	public Button butGameOver;
	public Text butGameOverText;

	public Image[] playerTurnIndicator;
	public Color turnNotActive;

	public float delayForPhase;
	public float delayForPhaseSmoothness;
	public float delayForStartingGame;
	public float delayForGameOver;

	public Camera cameraMain;
	public Camera cameraObserve;

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
	//	playerNames [0] = ;
	//	playerNames [1] = ;



		playerTurnIndicator [0].color = playerMat [0].color;
		playerTurnIndicator [1].color = turnNotActive;
		playerTurnIndicator [0].enabled = false;
		playerTurnIndicator [1].enabled = false;

		StartCoroutine (waitToStart());

	}

	IEnumerator waitToStart(){
		gameStart(false);
		yield return new WaitForSeconds (delayForStartingGame);
		gameStart(true);
		StartCoroutine( PopulateBoard ());
	}

	public void gameStart(bool stateI){
		foreach(Button but in butDirInputs){
			but.gameObject.SetActive(stateI);
		}
		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(stateI);
		}

		playerTurnIndicator [0].enabled = stateI;
		playerTurnIndicator [1].enabled = stateI;
	}

	public void changeCamera(){
		cameraMain.enabled = !cameraMain.enabled;
		cameraObserve.enabled = !cameraObserve.enabled;
	}

	public void nextTurn(){
		if (curPlayer == 0) {
			curPlayer = 1;
			playerTurnIndicator [0].color  = turnNotActive;
			playerTurnIndicator [1].color = playerMat [1].color;
		} else {
			curPlayer = 0;
			playerTurnIndicator [1].color  = turnNotActive;
			playerTurnIndicator [0].color = playerMat [0].color;
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
				availableChoices.Add(findClosestCubeNode(inputNodes[i].transform.GetChild(0)));
				availableChoices[availableChoices.Count-1].scaleNode(restingSize , choiceSize);
			}
		}
		if (validMove == false) {
			StartCoroutine( playerVictory (2));
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
			if(node.GetComponent<Cube_Node>() == null){
				continue;
			}
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
			StartCoroutine( playerVictory(3));
		}else{
			StartCoroutine( playerVictory (winningPlayer));
		}
		return true;
	}

	IEnumerator playerVictory(int winningPlayer){
		foreach(Button but in butDirInputs){
			but.gameObject.SetActive(false);
		}
		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(false);
		}
		butConfirm.gameObject.SetActive (false);

		yield return new WaitForSeconds (delayForGameOver);

		butGameOverText.text = playerNames[winningPlayer];
		if(winningPlayer == 1 || winningPlayer == 0){
			butGameOverText.text += " has won!";
			ColorBlock cb;
			cb = butGameOver.colors;
			cb.normalColor = playerMat[winningPlayer].color;
			cb.highlightedColor = playerMat[winningPlayer].color;
			butGameOver.colors = cb;
		}
		butGameOver.gameObject.SetActive (true);
	}

	public void ResetLevel(){
		Invoke ("resetLevelHelper",2);
	}
	void resetLevelHelper(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void chooseNode(int go){
		playerSel = go;

		//highlight selection
		ColorBlock cb;
		if(prevChoiceInputNode != -1){
			findClosestCubeNode (inputNodes[prevChoiceInputNode].transform.GetChild(0).transform).changeColorNode(playerMat [2].color);

			cb = butNodeInputs[prevChoiceInputNode].colors;
			cb.highlightedColor = playerMat [2].color;
			cb.normalColor = playerMat [2].color;
			butNodeInputs[prevChoiceInputNode].colors = cb;
		}
		prevChoiceInputNode = go;
		findClosestCubeNode (inputNodes[prevChoiceInputNode].transform.GetChild(0).transform).changeColorNode(playerMat [curPlayer].color);
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
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
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
		//update the chosen node 

		foreach(Cube_Node node in availableChoices){
			node.scaleNode(choiceSize , restingSize);
		}
		availableChoices.Clear();
		findClosestCubeNode (inputNodes[playerSel].transform.GetChild(0).transform).claimNode(curPlayer);

		inputNodes [playerSel].playNum = curPlayer;
		inputNodes [playerSel].renderer.material=playerMat[curPlayer];

		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(false);
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
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.white;
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
