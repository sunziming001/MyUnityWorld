using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;

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
		public static MapStreamManager GetInstance() { return instance; }

		

		private bool hasInited = false;

		private string mapBriefUrl;
		private XmlDocument mapBriefXml;
		private Dictionary<string, XmlDocument> mapSceneName2XmlDoc = new Dictionary<string, XmlDocument>();

		private HashSet<string> loadingSceneName = new HashSet<string>();
		private HashSet<string> loadedSceneName = new HashSet<string>();

		private delegate bool SceneUnitSearcher(MapNodeInfo mapNodInfo, in List<SceneUnit> sceneUnits);

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
					List<SceneUnit> neighbourScene = GetNeighbourSceneUnit(sceneUnit);
					LoadSceneUnitAnsyc(sceneUnit, neighbourScene);
				}
			}
		}


		private void LoadSceneUnitAnsyc(SceneUnit sceneUnit, List<SceneUnit> neighbourScene)
		{
			string sceneName = composeTrueSceneName(sceneUnit);
			List<string> neighbourSceneNames = new List<string>();
			for(int i = 0; i<neighbourScene.Count; i++)
			{
				SceneUnit tmpScene = neighbourScene[i];
				string name = composeTrueSceneName(tmpScene);
				neighbourSceneNames.Add(name);
			}


			LoadSceneAnsycAdditive(sceneName);

			for (int i = 0; i < neighbourSceneNames.Count; i++)
			{
				string name = neighbourSceneNames[i];
				LoadSceneAnsycAdditive(name);
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

				ret = SearchMapBriefXml((MapNodeInfo mapNodeInfo, in List<SceneUnit> sceneUnits) => {
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
							unit.sceneName = mapNodeInfo.name;
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

			int mapTotalCnt = mapBriefXml.GetElementsByTagName("MapNode").Count;
			int unitXSize = Int32.Parse( mapBriefXml.GetElementsByTagName("UnitXSize").Item(0).InnerText);
			int unitZSize = Int32.Parse(mapBriefXml.GetElementsByTagName("UnitZSize").Item(0).InnerText);

			ret.sceneName = null;
			ret.localXIdx = -1;
			ret.localZIdx = -1;
			ret.globalXIdx = -1;
			ret.globalZIdx = -1;
			ret.unitXSize = unitXSize;
			ret.unitZSize = unitZSize;


			SearchMapBriefXml((MapNodeInfo mapNodInfo, in List<SceneUnit> sceneUnits) => {
				int minX = mapNodInfo.globalXIdx * mapNodInfo.unitXSize;
				int maxX = minX + mapNodInfo.xCount * mapNodInfo.unitXSize;

				int minZ = mapNodInfo.globalZIdx * mapNodInfo.unitZSize;
				int maxZ = minZ + mapNodInfo.zCount * mapNodInfo.unitZSize;

				if (curX >= minX
					&& curX <= maxX
					&& curZ >= minZ
					&& curZ <= maxZ)
				{
					ret.sceneName = mapNodInfo.name;

					ret.globalXIdx = curX / unitXSize;
					ret.globalZIdx = curZ / unitZSize;

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

		private List<SceneUnit> SearchMapBriefXml( SceneUnitSearcher seacher)
		{
			List<SceneUnit> ret = new List<SceneUnit>();
			int mapTotalCnt = mapBriefXml.GetElementsByTagName("MapNode").Count;
			int unitXSize = Int32.Parse(mapBriefXml.GetElementsByTagName("UnitXSize").Item(0).InnerText);
			int unitZSize = Int32.Parse(mapBriefXml.GetElementsByTagName("UnitZSize").Item(0).InnerText);

			for (int i = 0; i < mapTotalCnt; i++)
			{
				XmlNode node = mapBriefXml.GetElementsByTagName("MapNode").Item(i);
				XmlElement element = (XmlElement)node;
				string mapName = element.GetAttribute("name");
				int xCount = Int32.Parse(element.GetAttribute("XCount"));
				int zCount = Int32.Parse(element.GetAttribute("ZCount"));
				int xIdx = Int32.Parse(element.GetAttribute("GlobalXIdx"));
				int zIdx = Int32.Parse(element.GetAttribute("GlobalZIdx"));

				MapNodeInfo mapNodeInfo;
				mapNodeInfo.name = mapName;
				mapNodeInfo.xCount = xCount;
				mapNodeInfo.zCount = zCount;
				mapNodeInfo.globalXIdx = xIdx;
				mapNodeInfo.globalZIdx = zIdx;
				mapNodeInfo.unitXSize = unitXSize;
				mapNodeInfo.unitZSize = unitZSize;

				if(seacher(mapNodeInfo, in ret))
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

			mapBriefUrl = Application.dataPath+ "/Scenes/MapBrief.xml";
			mapBriefXml = new XmlDocument();
			mapBriefXml.Load(mapBriefUrl);

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			hasInited = true;
		}


		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			loadingSceneName.Remove(scene.name);
			loadedSceneName.Add(scene.name);

		}

		void OnSceneUnloaded(Scene scene)
		{
			loadedSceneName.Remove(scene.name);
		}
	}
}

