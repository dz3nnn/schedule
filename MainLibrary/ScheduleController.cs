using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace MainLibrary
{
    /// <summary>
    /// Контроллер работы с элементами расписания
    /// </summary>
    public class ScheduleController
    {
        SettingsController sc = new SettingsController();
        /// <summary>
        /// Выгрузка расписания для группы
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns></returns>
        public IEnumerable<Schedule> GetScheduleForGroup(string group)
        {
            try
            {
                string sql = string.Format("SELECT * FROM schedule WHERE GroupName = @GroupName", group);
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Schedule>(sql,new Schedule {GroupName = group });
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
        /// Удаление элемента расписания по Id
        /// </summary>
        /// <param name="id">Id расписания</param>
        public void DeleteById(int id)
        {
            try
            {
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    con.Execute("DELETE FROM schedule WHERE Id = @Id", new Schedule { Id = id });
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// Выгрузка всего расписания
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Schedule> GetFullSchedule()
        {
            try
            {
                string sql = string.Format("SELECT * FROM schedule");
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Schedule>(sql);
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
        /// Подсчет всех часов в неделю из настроек
        /// </summary>
        /// <param name="settings">Настройки</param>
        /// <returns></returns>
        public int CalcHours(IEnumerable<Settings> settings)
        {
            int res = 0;
            foreach (Settings set in settings)
                res += set.Hours;
            return res;
        }
        /// <summary>
        /// Формирование расписания для группы
        /// </summary>
        /// <param name="group">Группа</param>
        public void CreateScheduleForGroup(string group)
        {
            try
            {
                var settings = sc.GetSettingsCoupleForGroup(group);
                for (int i = 1; i < +6; i++)
                    InputSchedule(new Schedule { GroupName = group, Subject = "Обед", Lesson = CheckCourse(group), WeekDay = i });
                while (CalcHours(settings) != 0)
                {
                    foreach (Settings set in settings)
                    {
                        int t = 0;
                        var days = FindMinDay(group);
                        foreach (int day in days)
                        {
                            int lesson = 1;
                            while (((lesson < 11 && day<6)||(lesson<4 && day==6)) && t < 2 && set.Hours > 0)
                            {
                                if (CanInputHere(new Schedule { Lesson = lesson, WeekDay = day, Room = set.Room, Teacher1 = set.Teacher1, GroupName = set.GroupName }) &&
                                        CanInputHere(new Schedule { Lesson = lesson + 1, WeekDay = day, Room = set.Room, Teacher1 = set.Teacher1, GroupName = set.GroupName }))
                                {
                                    Schedule sch = new Schedule
                                    {
                                        GroupName = set.GroupName,
                                        Lesson = lesson,
                                        Room = set.Room,
                                        Subject = set.Subject,
                                        Teacher1 = set.Teacher1,
                                        Teacher2 = set.Teacher2,
                                        WeekDay = day
                                    };
                                    InputSchedule(sch);
                                    sch.Lesson++;
                                    InputSchedule(sch);
                                    t = 2;
                                    set.Hours -= 2;
                                }
                                lesson++;
                            }
                        }
                    }
                }
                DeleteLastDinners(group);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Удаление обедов последним уроков для группы
        /// </summary>
        /// <param name="group">Группа</param>
        public void DeleteLastDinners(string group)
        {
            try
            {
                var schedule = GetScheduleForGroup(group);
                for (int i = 1; i <= 6; i++)
                {
                    Schedule max = new Schedule { Lesson = -1 };
                    foreach (Schedule sch in schedule)
                    {
                        if (sch.WeekDay == i && sch.Lesson > max.Lesson)
                            max = sch;
                    }
                    if (max.Lesson != -1 && max.Subject == "Обед")
                        DeleteById(max.Id);
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Проверка курса для формирования обедов
        /// </summary>
        /// <param name="st">Группа</param>
        /// <returns></returns>
        public int CheckCourse(string st)
        {
            try
            {
                string[] arr = st.Split('-');
                if (int.Parse(arr[1][0].ToString()) < 3)
                    return 5;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return 6;
        }
        /// <summary>
        /// Проверка можно ли вставить это расписание
        /// </summary>
        /// <param name="sch">Вставляемое расписание</param>
        /// <returns></returns>
        public bool CanInputHere(Schedule sch)
        {
            try
            {
                string sql = string.Format("SELECT * FROM schedule WHERE Lesson = @Lesson AND WeekDay = @WeekDay AND (Room = @Room OR Teacher1 = @Teacher1 OR GroupName = @GroupName)");
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Schedule>(sql,sch);
                    if (result.Count() == 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Добавление элемента расписания
        /// </summary>
        /// <param name="sch">Расписание</param>
        public void InputSchedule(Schedule sch)
        {
            try
            {
                string sql = @"INSERT INTO schedule (Lesson, WeekDay, Subject, Room, GroupName, Teacher1, Teacher2)
                                VALUES (@Lesson, @WeekDay, @Subject, @Room, @GroupName, @Teacher1, @Teacher2)";
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Execute(sql, sch);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
        }
        /// <summary>
        /// Подсчет нагрузки по дням для группы
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns>Список дней по возрастанию загруженности</returns>
        public IEnumerable<int> FindMinDay(string group)
        {
            try
            {
                var schedule = GetScheduleForGroup(group);
                int[] times = new int[6];
                int[] indexes = new int[6];
                List<int> result = new List<int>();
                foreach (Schedule sch in schedule)
                    times[sch.WeekDay - 1]++;
                for (int i = 1; i <= 6; i++) indexes[i-1] = i;
                Array.Sort(times,indexes);
                return indexes.ToList();
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Выгрузка нераспределенных настроек
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns></returns>
        public IEnumerable<Settings> GetUnallocatedSubjects(string group)
        {
            try
            {
                List<Settings> result = new List<Settings>();
                var settings = sc.GetSettingsForGroup(group);
                var schedule = GetScheduleForGroup(group);
                foreach (Settings set in settings)
                {
                    foreach(Schedule sch in schedule)
                    {
                        if (set.Subject == sch.Subject)
                            set.Hours--;
                    }
                    if (set.Hours > 0)
                        result.Add(set);
                }
                if (result.Count() == 0)
                    return null;
                return result;
            }
            catch(Exception ex)
            {
                Log.WriteLog(ex.Message);
            }
            return null;
        }

        public IEnumerable<Schedule> GetScheduleForGroupByDay(string group, int day)
        {
            try
            {
                string sql = string.Format("SELECT * FROM schedule WHERE GroupName = @GroupName AND WeekDay = @WeekDay", group);
                using (IDbConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    var result = con.Query<Schedule>(sql, new Schedule { GroupName = group , WeekDay = day});
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
