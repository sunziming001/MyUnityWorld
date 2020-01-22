using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using DataBase;

namespace DataBase
{
	public class MapInfoOperator
	{
		private const String TABLE_NAME = "MapBrief";
		private const String KEY_MAPNAME = "mapName";
		private const String KEY_XCOUNT = "xCount";
		private const String KEY_ZCOUNT = "zCount";
		private const String KEY_GlobalXIdx = "globalXIdx";
		private const String KEY_GlobalZIdx = "globalZIdx";
		private const String KEY_UNITXSIZE = "unitXSize";
		private const String KEY_UNITZSIZE = "unitZSize";
		private const String KEY_DATE = "createDate";

		private String[] COLUMNS = new String[] { KEY_MAPNAME, KEY_XCOUNT, KEY_ZCOUNT, KEY_GlobalXIdx, KEY_GlobalZIdx };


		public MapInfoOperator()
		{
			
		}
		public void Init()
		{
			CreateTableIfNotExist();
		}

		public void CreateTableIfNotExist()
		{
			SqliteHelper sqlHelper = SqliteHelper.GetInstance();
			sqlHelper.Open();

			IDbCommand dbcmd = sqlHelper.getDbCommand();

			dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
				KEY_MAPNAME + " TEXT PRIMARY KEY , " +
				KEY_XCOUNT + " INTEGER, " +
				KEY_ZCOUNT + " INTEGER, " +
				KEY_GlobalXIdx + " INTEGER, " +
				KEY_GlobalZIdx + " INTEGER, " +
				KEY_UNITXSIZE + " INTEGER, " +
				KEY_UNITZSIZE + " INTEGER, " +
				KEY_DATE + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

			dbcmd.ExecuteNonQuery();

			sqlHelper.Close();
		}

		public void InsertMap(MapInfo mapInfo)
		{
			SqliteHelper sqlHelper = SqliteHelper.GetInstance();
			sqlHelper.Open();

			IDbCommand dbcmd = sqlHelper.getDbCommand();

			dbcmd.CommandText =
				"INSERT INTO " + TABLE_NAME
				+ " ( "
				+ KEY_MAPNAME + ", "
				+ KEY_XCOUNT + ", "
				+ KEY_ZCOUNT + ", "
				+ KEY_GlobalXIdx + ", "
				+ KEY_GlobalZIdx + ", "
				+ KEY_UNITXSIZE + ", "
				+ KEY_UNITZSIZE + " ) "

				+ "VALUES ( '"
				+ mapInfo.mapName + "', '"
				+ mapInfo.xCount + "', '"
				+ mapInfo.zCount + "', '"
				+ mapInfo.globalXIdx + "', '"
				+ mapInfo.globalZIdx + "', '"
				+ mapInfo.unitXSize+ "', '"
				+ mapInfo.unitZSize+"'" 
				+" )";

			dbcmd.ExecuteNonQuery();

			sqlHelper.Close();
		}

		public List<MapInfo> SelectAllMapInfo()
		{
			List < MapInfo > ret= new List<MapInfo>();
			SqliteHelper sqlHelper = SqliteHelper.GetInstance();
			sqlHelper.Open();
			IDbCommand dbcmd = sqlHelper.getDbCommand();
			dbcmd.CommandText = "Select "
				+ KEY_MAPNAME + ", "
				+ KEY_XCOUNT + ", "
				+ KEY_ZCOUNT + ", "
				+ KEY_GlobalXIdx + ", "
				+ KEY_GlobalZIdx + ", "
				+ KEY_UNITXSIZE + ", "
				+ KEY_UNITZSIZE 
				+ " FROM "+ TABLE_NAME;

			IDataReader reader =  dbcmd.ExecuteReader();
			while (reader.Read())
			{
				MapInfo mapInfo = new MapInfo();

				mapInfo.mapName = reader[0].ToString();
				mapInfo.xCount = Int32.Parse( reader[1].ToString());
				mapInfo.zCount = Int32.Parse(reader[2].ToString());
				mapInfo.globalXIdx = Int32.Parse(reader[3].ToString());
				mapInfo.globalZIdx = Int32.Parse(reader[4].ToString());
				mapInfo.unitXSize = Int32.Parse(reader[5].ToString());
				mapInfo.unitZSize = Int32.Parse(reader[6].ToString());

				ret.Add(mapInfo);
			}

			sqlHelper.Close();
			return ret;
		}
	}
}
