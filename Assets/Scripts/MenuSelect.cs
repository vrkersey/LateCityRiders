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
    public GameObject rocketText;
    public GameObject groundText;
    public GameObject doubleText;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        chara = 0;
        PlayerPrefs.SetInt("Character", -1);
        groundSelect.SetActive(false);
        doubleSelect.SetActive(true);
        rocketSelect.SetActive(false);
        groundText.SetActive(false);
        doubleText.SetActive(true);
        rocketText.SetActive(false);
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

            SceneManager.LoadScene("Level1Version2");
        }
    }

    public void ChooseChar(int integer)
    {
        chara += integer;
        PlayerPrefs.SetInt("Character", chara);
        if (chara == -1)
        {
            chara += 1;
        }
        if (chara == 0)
        {
            groundSelect.SetActive(false);
            doubleSelect.SetActive(true);
            rocketSelect.SetActive(false);
            groundText.SetActive(false);
            doubleText.SetActive(true);
            rocketText.SetActive(false);
        }
        if (chara == 1)
        {
            groundSelect.SetActive(true);
            doubleSelect.SetActive(false);
            rocketSelect.SetActive(false);
            groundText.SetActive(true);
            doubleText.SetActive(false);
            rocketText.SetActive(false);
        }
        if (chara == 2)
        {
            groundSelect.SetActive(false);
            doubleSelect.SetActive(false);
            rocketSelect.SetActive(true);
            groundText.SetActive(false);
            doubleText.SetActive(false);
            rocketText.SetActive(true);
        }
        if (chara == 3)
        {
            chara += -1;
        }
    }
}
