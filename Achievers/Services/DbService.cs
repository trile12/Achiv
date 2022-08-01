using Achievers.Init;
using Achievers.Models;
using Achievers.Queries;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace Achievers.Services
{
    public class DbService
    {
        private static string dbPath = Environment.CurrentDirectory + @"\Assets\Db\Achievers.sdf";
        private static string ConnectionString = @"Data Source=" + dbPath + ";Password=achievers@123; Max Database Size=4091;Case Sensitive=True;";
        private static SqlCeConnection conn = new SqlCeConnection(ConnectionString);
        private static Timer timer = new Timer();

        static DbService()
        {
            timer.AutoReset = false;
            timer.Interval = 10000;
            timer.Elapsed += (e, s) =>
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            };
        }

        public static void ConditionallyCreateTables()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"\Assets\Db"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Assets\Db");
            }
            if (!File.Exists(dbPath))
            {
                using (var engine = new SqlCeEngine(ConnectionString))
                {
                    engine.CreateDatabase();
                }
            }
            using (var connection = new SqlCeConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCeCommand())
                {
                    command.Connection = connection;

                    if (!TableExists(connection, "UNIT"))
                    {
                        command.CommandText = "CREATE TABLE [UNIT] (id int PRIMARY KEY,name nvarchar(255), title nvarchar(255), title_vi nvarchar(255), image_url nvarchar(255), isfree int,deleted int)";
                        command.ExecuteNonQuery();

                        InitDataService.ImportUnitModel();
                    }
                    if (!TableExists(connection, "REVIEW"))
                    {
                        command.CommandText = "CREATE TABLE [REVIEW] (id int PRIMARY KEY, name nvarchar(255), image_url nvarchar(255), progress int, min_progress int,idunit int,active int)";
                        command.ExecuteNonQuery();

                        InitDataService.ImportReviewModel();
                    }
                    if (!TableExists(connection, "VOCABULARY"))
                    {
                        command.CommandText = "CREATE TABLE [VOCABULARY] (id int PRIMARY KEY,word nvarchar(255), word_form nvarchar(255), word_vi nvarchar(255), phonetic nvarchar(255), meaning nvarchar(255), example nvarchar(255) , image_url nvarchar(255) , audio_url nvarchar(255), grade_unit_id int, deleted int, word_parts nvarchar(255))";
                        command.ExecuteNonQuery();

                        InitDataService.ImportVoCabModel();
                    }
                    if (!TableExists(connection, "USAGEINFO"))
                    {
                        command.CommandText = "CREATE TABLE [USAGEINFO] (id int IDENTITY(1, 1) PRIMARY KEY, idVocab int, isfavorite int,ismemos int, memos nvarchar(255), username nvarchar(255), active int)";
                        command.ExecuteNonQuery();
                    }
                    //if (!TableExists(connection, "MEMOS"))
                    //{
                    //    command.CommandText = "CREATE TABLE [MEMOS] (id int PRIMARY KEY, content nvarchar(255))";
                    //    command.ExecuteNonQuery();
                    //}
                }
            }
        }

        public static async void SynceData()
        {
            //InitDataService.ImportUnitModel();
            //InitDataService.ImportVoCabModel();
            ConditionallyCreateTables();
            UnitTabQuery unitQuery = new UnitTabQuery();
            var lstReivew = await unitQuery.GetAllReviewUpdate();

            if (lstReivew.Count > 0)
            {
                foreach (var item in lstReivew)
                {
                    await HttpService.SendProgress(new
                    {
                        unit_review_id = item.id,
                        userId = App.UserId,
                        progress = item.progress
                    });
                }
            }

            WordListTabQuery wordListQuery = new WordListTabQuery();
            var lstUsage = await wordListQuery.GetUsageUpdate();

            if (lstUsage.Count > 0)
            {
                foreach (var item in lstUsage)
                {
                    if (item.ismemos == 1)
                    {
                        await HttpService.CreateMemos(new
                        {
                            userId = App.UserId,
                            unit_vocab_id = item.idVocab,
                            content = item.memos
                        });
                    }
                    else
                    {
                        await HttpService.DeleteMemos(new
                        {
                            userId = App.UserId,
                            unit_vocab_id = item.idVocab,
                        });
                    }

                    if (item.isfavorite == 1)
                    {
                        await HttpService.CreateFavorite(new
                        {
                            userId = App.UserId,
                            unit_vocab_id = item.idVocab
                        });
                    }
                    else
                    {
                        await HttpService.DeleteFavorite(new
                        {
                            userId = App.UserId,
                            unit_vocab_id = item.idVocab
                        });
                    }
                }
            }
            await InitDataService.UpdateUsageModel();
            await InitDataService.UpdateReviewModel();
        }

        public static async Task<GameModel> GetGameModel(int id)
        {
            string json = File.ReadAllText(Environment.CurrentDirectory + @"\Assets\GameModel\g" + id + ".json");
            return JsonConvert.DeserializeObject<GameModel>(json);
        }

        public static async void CreateGameModel()
        {
            for (int i = 1; i <= 40; i++)
            {
                await HttpService.GetGameModelById(i);
            }
        }

        public static bool TableExists(SqlCeConnection connection, string tableName)
        {
            using (var command = new SqlCeCommand())
            {
                command.Connection = connection;
                var sql = string.Format(
                        "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{0}'",
                         tableName);
                command.CommandText = sql;
                var count = Convert.ToInt32(command.ExecuteScalar());
                return (count > 0);
            }
        }

        public void Open()
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public void Close()
        {
            timer.Stop();
            timer.Start();
        }

        public Task<DbDataReader> ExecuteReturnData(string query, SqlCeParameter[] listParam = null)
        {
            var cmd = new SqlCeCommand();
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (listParam != null && listParam.Length > 0)
                {
                    foreach (var param in listParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                return cmd.ExecuteReaderAsync();
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public Task<int> ExecuteNonQuery(string query, SqlCeParameter[] listParam = null)
        {
            var cmd = new SqlCeCommand();
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (listParam != null && listParam.Length > 0)
                {
                    foreach (var param in listParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                return cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}