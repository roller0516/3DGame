using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GunPanelUI : MonoBehaviour
{
    public TextMeshProUGUI magazine;
    public Image magazineFill;
    public Image gunImage;
    public Image[] boomImage;
    public RectTransform backGroundImage;
    private RectTransform curRectTransform;
    private Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        magazineFill.fillAmount = 1.0f;
        gun = GameObject.Find("ShootFX").GetComponent<Gun>();
        curRectTransform = backGroundImage;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            BroadOpen();
        else if(Input.GetKeyUp(KeyCode.LeftControl))
            BroadClose();


        magazine.text = (gun.ammoRemain / gun.magCapacity).ToString();
        magazineFill.fillAmount =  (float)gun.magAmmo / (float)gun.magCapacity;
    }
    public void SetBoomAlpha(bool use,int count) 
    {
        Color color = boomImage[count].color;
        if (use)
        {
            color.a = 100;
            boomImage[count].color = color;
        }
        else 
        {
            color.a = 255;
            boomImage[count].color = color;
        }
    }
    void BroadOpen() 
    {
        backGroundImage.DOAnchorPosY(4.563f, 1, false);
        backGroundImage.DOSizeDelta(new Vector2(400, 409.126f),1,false);
    }
    void BroadClose() 
    {
        backGroundImage.DOAnchorPosY(-100, 1, false);
        backGroundImage.DOSizeDelta(new Vector2(400,200), 1, false);
    }
    
}
