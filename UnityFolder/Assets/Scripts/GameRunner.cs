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
	// // // // // // //

	//From Editor 
	public Cube_Node[] inputNodes;
	public Button[] butNodeInputs;
	public Button[] butDirInputs;
	public Button butConfirm;


	void Start () {
		S = this;
		curPlayer = 1;
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
		playerNames [1] += "Player One has won";
	}

	public void nextTurn(){
		if (curPlayer == 0) {
			curPlayer = 1;
		} else {
			curPlayer = 0;
		}
		//populates tic tac toe board
		PopulateBoard ();
		//check to see if any player won after board turned
		CheckPlayerWin ();
	}
	public void PopulateBoard(){
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
				butNodeInputs[i].gameObject.SetActive(true);
				validMove = true;
			}
		}
		if(validMove == false){
			playerVictory(2);
		}
		PhaseInTicTacToeBoard (true);
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
			for(float i=0;i < 1f;i=i+.005f){
				node.renderer.material.color = Color.Lerp(startingCol,endingCol,i);
				yield return new WaitForSeconds(.000005f);
			}

	}

	void CheckPlayerWin(){
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
			return;
		}
		if(bothWin==true){
			playerVictory(3);
		}
		playerVictory (winningPlayer);
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
		}

	public void ConfirmSel(){
		//checks to see if player has won
		CheckPlayerWin ();
		//cube is updated
		findClosestCubeNode (inputNodes[playerSel].transform.GetChild(0).transform).setColor(playerMat[curPlayer]);
		//clear tic tac toe board
		PhaseInTicTacToeBoard (false);
		foreach(Button but in butNodeInputs){
			but.gameObject.SetActive(false);
		}
		//board moves
		prevDir = playerChoiceDir;
		Cube_Movement.S.moveDir (playerChoiceDir);
	}

}
