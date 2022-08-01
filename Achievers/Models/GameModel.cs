using System.Collections.Generic;

namespace Achievers.Models
{
    public class GameModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image_url { get; set; }
        public GameName activity_type { get; set; }
        public int grade_unit_id { get; set; }
        public int total_question { get; set; }
        public int deleted { get; set; }
        public string instruction { get; set; }
        public string sub_instruction { get; set; }
        public NextUnit next_unit_review { get; set; }
        public List<QuestionModel> questions { get; set; }
    }

    public class NextUnit
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class QuestionModel
    {
        public string grid_str { get; set; }
        public List<AnswerModel> answers { get; set; }
    }

    public class AnswerModel
    {
        public string text { get; set; }
        public string correctText { get; set; }
        public int numOfPart { get; set; }

        public string definition { get; set; }
        public string image { get; set; }
        public string audio { get; set; }
        public string hint { get; set; }

        public List<string> parts { get; set; }
    }
}