using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using DataBase;


namespace MapEditor
{

	public class CreateMapWizard : ScriptableWizard
	{
		public int xUnitSize = 100;
		public int zUnitSize = 100;

		public int xUnitCount = 1;
		public int zUnitCount = 1;

		public int terrainPosX = 0;
		public int terrainPosY = 0;
		public int terrainPosZ = 0;

		public string mapName;
		public List<TerrainLayer> terrainLayers = new List<TerrainLayer>();
		private MapInfoOperator mapInfoOperator = new MapInfoOperator();

		[MenuItem("MapEditor/Create Map")]
		static void CreateWizard()
		{

			ScriptableWizard.DisplayWizard<CreateMapWizard>("Create Map", "Create");
		}

		void OnWizardCreate()
		{
			mapInfoOperator.Init();
			for (int i = 0; i < xUnitCount; i++)
			{
				for (int j = 0; j < zUnitCount; j++)
				{
					//createSceneAndSave(mapName, i, j);
				}

			}

			int globalXIdx = terrainPosX/ xUnitSize;
			int globalZIdx = terrainPosZ/ zUnitSize;

			MapInfo mapInfo = new MapInfo();
			mapInfo.mapName = mapName;
			mapInfo.xCount = xUnitCount;
			mapInfo.zCount = zUnitCount;
			mapInfo.globalXIdx = globalXIdx;
			mapInfo.globalZIdx = globalZIdx;
			mapInfo.unitXSize = xUnitSize;
			mapInfo.unitZSize = zUnitSize;

			mapInfoOperator.InsertMap(mapInfo);

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