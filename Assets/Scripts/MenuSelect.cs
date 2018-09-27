using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public GameObject DoubleJumpOutline, GroundPoundOutline, RocketOutline;
	public int chara;
    public GameObject rocketSelect;
    public GameObject groundSelect;
    public GameObject doubleSelect;
   
    void Start()
    {
        chara = -1;
        PlayerPrefs.SetInt("Character", -1);
        groundSelect.SetActive(false);
        doubleSelect.SetActive(false);
        rocketSelect.SetActive(false);
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

    public void ChooseChar(int integer)
    {
        chara += integer;
        PlayerPrefs.SetInt("Character", chara);
        if (chara == -2)
        {
            chara += 1;
        }
        if (chara == -1)
        {
            groundSelect.SetActive(false);
            doubleSelect.SetActive(false);
            rocketSelect.SetActive(false);
        }
        if (chara == 0)
        {
            groundSelect.SetActive(false);
            doubleSelect.SetActive(true);
            rocketSelect.SetActive(false);
        }
        if (chara == 1)
        {
            groundSelect.SetActive(true);
            doubleSelect.SetActive(false);
            rocketSelect.SetActive(false);
        }
        if (chara == 2)
        {
            groundSelect.SetActive(false);
            doubleSelect.SetActive(false);
            rocketSelect.SetActive(true);
        }
        if (chara == 3)
        {
            chara += -1;
        }
    }
}
