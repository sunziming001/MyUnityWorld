
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using DataBase;

public class PostBuildProcesser
{
	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			try
			{
				FileUtil.CopyFileOrDirectory(
					SqliteHelper.GetInstance().GetDBAbsoluteFilePath(),

					getPathOfFile(pathToBuiltProject)+"/"
					+ Application.productName+"_Data" +"/"
					+ SqliteHelper.GetInstance().GetDBRelativeFilePath());
			}catch
			{
				Debug.LogError("game data base not copyed");
			}
			
		}
		else
		{
			Debug.LogWarning("game data base not copyed");
		}
			
	}

	public static string getPathOfFile(string file)
	{
		string ret = "";

		string[] units = file.Split('/');
		for(int i =0; i<units.Length -1; i++)
		{
			ret += units[i];

			if(i!= units.Length-2)
			{
				ret += "/";
			}
		}

		return ret;
	}
}


