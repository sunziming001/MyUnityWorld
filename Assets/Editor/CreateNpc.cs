using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


namespace MapEditor
{
	public class CreateNPCWizard : ScriptableWizard
	{

		private XmlDocument npcBriefXml;

		public RuntimeAnimatorController animatorController;
		public MonoScript ctrlScript;
		public GameObject npcPrefabObj ;
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


		[MenuItem("MapEditor/Create NPC")]
		static void CreateWizard()
		{
			ScriptableWizard.DisplayWizard<CreateNPCWizard>("Create NPC", "Create");
			
		}

		private void OnWizardCreate()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("MapInfo/NpcBrief");
			npcBriefXml = new XmlDocument();
			npcBriefXml.LoadXml(textAsset.text);
			string xmlPath = Application.dataPath + "/Resources/MapInfo/NpcBrief.xml";

			string animatorPath = GetResourcePath(AssetDatabase.GetAssetPath(animatorController));
			string npcPrefabPath = GetResourcePath(AssetDatabase.GetAssetPath(npcPrefabObj));
			string scriptTypeName = ctrlScript.GetClass().ToString();

			Debug.Log(animatorPath);
			Debug.Log(npcPrefabPath);
			Debug.Log(scriptTypeName);

		}


		private string GetResourcePath(string assetPath)
		{
			string ret ="";
			List<string> dirList = null;
			string[] pathes = assetPath.Split('/');

			if(pathes.Length >0)
			{
				string fileName = pathes[pathes.Length - 1];
				pathes[pathes.Length - 1] = fileName.Split('.')[0];

				dirList = new List<string>(pathes);
				dirList.RemoveAt(0);
				dirList.RemoveAt(0);

				for (int i = 0; i < dirList.Count; i++)
				{
					ret += dirList[i];
					if (i != dirList.Count - 1)
					{
						ret += '/';
					}
				}

			}
			else
			{
				ret = "";	
			}

			return ret;
		}

	}
}

