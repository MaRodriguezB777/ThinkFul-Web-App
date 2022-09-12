using System.Configuration;
using ThinkfulApp.Custom_Validation;

namespace ThinkfulApp.Models
{
    public class ChartModel
    {

        [IntegerValidator(MinValue = 1)]
        public int Id { get; set; }

        [GoalListCustom]
        public List<int?> GoalList { get; set; } = new List<int?>();

        [LabelListCustom]
        public List<string?> LabelList { get; set; } = new List<string?>();

        public int NumOfGoals { get; set; } = 6;

        //[Required]
        //[StringLength(maximumLength: 75)]
        //public string? LabelValidation;

        //[DefaultSettingValue("0")]
        //[Range(1, 100)]
        //public int GoalValidation;

    }
}
