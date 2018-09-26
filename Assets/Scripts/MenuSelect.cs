using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public GameObject DoubleJumpOutline, GroundPoundOutline, RocketOutline;
	public int chara;
   
    void Start()
    {
        chara = -1;
        PlayerPrefs.SetInt("Character", -1);
    }

	public void ChooseDoubleJump()
    {
        chara = 0;
        PlayerPrefs.SetInt("Character",chara);
        //DoubleJumpOutline.GetComponent<Image>().
        DoubleJumpOutline.SetActive(true);
        GroundPoundOutline.SetActive(false);
        RocketOutline.SetActive(false);
    }
    public void ChooseGroundPound()
    {
        chara = 1;
        PlayerPrefs.SetInt("Character", chara);
        DoubleJumpOutline.SetActive(false);
        GroundPoundOutline.SetActive(true);
        RocketOutline.SetActive(false);
    }
    public void ChooseRocket()
    {
        chara = 2;
        PlayerPrefs.SetInt("Character", chara);
        DoubleJumpOutline.SetActive(false);
        GroundPoundOutline.SetActive(false);
        RocketOutline.SetActive(true);
    }
    public void Begin()
    {
        if(chara != -1)
        {

            SceneManager.LoadScene("Level1");
        }
    }
}
