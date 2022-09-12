using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ThinkfulApp.Custom_Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GoalListCustomAttribute : ValidationAttribute
    {

        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        public int DefaultValue { get; private set; }
        public GoalListCustomAttribute(string errorMessage = "There's something off with your Progress, keep it on a scale from 0 to 100", int minimum = 0, int maximum = 100, int defaultValue = 10) : base(errorMessage)
        {
            Minimum = minimum;
            Maximum = maximum;
            DefaultValue = defaultValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var obj = validationContext.ObjectInstance;
            if (value is List<int?> list)
            {
                PropertyInfo[] myPropertyInfo = obj.GetType().GetProperties();
                foreach (PropertyInfo info in myPropertyInfo)
                {
                    if (info.PropertyType.Equals(typeof(List<int?>)))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] == null)
                            {
                                list[i] = DefaultValue;
                            }
                            if (list[i] < Minimum || list[i] > Maximum)
                            {
                                return new ValidationResult(ErrorMessage);
                            }
                        }
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
