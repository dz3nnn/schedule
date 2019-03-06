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
    /// Контроллер работы с элементами семестров
    /// </summary>
    public class SemesterController
    {
        /// <summary>
        /// Добавление семестра
        /// </summary>
        /// <param name="semester"></param>
        public void AddSemester(Semester semester)
        {
            try
            {
                string sql = @"INSERT INTO semesters (GroupName,DateStartFirst,DateStopFirst,DateStartSecond,DateStopSecond,OnlyFirst) 
                                                VALUES (@GroupName,@DateStartFirst,@DateStopFirst,@DateStartSecond,@DateStopSecond,@OnlyFirst)";
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Execute(sql, semester);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Выгрузка всех семестров
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Semester> GetAllSemesters()
        {
            try
            {
                string sql = "SELECT * FROM semesters";
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Semester>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return null;
        }
    }
}
