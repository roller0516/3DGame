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
    public int maxCombo;
    public int curCombo;
    public bool skillDown;
}

public class SkillCommand : MonoBehaviour
{
    private GameData gamedata = new GameData();
    [SerializeField]
    private List<skillInfo> skillInfo = new List<skillInfo>();
    [SerializeField]
    private Image ImagePreb;
    private GunPanelUI gunPanel;
    private int count;
    private int open;
    private PlayerMovement playerMovement;
    void Start()
    {
        gunPanel = GameObject.Find("Boarder").GetComponent<GunPanelUI>();
        playerMovement = GetComponent<PlayerMovement>();
        DataSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            gunPanel.skillOn = true;
            gunPanel.BroadOpen();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            gunPanel.skillOn = false;
            gunPanel.commandPanel.SetActive(false);
            gunPanel.BroadClose();
        }

        if (!gunPanel.skillOn)
        {
            count = 0;
            open = 0;
            for (int i = 0; i < skillInfo.Count; i++)
            {
                skillInfo[i].skillDown = false;
            }
            return;
        }
        else if (gunPanel.skillOn && open == 0) 
        {
            for (int i = 0; i < skillInfo.Count; i++)
            {
                skillInfo[i].skillDown = true;
                open++;
            }
        }
            
        if (Input.GetKeyDown(KeyCode.W))
            UseSkill("w");
        if (Input.GetKeyDown(KeyCode.A))
            UseSkill("a");
        if (Input.GetKeyDown(KeyCode.S))
            UseSkill("s");
        if (Input.GetKeyDown(KeyCode.D))
            UseSkill("d");
    }
    void UseSkill(string key) 
    {
        for (int i = 0; i < skillInfo.Count; i++) 
        {
            if (skillInfo[i].skillDown == false) continue;
            if (count > skillInfo[i].keyImage.Length) continue;
            if (skillInfo[i].keyImage[count].sprite == null) continue;
            if (skillInfo[i].keyImage[count].sprite.name == key)
            {
                skillInfo[i].keyImage[count].color = new Color(255, 255, 0);
                skillInfo[i].curCombo++;
            }
            else if (skillInfo[i].keyImage[count].sprite.name != key)
            {
                closeSkill(i);
            }

            if (skillInfo[i].curCombo == skillInfo[i].maxCombo) //º“»Ø
            {
                playerMovement.CreateThrowItem(playerMovement.throwItem[1]);
                StartCoroutine(spawnItem(i));
				closeSkill(i);
				count = 0;
			}
        }
        count++;
    }
    
    void closeSkill(int spellNum)
    {
        for (int i = 0; i < skillInfo[spellNum].maxCombo; i++)
        {
            skillInfo[spellNum].keyImage[i].color = new Color(50, 50, 50);
        }
        skillInfo[spellNum].curCombo = 0;
        skillInfo[spellNum].skillDown = false;
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
    void DataSetting()
    {
        for (int i = 0; i < gamedata.Skill.Count; i++)
        {
            skillInfo skill = new skillInfo();

            skill.keyImage = new Image[7];
            skill.name = gamedata.Skill[i].name;
            for (int j = 0; j < 7; j++)
            {
                skill.keyImage[j] = Instantiate(ImagePreb, GameObject.Find("CommnadPanel").transform);
                
                switch (j)
                {
                    case 0:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command1);
                        if (gamedata.Skill[i].command1 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 1:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command2);
                        if (gamedata.Skill[i].command2 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 2:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command3);
                        if (gamedata.Skill[i].command3 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 3:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command4);
                        if (gamedata.Skill[i].command4 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 4:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command5);
                        if (gamedata.Skill[i].command5 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 5:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command6);
                        if (gamedata.Skill[i].command6 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                    case 6:
                        skill.keyImage[j].sprite = SetSprite(gamedata.Skill[i].command7);
                        if (gamedata.Skill[i].command7 == "")
                        {
                            skill.keyImage[j].color = new Color(0, 0, 0, 0);
                            continue;
                        }
                        else
                            skill.maxCombo++;
                        break;
                }
            }
            skillInfo.Add(skill);
        }
    }

    IEnumerator spawnItem(int index) 
    {
        GameObject item = playerMovement.HasThrowItem;
        yield return new WaitForSeconds(3.0f);
        Vector3 v = new Vector3(item.transform.position.x, item.transform.position.y + 20, item.transform.position.z);
        ItemManager.instance.SpawnItem(skillInfo[index].name,v, Quaternion.identity);
    }
}
