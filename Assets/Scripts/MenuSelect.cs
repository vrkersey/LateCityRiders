using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public GameObject DoubleJumpOutline, GroundPoundOutline;
	public int chara;
   
    void start()
    {
        chara = 0;
        PlayerPrefs.SetInt("Character", 0);
    }

	public void ChooseDoubleJump()
    {
        chara = 0;
        PlayerPrefs.SetInt("Character",chara);
        DoubleJumpOutline.SetActive(true);
        GroundPoundOutline.SetActive(false);
    }
    public void ChooseGroundPound()
    {
        chara = 1;
        PlayerPrefs.SetInt("Character", chara);
        DoubleJumpOutline.SetActive(false);
        GroundPoundOutline.SetActive(true);
    }

    public void Begin()
    {
        SceneManager.LoadScene("First Playable");
    }
}
