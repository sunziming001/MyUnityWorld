using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataBase;

namespace GameCtrl
{
	public struct SceneUnit
	{
		public string sceneName;

		public int localXIdx;
		public int localZIdx;

		public int globalXIdx;
		public int globalZIdx;

		public int unitXSize;
		public int unitZSize;

		public bool isValid()
		{
			return sceneName != null;
		}

	}

	public struct MapNodeInfo
	{
		public string name;

		public int xCount;
		public int zCount;
		public int globalXIdx;
		public int globalZIdx;
		public int unitXSize;
		public int unitZSize;
	}

	public class MapStreamManager : MonoBehaviour
	{

		private static MapStreamManager instance;
		private static string RootScene = "RootScene";
		public static MapStreamManager GetInstance() { return instance; }

		

		private bool hasInited = false;

		private MapInfoOperator mapInfoOperator = new MapInfoOperator();

		private HashSet<string> loadingSceneName = new HashSet<string>();
		private HashSet<string> loadedSceneName = new HashSet<string>();
		private HashSet<string> curActiveScenes = new HashSet<string>();
		private string sceneNameWhichPlayerIn = null;
		private delegate bool SceneUnitSearcher(MapInfo mapINfo, in List<SceneUnit> sceneUnits);


		private void Awake()
		{
			if (instance != null
				&& instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				instance = this;

			}
			Init();
		}


		void Update()
		{
			Transform playerTransfrom =GameObjManager.GetInstance().GetPlayerTransfrom();
			if(playerTransfrom != null)
			{
				SceneUnit sceneUnit = GetSceneUint(playerTransfrom.position);

				if(sceneUnit.sceneName != null)
				{
					sceneNameWhichPlayerIn = sceneUnit.sceneName;
					List<SceneUnit> neighbourScene = GetNeighbourSceneUnit(sceneUnit);
					LoadSceneUnitAnsyc(sceneUnit, neighbourScene);
				}

				TryUnloadScene();
			}
		}

		public Scene GetSceneWhichPlayerIn()
		{
			return SceneManager.GetSceneByName(sceneNameWhichPlayerIn);
		}



		private void LoadSceneUnitAnsyc(SceneUnit sceneUnit, List<SceneUnit> neighbourScene)
		{
			string sceneName = composeTrueSceneName(sceneUnit);
			List<string> neighbourSceneNames = new List<string>();
			curActiveScenes.Clear();

			for (int i = 0; i<neighbourScene.Count; i++)
			{
				SceneUnit tmpScene = neighbourScene[i];
				string name = composeTrueSceneName(tmpScene);
				neighbourSceneNames.Add(name);
			}


			string curSceneName = SceneManager.GetActiveScene().name;
			if (curSceneName != sceneUnit.sceneName)
			{
				LoadSceneAnsycAdditive(sceneName);
				curActiveScenes.Add(sceneName);
				for (int i = 0; i < neighbourSceneNames.Count; i++)
				{
					string name = neighbourSceneNames[i];
					LoadSceneAnsycAdditive(name);
					curActiveScenes.Add(name);
				}
			}

			
			
		}

		private void LoadSceneAnsycAdditive(string sceneName)
		{
			if(!loadingSceneName.Contains(sceneName)
				&& !loadedSceneName.Contains(sceneName))
			{
				loadingSceneName.Add(sceneName);
				SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			}
			
		}

		

		private void UnloadSceneAnsyc(string sceneName)
		{
			SceneManager.UnloadSceneAsync(sceneName);
		}

		private string composeTrueSceneName(SceneUnit sceneUnit)
		{
			string ret;
			ret = sceneUnit.sceneName + "-" + sceneUnit.localXIdx + "-" + sceneUnit.localZIdx;
			return ret;
		}

		private List<SceneUnit> GetNeighbourSceneUnit(SceneUnit sceneUnit)
		{
			List<SceneUnit> ret = new List<SceneUnit>();
			List<MapInfo> mapInfoList = mapInfoOperator.SelectAllMapInfo();
			if(sceneUnit.isValid())
			{
				List<Tuple<int, int>> neighbourIndxes = new List<Tuple<int, int>>();

				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx + 1, sceneUnit.globalZIdx));
				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx + 1, sceneUnit.globalZIdx + 1));
				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx + 1, sceneUnit.globalZIdx - 1));

				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx - 1, sceneUnit.globalZIdx));
				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx - 1, sceneUnit.globalZIdx + 1));
				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx - 1, sceneUnit.globalZIdx - 1));


				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx , sceneUnit.globalZIdx + 1));
				neighbourIndxes.Add(new Tuple<int, int>(sceneUnit.globalXIdx, sceneUnit.globalZIdx - 1));

				ret = SearchMapInfoList(mapInfoList,(MapInfo mapNodeInfo, in List<SceneUnit> sceneUnits) => {
					for(int i =0; i<neighbourIndxes.Count; i++)
					{
						Tuple<int, int> xAndZ = neighbourIndxes[i];
						int x = xAndZ.Item1;
						int z = xAndZ.Item2;

						if(mapNodeInfo.globalXIdx <= x
							&& mapNodeInfo.globalXIdx + mapNodeInfo.xCount -1 >= x 
							&& mapNodeInfo.globalZIdx <= z
							&& mapNodeInfo.globalZIdx + mapNodeInfo.zCount - 1 >= z)
						{
							SceneUnit unit;
							unit.sceneName = mapNodeInfo.mapName;
							unit.globalXIdx = x;
							unit.globalZIdx = z;
							unit.localXIdx = unit.globalXIdx - mapNodeInfo.globalXIdx;
							unit.localZIdx = unit.globalZIdx - mapNodeInfo.globalZIdx;
							unit.unitXSize = mapNodeInfo.unitXSize;
							unit.unitZSize = mapNodeInfo.unitZSize;

							sceneUnits.Add(unit);
						}

					}
					

					return false;
				});
			}
			return ret;
		}

		



		private SceneUnit GetSceneUint(Vector3 vector)
		{
			SceneUnit ret;
			
			int curX =(int) vector.x;
			int curZ = (int)vector.z;

			List<MapInfo> mapInfoList = mapInfoOperator.SelectAllMapInfo();
			int mapTotalCnt = mapInfoList.Count;

			ret.sceneName = null;
			ret.localXIdx = -1;
			ret.localZIdx = -1;
			ret.globalXIdx = -1;
			ret.globalZIdx = -1;
			ret.unitXSize = -1;
			ret.unitZSize = -1;


			SearchMapInfoList(mapInfoList, (MapInfo mapNodInfo, in List<SceneUnit> sceneUnits) => {
				int minX = mapNodInfo.globalXIdx * mapNodInfo.unitXSize;
				int maxX = minX + mapNodInfo.xCount * mapNodInfo.unitXSize;

				int minZ = mapNodInfo.globalZIdx * mapNodInfo.unitXSize;
				int maxZ = minZ + mapNodInfo.zCount * mapNodInfo.unitZSize;

				if (curX >= minX
					&& curX <= maxX
					&& curZ >= minZ
					&& curZ <= maxZ)
				{
					ret.sceneName = mapNodInfo.mapName;

					ret.globalXIdx = curX / mapNodInfo.unitXSize;
					ret.globalZIdx = curZ / mapNodInfo.unitZSize;

					ret.localXIdx = ret.globalXIdx - mapNodInfo.globalXIdx;
					ret.localZIdx = ret.globalZIdx - mapNodInfo.globalZIdx;

					return true;
				}
				else
				{
					return false;
				}
			
			});
			

			return ret;
		}

		private List<SceneUnit> SearchMapInfoList(List<MapInfo> mapInfoList, SceneUnitSearcher seacher)
		{
			List<SceneUnit> ret = new List<SceneUnit>();
			for(int i =0; i<mapInfoList.Count;i++)
			{
				MapInfo mapInfo = mapInfoList[i];
				if(seacher(mapInfo, in ret))
				{
					break;
				}
			}
			return ret;
		}

		private void Init()
		{
			if(hasInited)
			{
				return;
			}

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			hasInited = true;
		}


		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			loadingSceneName.Remove(scene.name);
			loadedSceneName.Add(scene.name);

			TryUnloadScene();
		}

		void TryUnloadScene()
		{
			foreach (var item in loadedSceneName)
			{
				if (!curActiveScenes.Contains(item)
					&& item != RootScene)
				{
					SceneManager.UnloadSceneAsync(item);
				}
			}
		}

		void OnSceneUnloaded(Scene scene)
		{
			loadedSceneName.Remove(scene.name);
		}
	}
}

