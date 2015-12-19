using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public InputField[] playNames;


	public Image[] playCol;

	public Slider[] sliders;

	//Color Change Varibales
	int playNumber ; 
	char whatColor ;
	float value;
	/////
	public void Start(){
		if(Game_Loader.S == null){
			return;
		}
		playNames[0].text = Game_Loader.S.playNames[0];
		playNames [1].text = Game_Loader.S.playNames [1];

		sliders [0].value = Game_Loader.S.playCol [0].r;
		sliders [1].value = Game_Loader.S.playCol [0].g;
		sliders [2].value = Game_Loader.S.playCol [0].b;

		sliders [3].value = Game_Loader.S.playCol [1].r;
		sliders [4].value = Game_Loader.S.playCol [1].g;
		sliders [5].value = Game_Loader.S.playCol [1].b;


	}

	public void StartGame(){
		//set dont destroy on load variables

		Game_Loader.S.playNames [0] = playNames[0].text;
		Game_Loader.S.playNames [1] = playNames[1].text;

		Game_Loader.S.playCol [0] = playCol [0].color;
		Game_Loader.S.playCol [1] = playCol [1].color;
		//set player prefs
		PlayerPrefs.SetString ("lastOneName",playNames[0].text);
		PlayerPrefs.SetString ("lastTwoName",playNames[1].text);

		PlayerPrefs.SetFloat ("lastOneColR", playCol[0].color.r);
		PlayerPrefs.SetFloat ("lastOneColG", playCol[0].color.g);
		PlayerPrefs.SetFloat ("lastOneColB", playCol[0].color.b);

		PlayerPrefs.SetFloat ("lastTwoColR", playCol[1].color.r);
		PlayerPrefs.SetFloat ("lastTwoColG", playCol[1].color.g);
		PlayerPrefs.SetFloat ("lastTwoColB", playCol[1].color.b);

		Application.LoadLevel ("GameScreen");
	}



	public void setPlayNumber(int num){
		playNumber = num;
	}
	public void setColorChar(string c){
		whatColor = c[0];
	}
	public void setValue(int i){
		value = sliders[i].value;
	}
	
	public void changeColor(){
		Color temp = playCol [playNumber].color;
		switch(whatColor){
		case 'r':
			temp.r = value;
			break;
		case 'g':
			temp.g = value;
			break;
		case 'b':
			temp.b = value;
			break;
		}
		playCol [playNumber].color = temp;
	}



}
