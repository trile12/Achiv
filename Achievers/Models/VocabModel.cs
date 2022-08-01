using Achievers.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace Achievers.Models
{
    public class VocabModel : BaseViewModel
    {
        public VocabModel()
        {
            Examples = new List<Example>();
        }

        public int ID { get; set; }
        public string Word { get; set; }
        public WordForm WordForm { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Translate { get; set; }
        public string Explain { get; set; }
        public string Image { get; set; }
        public List<Example> Examples { get; set; }
        public string IPA { get; set; }
        public string HoverBackgroundColor { get; set; }
        public string HoverBorderColor { get; set; }

        private Visibility isChecked;

        public Visibility Checked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }
    }

    public class WordForm
    {
        public WordForm(WordFormName name)
        {
            Name = name;
        }

        public WordFormName Name { get; set; }

        public string Code
        {
            get
            {
                switch (Name)
                {
                    case WordFormName.Adjective:
                        return "(adj)";

                    case WordFormName.Noun:
                        return "(n)";

                    case WordFormName.VerbPhrase:
                        return "(v phr)";

                    case WordFormName.Verb:
                        return "(v)";

                    case WordFormName.NounPhrase:
                        return "(n phr)";

                    case WordFormName.Preposition:
                        return "(prep)";

                    case WordFormName.Adverb:
                        return "(adv)";

                    case WordFormName.PhrasalVerb:
                        return "(phr v)";

                    default:
                        return "None";
                }
            }
        }
    }

    public enum WordFormName
    {
        None,
        Adjective,
        Noun,
        VerbPhrase,
        Verb,
        NounPhrase,
        Preposition,
        Adverb,
        PhrasalVerb
    }

    public class Example
    {
        public string Title { get; set; }
    }
}