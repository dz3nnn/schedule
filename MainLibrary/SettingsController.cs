using System;
using System.Collections.Generic;
using Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Configuration;

namespace MainLibrary
{
    /// <summary>
    /// Контроллер работы с элементами настроек
    /// </summary>
    public class SettingsController
    {
        /// <summary>
        /// Выгрузка настроек для группы
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns></returns>
        public IEnumerable<Settings> GetSettingsForGroup(string group)
        {
            try
            {
                string sql = string.Format("SELECT * FROM settings WHERE GroupName = @GroupName", group);
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Settings>(sql,new Settings {GroupName = group });
                    return result;
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Выгрузка настроек для группы (2 часа в день)
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns></returns>
        public IEnumerable<Settings> GetSettingsCoupleForGroup(string group)
        {
            try
            {
                string sql = string.Format("SELECT * FROM settings WHERE GroupName = @GroupName AND HoursDay = 2", group);
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Settings>(sql, new Settings { GroupName = group });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Добавление элемента настроек
        /// </summary>
        /// <param name="settings"></param>
        public void AddSettings(Settings settings)
        {
            try
            {
                string sql = @"INSERT INTO settings (GroupName,Subject,Teacher1,Teacher2,Room,Hours,HoursAll,HoursDay) VALUES (@GroupName, @Subject, @Teacher1, @Teacher2, @Room, @Hours,@HoursAll,@HoursDay)";
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Execute(sql, settings);
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Удаление элемента настроек по Id
        /// </summary>
        /// <param name="id">Id</param>
        public void DeleteSettings (int id)
        {
            try
            {
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    con.Execute("DELETE FROM settings WHERE Id = @Id",new Settings {Id=id });
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
    }
}
