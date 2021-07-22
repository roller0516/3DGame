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
    public GameObject miniMapPanel;
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
        if (skillOn)
            StartCoroutine(CommandPanelOn());
        else if(!skillOn)
            StopCoroutine(CommandPanelOn());

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(MiniMapPanelOn());
            BroadOpen();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            StopCoroutine(MiniMapPanelOn());
            miniMapPanel.SetActive(false);
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
    public void BroadOpen() 
    {
        backGroundImage.DOAnchorPosY(4.563f, 1, false);
        backGroundImage.DOSizeDelta(new Vector2(400, 409.126f),1,false);
        ammoPanel.SetActive(false);
    }
    public void BroadClose() 
    {
        backGroundImage.DOAnchorPosY(-100, 1, false);
        backGroundImage.DOSizeDelta(new Vector2(400,200), 1, false);
        ammoPanel.SetActive(true);
    }

    public IEnumerator CommandPanelOn() 
    {
        yield return new WaitForSeconds(0.3f);
        if(skillOn)
            commandPanel.SetActive(true);
    }

    public IEnumerator MiniMapPanelOn()
    {
        yield return new WaitForSeconds(0.3f);

        if (!miniMapPanel.activeSelf)
            miniMapPanel.SetActive(true);
    }

}
