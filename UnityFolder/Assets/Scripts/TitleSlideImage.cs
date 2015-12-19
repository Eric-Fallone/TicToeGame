using UnityEngine;
using System.Collections;

public class TitleSlideImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("loadMain",2);
	}

	void loadMain(){
		Application.LoadLevel ("MainMenu");
	}

}
