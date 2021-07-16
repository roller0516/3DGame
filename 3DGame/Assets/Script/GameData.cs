using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Data.Utility;
using Core.Data;

namespace Data
{
	public class GameData
	{
		public string SkillPath = "Assets/Data/TSV/Skill.tsv";
		public Dictionary<int,Skill> Skill;

		public GameData()
		{
			Skill  =  TableStream.LoadTableByTSV(SkillPath).TableToDictionary<int,Skill>();
		}
	}
}
