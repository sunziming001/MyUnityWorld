using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Data;

namespace DataBase
{
	public class SqliteHelper
	{
		private static SqliteHelper instance = null;

		private const string database_name = "game_db";

		public string db_connection_string;
		public IDbConnection db_connection = null;

		private SqliteHelper()
		{

			db_connection_string = "URI=file:" + GetDBAbsoluteFilePath();
			db_connection = new SqliteConnection(db_connection_string);

		}
		public string GetDBAbsoluteFilePath()
		{
			return Application.dataPath + "/"+GetDBRelativeFilePath();
		}

		public string GetDBRelativeFilePath()
		{
			return "Resources/" + database_name;
		}

		~SqliteHelper()
		{
			
		}

		public void Open()
		{


			if (db_connection.State != ConnectionState.Open)
			{
				db_connection.Open();
			}
			
		}

		public void Close()
		{
			if (db_connection.State != ConnectionState.Closed)
			{
				db_connection.Close();
			}
			
		}

		public static SqliteHelper GetInstance()
		{
			if (instance == null)
				instance = new SqliteHelper();

			return instance;
		}

		//helper functions
		public IDbCommand getDbCommand()
		{
			return db_connection.CreateCommand();
		}

		public IDataReader getAllData(string table_name)
		{
			IDbCommand dbcmd = db_connection.CreateCommand();
			dbcmd.CommandText =
				"SELECT * FROM " + table_name;
			IDataReader reader = dbcmd.ExecuteReader();
			return reader;
		}

		public void deleteAllData(string table_name)
		{
			IDbCommand dbcmd = db_connection.CreateCommand();
			dbcmd.CommandText = "DROP TABLE IF EXISTS " + table_name;
			dbcmd.ExecuteNonQuery();
		}

		public IDataReader getNumOfRows(string table_name)
		{
			IDbCommand dbcmd = db_connection.CreateCommand();
			dbcmd.CommandText =
				"SELECT COALESCE(MAX(id)+1, 0) FROM " + table_name;
			IDataReader reader = dbcmd.ExecuteReader();
			return reader;
		}

		public void close()
		{
			db_connection.Close();
		}
	}
}