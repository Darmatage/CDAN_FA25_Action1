using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class FadeToBlack_Canvas : MonoBehaviour
{

	Image imageToFade;
    float alphaLevel = 0f; 
	public float fadeSpeed= 0.005f;

	public string nextLevel ="EndWin";

     void Start(){
        imageToFade = gameObject.GetComponent<Image>();
		imageToFade.color = new Color(0, 0, 0, alphaLevel);
		
    }

    // Update is called once per frame
    public void ActivateFade()
    {
        StartCoroutine(FadeIn(imageToFade));
    }

	IEnumerator FadeIn(Image fadeImage){
		for (int i = 0; i < 100; i++){
			alphaLevel += fadeSpeed;
			yield return null;
			fadeImage.color = new Color(0, 0, 0, alphaLevel);
			//Debug.Log("Alpha is: " + alphaLevel);
		}
		NextLevel();
	} 

	public void NextLevel()
    {
        SceneManager.LoadScene("nextLevel");
    }
}
