using Achievers.Queries;
using Achievers.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Threading.Tasks;

namespace Achievers.Init
{
    public class InitDataService
    {
        public static async void ImportUnitModel()
        {
            try
            {
                var lstUnitModel = await HttpService.GetAllUnitByUser();
                DbService db = new DbService();
                db.Open();
                foreach (var item in lstUnitModel)
                {
                    var listParam = new SqlCeParameter[7];
                    listParam[0] = new SqlCeParameter("@id", item.id);
                    listParam[1] = new SqlCeParameter("@name", item.name == null ? string.Empty : item.name);
                    listParam[2] = new SqlCeParameter("@title", item.title);
                    listParam[3] = new SqlCeParameter("@title_vi", item.title_vi);
                    listParam[4] = new SqlCeParameter("@image_url", item.image_url);
                    listParam[5] = new SqlCeParameter("@isfree", item.is_free);
                    listParam[6] = new SqlCeParameter("@deleted", item.deleted);
                    await db.ExecuteNonQuery(@"insert into UNIT(id,name,title,title_vi,image_url,isfree,deleted)
                                                    values(@id,@name,@title,@title_vi,@image_url,@isfree,@deleted)", listParam);
                }
                db.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async void ImportReviewModel()
        {
            try
            {
                DbService db = new DbService();
                db.Open();
                for (int i = 1; i <= 10; i++)
                {
                    var lstReview = await HttpService.GetAllReviewByUnit(i);
                    foreach (var item in lstReview)
                    {
                        var listParam = new SqlCeParameter[6];
                        listParam[0] = new SqlCeParameter("@id", item.id);
                        listParam[1] = new SqlCeParameter("@name", item.name == null ? string.Empty : item.name);
                        listParam[2] = new SqlCeParameter("@image_url", item.image_url);
                        listParam[3] = new SqlCeParameter("@progress", item.progress ?? 0);
                        listParam[4] = new SqlCeParameter("@min_progress", item.min_progress);
                        listParam[5] = new SqlCeParameter("@idunit", i);
                        await db.ExecuteNonQuery(@"insert into REVIEW(id,name,image_url,progress,min_progress,idunit)
                                                    values(@id,@name,@image_url,@progress,@min_progress,@idunit)", listParam);
                    }
                }

                db.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<int> UpdateReviewModel()
        {
            try
            {
                int result = 0;
                DbService db = new DbService();
                db.Open();
                for (int i = 1; i <= 10; i++)
                {
                    var lstReview = await HttpService.GetAllReviewByUnit(i);
                    foreach (var item in lstReview)
                    {
                        var listParam = new SqlCeParameter[6];
                        listParam[0] = new SqlCeParameter("@id", item.id);
                        listParam[1] = new SqlCeParameter("@name", item.name == null ? string.Empty : item.name);
                        listParam[2] = new SqlCeParameter("@image_url", item.image_url);
                        listParam[3] = new SqlCeParameter("@progress", item.progress ?? 0);
                        listParam[4] = new SqlCeParameter("@min_progress", item.min_progress);
                        listParam[5] = new SqlCeParameter("@idunit", i);
                        await db.ExecuteNonQuery(@"update REVIEW set progress=@progress,min_progress=@min_progress,idunit=@idunit,name=@name,active=0 where id=@id", listParam);
                    }
                }

                db.Close();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<int> UpdateUsageModel()
        {
            try
            {
                int result = 0;
                DbService db = new DbService();
                db.Open();

                List<UsageInfoModel> lstUsage = new List<UsageInfoModel>();
                var usage = await db.ExecuteReturnData(@"select * from USAGEINFO where username='" + App.UserId + "'");
                while (usage.Read())
                {
                    UsageInfoModel info = new UsageInfoModel();
                    info.id = int.Parse(usage["id"].ToString());
                    info.idVocab = int.Parse(usage["idVocab"].ToString());
                    info.isfavorite = int.Parse(usage["isfavorite"].ToString());
                    info.ismemos = usage["ismemos"] != DBNull.Value ? int.Parse(usage["ismemos"].ToString()) : 0;
                    info.active = int.Parse(usage["active"].ToString());

                    lstUsage.Add(info);
                }
                var lstFav = await HttpService.GetAllFavorite();
                var lstMemos = await HttpService.GetAllMemos();

                lstFav.AddRange(lstMemos);
                var lstAll = lstFav.GroupBy(p => p.id).Select(g => g.First()).ToList();

                var lstToUpdate = lstAll.Where(x => lstUsage.Any(o => o.idVocab == x.id)).ToList();
                var lstToInsert = lstAll.Except(lstToUpdate).ToList();
                if (lstToUpdate.Count > 0)
                {
                    foreach (var item in lstToUpdate)
                    {
                        var listParam = new SqlCeParameter[5];
                        listParam[0] = new SqlCeParameter("@idVocab", item.id);
                        listParam[1] = new SqlCeParameter("@isfavorite", item.is_favorite);
                        listParam[2] = new SqlCeParameter("@username", App.UserId);
                        listParam[3] = new SqlCeParameter("@memos", string.IsNullOrEmpty(item.memo) ? "" : item.memo);
                        listParam[4] = new SqlCeParameter("@ismemos", string.IsNullOrEmpty(item.memo) ? 0 : 1);

                        await db.ExecuteNonQuery(@"update USAGEINFO set isfavorite=@isfavorite,active=0,ismemos=@ismemos,memos=@memos
                                                       where username= @username and idVocab=@idVocab", listParam);
                    }
                }
                if (lstToInsert.Count > 0)
                {
                    foreach (var item in lstToInsert)
                    {
                        var listParam = new SqlCeParameter[5];
                        listParam[0] = new SqlCeParameter("@idVocab", item.id);
                        listParam[1] = new SqlCeParameter("@isfavorite", item.is_favorite);
                        listParam[2] = new SqlCeParameter("@username", App.UserId);
                        listParam[3] = new SqlCeParameter("@memos", string.IsNullOrEmpty(item.memo) ? "" : item.memo);
                        listParam[4] = new SqlCeParameter("@ismemos", string.IsNullOrEmpty(item.memo) ? 0 : 1);
                        await db.ExecuteNonQuery(@"insert into USAGEINFO(idVocab,isfavorite,ismemos,memos,username,active)
                                                       values(@idVocab,@isfavorite,@ismemos,@memos,@username,0)", listParam);
                    }
                }

                result = await db.ExecuteNonQuery(@"update USAGEINFO set active=0 where username='" + App.UserId + "'");
                db.Close();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async void ImportVoCabModel()
        {
            try
            {
                var lstVoCab = await HttpService.GetAllVocab();
                //var lstVoCab = await new WordListTabQuery().GetAllWord();
                lstVoCab.ForEach(x =>
                {
                    if (string.IsNullOrEmpty(x.image_url))
                    {
                        x.image_url = "/logo.png";
                    }
                });
                DbService db = new DbService();
                db.Open();
                foreach (var item in lstVoCab)
                {
                    var listParam = new SqlCeParameter[12];
                    listParam[0] = new SqlCeParameter("@id", item.id);
                    listParam[1] = new SqlCeParameter("@word", item.word);
                    listParam[2] = new SqlCeParameter("@word_form", item.word_form);
                    listParam[3] = new SqlCeParameter("@word_vi", item.word_vi);
                    listParam[4] = new SqlCeParameter("@phonetic", item.phonetic);
                    listParam[5] = new SqlCeParameter("@meaning", item.meaning);
                    listParam[6] = new SqlCeParameter("@example", item.example);
                    listParam[7] = new SqlCeParameter("@image_url", item.image_url);
                    listParam[8] = new SqlCeParameter("@audio_url", item.audio_url);
                    listParam[9] = new SqlCeParameter("@grade_unit_id", item.grade_unit_id);
                    listParam[10] = new SqlCeParameter("@deleted", item.deleted);
                    listParam[11] = new SqlCeParameter("@word_parts", item.word_parts == null ? string.Empty : item.word_parts);
                    await db.ExecuteNonQuery(@"insert into VOCABULARY(id,word,word_form,word_vi,phonetic,meaning,example,image_url,audio_url,grade_unit_id,deleted,word_parts)
                                                    values(@id,@word,@word_form,@word_vi,@phonetic,@meaning,@example,@image_url,@audio_url,@grade_unit_id,@deleted,@word_parts)", listParam);
                }
                db.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}