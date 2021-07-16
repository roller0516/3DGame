using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunPanelUI : MonoBehaviour
{
    public TextMeshProUGUI magazine;
    public Image magazineFill;
    public Image gunImage;
    public Image[] boomImage;
    private Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        magazineFill.fillAmount = 1.0f;
        gun = GameObject.Find("ShootFX").GetComponent<Gun>();
    }
    // Update is called once per frame
    void Update()
    {
        magazine.text = (gun.ammoRemain / gun.magCapacity).ToString();
        magazineFill.fillAmount -=  gun.magAmmo / gun.magCapacity;
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
}
