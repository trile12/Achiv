using Achievers.Models;
using Achievers.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Achievers.Queries
{
    public class UnitTabQuery
    {
        public async Task<List<UnitModel>> GetUnit()
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<UnitModel>();
            var result = await db.ExecuteReturnData(
                      @"select * from UNIT");

            while (result.Read())
            {
                UnitModel unit = new UnitModel();
                unit.id = int.Parse(result["id"].ToString());
                unit.name = result["name"].ToString() == string.Empty ? null : result["name"].ToString();
                unit.title = result["title"].ToString();
                unit.title_vi = result["title_vi"].ToString();
                unit.image_url = result["image_url"].ToString();
                unit.deleted = int.Parse(result["deleted"].ToString());
                unit.is_free = int.Parse(result["isfree"].ToString());

                unit.reviews = new List<ReviewModel>();

                var reviews = await db.ExecuteReturnData(
                @"select * from REVIEW where idunit=" + unit.id);

                while (reviews.Read())
                {
                    unit.reviews.Add(new ReviewModel
                    {
                        id = int.Parse(reviews["id"].ToString()),
                        name = reviews["name"].ToString(),
                        image_url = reviews["image_url"].ToString(),
                        progress = reviews["progress"].ToString() == "0" ? null : (int?)int.Parse(reviews["progress"].ToString()),
                        min_progress = int.Parse(reviews["min_progress"].ToString()),
                    });
                }
                list.Add(unit);
                reviews.Close();
            }

            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        public async Task<List<ReviewModel>> GetAllReviewByUnit(int unit)
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<ReviewModel>();
            var result = await db.ExecuteReturnData(
                      @"select * from REVIEW where idunit=" + unit);

            while (result.Read())
            {
                ReviewModel review = new ReviewModel();
                review.id = int.Parse(result["id"].ToString());
                review.name = result["name"].ToString() == string.Empty ? null : result["name"].ToString();
                review.image_url = result["image_url"].ToString();
                review.progress = result["progress"] != DBNull.Value ? int.Parse(result["progress"].ToString()) : 0;
                review.min_progress = int.Parse(result["min_progress"].ToString());

                list.Add(review);
            }
            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        public async Task<int> UpdateReview(int reviewId, int progress)
        {
            try
            {
                int result = 0;
                DbService db = new DbService();
                db.Open();

                result = await db.ExecuteNonQuery(@"update REVIEW set active=1, progress=" + progress + " where id=" + reviewId);

                db.Close();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region synce

        public async Task<List<ReviewModel>> GetAllReviewUpdate()
        {
            DbService db = new DbService();
            db.Open();
            var list = new List<ReviewModel>();
            var result = await db.ExecuteReturnData(
                      @"select * from REVIEW where active=1");

            while (result.Read())
            {
                ReviewModel review = new ReviewModel();
                review.id = int.Parse(result["id"].ToString());
                review.name = result["name"].ToString() == string.Empty ? null : result["name"].ToString();
                review.image_url = result["image_url"].ToString();
                review.progress = result["progress"] != DBNull.Value ? int.Parse(result["progress"].ToString()) : 0;
                review.min_progress = int.Parse(result["min_progress"].ToString());

                list.Add(review);
            }
            result.Close();
            db.Close();
            return await Task.FromResult(list);
        }

        #endregion synce
    }
}