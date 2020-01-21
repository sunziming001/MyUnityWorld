using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;



namespace MapEditor
{

	public class CreateMapWizard : ScriptableWizard
	{
		private int xUnitSize = 100;
		private int zUnitSize = 100;
		private XmlDocument mapBriefXml;

		public int xUnitCount = 1;
		public int zUnitCount = 1;

		public int terrainPosX = 0;
		public int terrainPosY = 0;
		public int terrainPosZ = 0;

		public string mapName;
		public List<TerrainLayer> terrainLayers = new List<TerrainLayer>();


		[MenuItem("MapEditor/Create Map")]
		static void CreateWizard()
		{
			ScriptableWizard.DisplayWizard<CreateMapWizard>("Create Map", "Create");

			
			

		}

		void OnWizardCreate()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("MapInfo/MapBrief");
			mapBriefXml = new XmlDocument();
			mapBriefXml.LoadXml(textAsset.text);

			string xmlPath = Application.dataPath + "/Resources/MapInfo/MapBrief.xml";
			xUnitSize = Int32.Parse(mapBriefXml.GetElementsByTagName("UnitXSize").Item(0).InnerText);
			zUnitSize = Int32.Parse(mapBriefXml.GetElementsByTagName("UnitZSize").Item(0).InnerText);

			for (int i = 0; i < xUnitCount; i++)
			{
				for (int j = 0; j < zUnitCount; j++)
				{
					createSceneAndSave(mapName, i, j);
				}

			}
			

			
			//save node to xml file
			int globalXIdx = terrainPosX/ xUnitSize;
			int globalZIdx = terrainPosZ/ zUnitSize;

			XmlElement elem = mapBriefXml.CreateElement("MapNode");
			elem.SetAttribute("name", mapName);
			elem.SetAttribute("XCount", xUnitCount.ToString());
			elem.SetAttribute("ZCount", zUnitCount.ToString());
			elem.SetAttribute("GlobalXIdx", globalXIdx.ToString());
			elem.SetAttribute("GlobalZIdx", globalZIdx.ToString());

			mapBriefXml.DocumentElement.AppendChild(elem);
			mapBriefXml.Save(xmlPath);

		}


		private void createSceneAndSave(string mapName, int xIdx, int zIdx)
		{
			Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
			newScene.name =  mapName + "-" + xIdx.ToString() + "-" + zIdx.ToString();

			TerrainData terraindata = new TerrainData();
			terraindata.size = new Vector3(xUnitSize, 70, zUnitSize);
			

			
			terraindata.terrainLayers = terrainLayers.ToArray();


			GameObject terrain  = Terrain.CreateTerrainGameObject(terraindata);
			terrain.name = "Terrain-" + xIdx.ToString() + "-" + zIdx.ToString();
			TerrainCollider collider = terrain.GetComponent<TerrainCollider>();
			collider.terrainData = terraindata;

			EditorSceneManager.MoveGameObjectToScene(terrain, newScene);
			terrain.transform.position = new Vector3(terrainPosX + xIdx * xUnitSize, 0, terrainPosZ + zIdx * zUnitSize);

			string newScenePath = "Assets/Scenes/" + mapName + "/" + newScene.name + ".unity";

			if(!AssetDatabase.IsValidFolder("Assets/Scenes/"+mapName))
			{
				AssetDatabase.CreateFolder("Assets/Scenes", mapName);
			}
			
			

			EditorSceneManager.SaveScene(newScene, newScenePath);

		}

		void OnWizardUpdate()
		{

		}


	}
}