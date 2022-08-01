using Achievers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Achievers.Services
{
    public static class HttpService
    {
        //static int c = 1;
        public const string baseUrl = "https://achieverapi.eduhome.com.vn";

        private const string baseAuth = "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlRpbmggRGVwIFRyYWkgbmhhdCB0aGUgZ2lvaSIsImlhdCI8KUDxNjIzOTAyXy6";

        public static async Task<T> CallApi<T>(string url, string method, object body)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.BaseAddress = baseUrl;
                    http.Encoding = Encoding.UTF8;
                    http.Headers["Authorization"] = baseAuth;
                    http.Headers["Content-Type"] = "application/json";
                    string content = JsonConvert.SerializeObject(body);
                    if (method == "GET")
                    {
                        var result = await http.DownloadStringTaskAsync(url);
                        //System.IO.File.WriteAllText(Environment.CurrentDirectory + @"\Assets\GameModel\g" + c + ".json", result);
                        //c++;
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                    else
                    {
                        var result = await http.UploadStringTaskAsync(url, method, content);
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                LogService.Error("HttpService", ex);
#endif
                //var result = HttpErrorMessage(ex);
                //if (await PopupErrorManager.Show(result.Title, result.Message)
                //        == 0)
                //{
                //    PopupErrorManager.Hide();
                //    return await CallApi<T>(url, method, body, auth);
                //}
            }
            return default(T);
        }

        public static async Task<List<UnitModel>> GetAllUnit()
        {
            return await CallApi<List<UnitModel>>("/units", "GET", null);
        }

        public static async Task<List<UnitModel>> GetAllUnitByUser()
        {
            return await CallApi<List<UnitModel>>("/units/user/" + App.UserId, "GET", null);
        }

        public static async Task<List<ReviewModel>> GetAllReviewByUnit(int unit)
        {
            return await CallApi<List<ReviewModel>>("/reviews/unit/" + unit + "/user/" + App.UserId, "GET", null);
        }

        public static async Task<List<WordModel>> GetAllVocabByUnit(int unit)
        {
            return await CallApi<List<WordModel>>("/vocabs/unit/" + unit + "/user/" + App.UserId, "GET", null);
        }

        public static async Task<List<WordModel>> GetAllVocab()
        {
            return await CallApi<List<WordModel>>("/vocabs/user/" + App.UserId, "GET", null);
        }

        public static async Task<List<WordModel>> GetVocabDetailById(int id)
        {
            return await CallApi<List<WordModel>>("/vocabs/" + id + "/user/" + App.UserId, "GET", null);
        }

        public static async Task<List<WordModel>> GetAllMemos()
        {
            return await CallApi<List<WordModel>>("/memos/user/" + App.UserId, "GET", null);
        }

        public static async Task<object> CreateMemos(object body)
        {
            return await CallApi<object>("/memos", "POST", body);
        }

        public static async Task<object> DeleteMemos(object body)
        {
            return await CallApi<object>("/memos", "DELETE", body);
        }

        public static async Task<List<WordModel>> GetAllFavorite()
        {
            return await CallApi<List<WordModel>>("/favorites/user/" + App.UserId, "GET", null);
        }

        public static async Task<object> CreateFavorite(object body)
        {
            return await CallApi<object>("/favorites", "POST", body);
        }

        public static async Task<object> DeleteFavorite(object body)
        {
            return await CallApi<object>("/favorites", "DELETE", body);
        }

        public static async Task<GameModel> GetGameModelById(int id)
        {
            return await CallApi<GameModel>("/reviews/" + id + "/questions", "GET", null);
        }

        public static async Task<object> SendProgress(object body)
        {
            return await CallApi<object>("/reviews/progress", "POST", body);
        }

        private static HttpErrorMessage HttpErrorMessage(Exception ex)
        {
            string title = "OOPS!! SOMETHING WENT WRONG";
            string description = "An error has ocurred";
            if (ex is WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.NameResolutionFailure || webEx.Status == WebExceptionStatus.ReceiveFailure)
                {
                    title = "OOPS!! NETWORK CONNECTION LOST";
                    description = "Please check your network and try again.";
                }
                else
                {
                    title = webEx.Status.ToString();
                    description = webEx.Message;

#if DEBUG
                    description += " -- " + webEx.Response?.ResponseUri + " -- " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
#endif
                }
            }
            return new HttpErrorMessage() { Title = title, Message = description };
        }
    }

    public class HttpErrorMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}