using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Models;
using MySql.Data.MySqlClient;

namespace MainLibrary
{
    /// <summary>
    /// Контроллер работы с элементами замен
    /// </summary>
    public class ChangeController
    {
        /// <summary>
        /// Выгрузка всех замен
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Change> GetChanges()
        {
            try
            {
                string sql = string.Format("SELECT * FROM changes");
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Change>(sql);
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
        /// Выгрузка замен по дате
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Change> GetChangesByDate(DateTime date)
        {
            try
            {
                string sql = string.Format("SELECT * FROM changes WHERE ChangeDate = @ChangeDate");
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Change>(sql,new Change { ChangeDate = date});
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
        /// Добавление замены в базу
        /// </summary>
        /// <param name="change">Добавляемая замена</param>
        public void AddChange(Change change)
        {
            try
            {
                string sql = @"INSERT INTO changes (GroupName,ChangeDate,Lesson,SubjectOn) 
                                                VALUES (@GroupName,@ChangeDate,@Lesson,@SubjectOn) ";
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Execute(sql, change);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
    }
}
