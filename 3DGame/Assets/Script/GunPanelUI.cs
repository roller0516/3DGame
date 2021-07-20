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
    public GameObject commandPanel;
    public GameObject ammoPanel;
    public bool skillOn;
    private RectTransform curRectTransform;
    private Gun gun;
    
    // Start is called before the first frame update
    void Start()
    {
        magazineFill.fillAmount = 1.0f;
        gun = GameObject.Find("ShootFX").GetComponent<Gun>();
        curRectTransform = backGroundImage;
        commandPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            skillOn = true;
            StartCoroutine(CommandPanelOn());
            ammoPanel.SetActive(false);
            BroadOpen();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) 
        {
            skillOn = false;
            StopAllCoroutines();
            commandPanel.SetActive(false);
            ammoPanel.SetActive(true);
            BroadClose();
        }

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

    IEnumerator CommandPanelOn() 
    {
        yield return new WaitForSeconds(0.3f);

        if(!commandPanel.activeSelf)
            commandPanel.SetActive(true);
    }

}
