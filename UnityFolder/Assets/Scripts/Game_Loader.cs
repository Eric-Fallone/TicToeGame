using UnityEngine;
using System.Collections;

public class Game_Loader : MonoBehaviour {
	public static Game_Loader S;
	public string[] playNames;
	public Color[] playCol;
	public isPlayer compLevel; 	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		if(S != null){
			GameObject.Destroy(this);
		}
		S = this;

		playNames = new string[2];
		playCol = new Color[2];
		playCol [0] = new Color (0,0,0);
		playCol [1] = new Color (0,0,0);
		///////load from player preffs
		//"lastOneName"
		playNames[0]=PlayerPrefs.GetString("lastOneName","");
		
		//"lastOneCol R , G , B"

		playCol[0]=new Color(PlayerPrefs.GetFloat("lastOneColR",0),PlayerPrefs.GetFloat("lastOneColG",0),PlayerPrefs.GetFloat("lastOneColB",0));

		//"wasHumanLast" checks to see if that last player two was a human or not 0 for human 1 for computer
		if (PlayerPrefs.GetInt ("wasHumanLast", 0) == 0) {
			//"lastTwoName"
			playNames [1] = PlayerPrefs.GetString ("lastTwoName", "");

			//"lastTwoCol R , G , B"
			playCol [1] = new Color (PlayerPrefs.GetFloat ("lastTwoColR",1), PlayerPrefs.GetFloat ("lastTwoColG",1), PlayerPrefs.GetFloat ("lastTwoColB",1));
			
		}else{
		//"lastCompLevel"
				switch(PlayerPrefs.GetInt("lastCompLevel",1)){
				case 1:
					compLevel= isPlayer.easy;
					break;
				case 2:
					compLevel= isPlayer.med;
					break;
				case 3:
					compLevel= isPlayer.dif;
					break;
			}
		}
		///////
	}	
}
