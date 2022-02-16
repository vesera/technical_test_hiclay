using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] public GameObject ShopMenu;

    [SerializeField] public Button BuyButton;

    [SerializeField] public PlayerController playerController;

    [SerializeField] public GameObject DoubleJump;
    [SerializeField] public GameObject DoubleShot;

    private Button D_Jump_Button;
    private Button D_Shot_Button;
    [HideInInspector] public bool isJumpBrought, isShotBrought, isJumpClicked, isShotClicked;
    [SerializeField] public int shotPrice, jumpPrice;

    void Awake()
    {
        BuyButton.interactable = false;
        isJumpBrought = false;
        isShotBrought = false;
        isShotClicked = false;
        isJumpClicked = false;
        D_Jump_Button = DoubleJump.GetComponent<Button>();
        D_Shot_Button = DoubleShot.GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        D_Jump_Button.onClick.AddListener(delegate { SetBuyJump(!isJumpBrought); });
        D_Shot_Button.onClick.AddListener(delegate { SetBuyShot(!isShotBrought); });
        ToggleBuyButton();
    }

    public void ToggleBuyButton()
    {
        if (isShotClicked || isJumpClicked) {
            BuyButton.interactable = true;
        }

        BuyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (playerController.money >= jumpPrice && isJumpClicked)
        {
            isJumpBrought = true;
            D_Jump_Button.interactable = false;
        }
        //else
        //{

        //}

        if(playerController.money >= shotPrice && isShotClicked)
        {
            isShotBrought = true;
            D_Shot_Button.interactable = false;
        }
        //else
        //{

        //}
    }

    public void SetBuyJump(bool isBrought)
    {
        if (!isJumpClicked)
        {
            isJumpClicked = true;
        }
        else
        {
            isShotClicked = false;
        }
    }

    public void SetBuyShot(bool isBrought)
    {
        if (!isShotClicked)
        {
            isShotClicked = true;
        }
        else
        {
            isJumpClicked = false;
        }
    }

    public void ShowShopMenu()
    {
        ShopMenu.gameObject.SetActive(true);
    }

    public void HideShopMenu()
    {
        ShopMenu.gameObject.SetActive(false);
    }
}
