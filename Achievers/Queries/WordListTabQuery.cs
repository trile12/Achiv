using Achievers.Models;
using Achievers.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Threading.Tasks;

namespace Achievers.Queries
{
    public class WordListTabQuery
    {
        public async Task<List<WordModel>> GetAllWord()
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<WordModel>();
            var result = await db.ExecuteReturnData(
                      @"select a.id,a.word,a.word_form,a.word_vi,a.phonetic,a.meaning,a.example,a.image_url,a.audio_url,a.grade_unit_id,a.deleted,b.isfavorite,b.memos from VOCABULARY a
                        left join USAGEINFO b on a.id=b.idVocab and b.username='" + App.UserId + "'");

            while (result.Read())
            {
                WordModel word = new WordModel();
                word.id = int.Parse(result["id"].ToString());
                word.word = result["word"].ToString();
                word.word_form = result["word_form"].ToString();
                word.word_vi = result["word_vi"].ToString();
                word.phonetic = result["phonetic"].ToString();
                word.meaning = result["meaning"].ToString();
                word.example = result["example"].ToString();
                word.image_url = result["image_url"] != DBNull.Value ? result["image_url"].ToString() : string.Empty;
                word.audio_url = result["audio_url"] != DBNull.Value ? result["audio_url"].ToString() : string.Empty;
                word.grade_unit_id = result["grade_unit_id"] != DBNull.Value ? int.Parse(result["grade_unit_id"].ToString()) : 0;
                word.deleted = result["deleted"] != DBNull.Value ? int.Parse(result["deleted"].ToString()) : 0;

                word.is_favorite = result["isfavorite"] != DBNull.Value ? int.Parse(result["isfavorite"].ToString()) : 0;
                word.memo = result["memos"].ToString();

                list.Add(word);
            }

            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        public async Task<List<WordModel>> GetAllVocabByUnitId(int unitId)
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<WordModel>();
            var result = await db.ExecuteReturnData(
                      @"select a.id,a.word,a.word_form,a.word_vi,a.phonetic,a.meaning,a.example,a.image_url,a.audio_url,a.grade_unit_id,a.deleted,b.isfavorite,b.memos from VOCABULARY a
                        left join USAGEINFO b on a.id=b.idVocab and b.username='" + App.UserId + "' " +
                        "where a.grade_unit_id=" + unitId);

            while (result.Read())
            {
                WordModel word = new WordModel();
                word.id = int.Parse(result["id"].ToString());
                word.word = result["word"].ToString();
                word.word_form = result["word_form"].ToString();
                word.word_vi = result["word_vi"].ToString();
                word.phonetic = result["phonetic"].ToString();
                word.meaning = result["meaning"].ToString();
                word.example = result["example"].ToString();
                word.image_url = result["image_url"] != DBNull.Value ? result["image_url"].ToString() : string.Empty;
                word.audio_url = result["audio_url"] != DBNull.Value ? result["audio_url"].ToString() : string.Empty;
                word.grade_unit_id = result["grade_unit_id"] != DBNull.Value ? int.Parse(result["grade_unit_id"].ToString()) : 0;
                word.deleted = result["deleted"] != DBNull.Value ? int.Parse(result["deleted"].ToString()) : 0;

                word.is_favorite = result["isfavorite"] != DBNull.Value ? int.Parse(result["isfavorite"].ToString()) : 0;
                word.memo = result["memos"].ToString();

                list.Add(word);
            }

            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        public async Task<List<WordModel>> GetWordDetail(int id)
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<WordModel>();
            var result = await db.ExecuteReturnData(
                      @"select a.id,a.word,a.word_form,a.word_vi,a.phonetic,a.meaning,a.example,a.image_url,a.grade_unit_id,a.deleted,b.isfavorite,b.memos from VOCABULARY a
                        left join USAGEINFO b on a.id=b.idVocab and b.username='" + App.UserId + "'");

            while (result.Read())
            {
                WordModel word = new WordModel();
                word.id = int.Parse(result["id"].ToString());
                word.word = result["word"].ToString();
                word.word_form = result["word_form"].ToString();
                word.word_vi = result["word_vi"].ToString();
                word.phonetic = result["phonetic"].ToString();
                word.meaning = result["meaning"].ToString();
                word.example = result["example"].ToString();
                word.image_url = result["image_url"].ToString();
                word.grade_unit_id = int.Parse(result["grade_unit_id"].ToString());
                word.deleted = int.Parse(result["deleted"].ToString());

                word.is_favorite = int.Parse(result["isfavorite"].ToString());
                word.memo = result["memos"].ToString();

                list.Add(word);
            }

            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        public async Task<int> UpdateFavorite(int idVocab, int status)
        {
            try
            {
                int result = 0;
                DbService db = new DbService();
                db.Open();

                var query = @"select * from USAGEINFO where idVocab=" + idVocab + " and username='" + App.UserId + "'";
                var isExist = await db.ExecuteReturnData(query);

                var listParam = new SqlCeParameter[4];
                listParam[0] = new SqlCeParameter("@idVocab", idVocab);
                listParam[1] = new SqlCeParameter("@isfavorite", status);
                listParam[2] = new SqlCeParameter("@username", App.UserId);
                listParam[3] = new SqlCeParameter("@active", 1);

                if (isExist.Read())
                {
                    result = await db.ExecuteNonQuery(@"update USAGEINFO set isfavorite=@isfavorite, active=@active where idVocab=@idVocab", listParam);
                }
                else
                {
                    result = await db.ExecuteNonQuery(@"insert into USAGEINFO(idVocab,isfavorite,username,active)
                                                    values(@idVocab,@isfavorite,@username,@active)", listParam);
                }

                db.Close();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateMemos(int idVocab, string content) // xoá  Memos -> content=""
        {
            try
            {
                int result = 0;
                DbService db = new DbService();
                db.Open();

                var query = @"select * from USAGEINFO where idVocab=" + idVocab + " and username='" + App.UserId + "'";
                var isExist = await db.ExecuteReturnData(query);

                var listParam = new SqlCeParameter[5];
                listParam[0] = new SqlCeParameter("@idVocab", idVocab);
                listParam[1] = new SqlCeParameter("@ismemos", Convert.ToInt32(!string.IsNullOrEmpty(content)));
                listParam[2] = new SqlCeParameter("@memos", content);
                listParam[3] = new SqlCeParameter("@username", App.UserId);
                listParam[4] = new SqlCeParameter("@active", 1);

                if (isExist.Read())
                {
                    result = await db.ExecuteNonQuery(@"update USAGEINFO set memos=@memos,ismemos=@ismemos, active=@active where idVocab=@idVocab and username=@username", listParam);
                }
                else
                {
                    result = await db.ExecuteNonQuery(@"insert into USAGEINFO(idVocab,isfavorite,ismemos,memos,username,active)
                                                    values(@idVocab,0,1,@memos,@username,@active)", listParam);
                }

                db.Close();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region synce

        public async Task<List<UsageInfoModel>> GetUsageUpdate()
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<UsageInfoModel>();
            var result = await db.ExecuteReturnData(
                      @"select * from usageinfo where active=1");

            while (result.Read())
            {
                UsageInfoModel word = new UsageInfoModel();
                word.id = int.Parse(result["id"].ToString());
                word.idVocab = int.Parse(result["idVocab"].ToString());
                word.isfavorite = int.Parse(result["isfavorite"].ToString());
                word.ismemos = result["ismemos"] != DBNull.Value ? int.Parse(result["ismemos"].ToString()) : 0;
                word.memos = result["memos"].ToString();

                list.Add(word);
            }

            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        #endregion synce
    }

    public class UsageInfoModel
    {
        public int id { get; set; }
        public int idVocab { get; set; }
        public int isfavorite { get; set; }
        public int ismemos { get; set; }
        public string memos { get; set; }
        public string username { get; set; }
        public int active { get; set; }
    }
}