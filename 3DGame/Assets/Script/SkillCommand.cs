using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

[System.Serializable]
public class skillInfo
{
    public string name;
    public Image[] keyImage;
    public bool isKeyDown;
}

public class SkillCommand : MonoBehaviour
{
    GameData gamedata = new GameData();
    [SerializeField]
    List<skillInfo> skillInfo = new List<skillInfo>();
    [SerializeField]
    Image ImagePreb;
    GunPanelUI gunPanel;
    int count;
    void Start()
    {
        gunPanel = GameObject.Find("Boarder").GetComponent<GunPanelUI>();
        for (int i = 0; i < gamedata.Skill.Count; i++) 
        {
            skillInfo skill = new skillInfo();

            skill.keyImage = new Image[7];

            for (int j = 0; j < 7; j++) 
            {
                skill.keyImage[j] = Instantiate(ImagePreb,GameObject.Find("CommnadPanel").transform);

                if (j == 0)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command1);
                if (j == 1)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command2);
                if (j == 2)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command3);
                if (j == 3)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command4);
                if (j == 4)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command5);
                if (j == 5)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command6);
                if (j == 6)
                    skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command7);
            }
            skillInfo.Add(skill);
        }
    }
    private void Update()
    {
        if (!gunPanel.skillOn) return;
        for (int i = 0; i < gamedata.Skill.Count; i++)
        {
            if (Input.GetKey(gamedata.Skill[i].command1) && count == 0) 
            {
                skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
                count++;
            }
            //if (Input.GetKey(gamedata.Skill[i].command2) && count == 1)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count++;
            //}
            //if (Input.GetKey(gamedata.Skill[i].command3) && count == 2)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count++;
            //}
            //if (Input.GetKey(gamedata.Skill[i].command4) && count == 3)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count++;
            //}
            //if (Input.GetKey(gamedata.Skill[i].command5) && count == 4)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count++;
            //}
            //if (Input.GetKey(gamedata.Skill[i].command6) && count == 5)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count++;
            //}
            //if (Input.GetKey(gamedata.Skill[i].command7) && count == 6)
            //{
            //    skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
            //    count = 0;
            //}
        }
    }

    Sprite SetSprite(string spriteName) 
    {
        Sprite im;

        if (spriteName != "") 
        {
            im = Resources.Load<Sprite>("ArrowImage/"+spriteName);
            return im;
        }
        return null;
    }

}
