using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Dapper;
using Models;
using MySql.Data.MySqlClient;

namespace MainLibrary
{
    /// <summary>
    /// Контроллер работы с элементами справочников
    /// </summary>
    public class BookController
    {
        /// <summary>
        /// Типы справочников
        /// </summary>
        public enum BookTypes
        {
            teachers,
            subjects,
            rooms,
            groups
        }
        /// <summary>
        /// Выгрузка всех элементов справочника
        /// </summary>
        /// <param name="bk">Тип справочника</param>
        /// <returns></returns>
        public IEnumerable<BookItem> GetBookItems(BookTypes bk)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} ORDER BY Name",bk.ToString());
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<BookItem>(sql);
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
        /// Удаление элемента справочника по id
        /// </summary>
        /// <param name="id">Id справочника</param>
        /// <param name="bk">Тип справочника</param>
        public void DeleteBookItem(int id, BookTypes bk)
        {
            try
            {
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    con.Execute(string.Format("DELETE FROM {0} WHERE Id = @Id",bk.ToString()), new BookItem { Id = id });
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Добавление элемента в справочник
        /// </summary>
        /// <param name="bi">Добавляемый элемент</param>
        /// <param name="btype">Тип справочника</param>
        public void AddBookItem(BookItem bi,string btype)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO {0} (Name) VALUES (@Name)",btype);
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Execute(sql, bi);
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
    }
}
