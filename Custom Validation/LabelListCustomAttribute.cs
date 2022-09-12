using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ThinkfulApp.Custom_Validation
{
    public class LabelListCustomAttribute : ValidationAttribute
    {
        public int MaximumLength { get; private set; }
        public int MinimumLength { get; private set; }
        public string DefaultValue { get; private set; }
        public LabelListCustomAttribute(string errorMessage = "There's something off with your Goal descriptions. Likely, it is too long.", int maximumLength = 75, int minimumLength = 1, string defaultValue = "Unspecified") : base(errorMessage)
        {
            MaximumLength = maximumLength;
            MinimumLength = minimumLength;
            DefaultValue = defaultValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var obj = validationContext.ObjectInstance;
            if (value is List<string?> list)
            {
                PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.PropertyType.Equals(typeof(List<string?>)))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] == "" || list[i] == null)
                            {
                                list[i] = DefaultValue;
                            }
                            if (list[i].Length > MaximumLength || list[i].Length < MinimumLength)
                            {
                                return new ValidationResult(ErrorMessage);
                            }
                        }
                        HashSet<string> uniques = new HashSet<string>(list);
                        if (uniques.Count != list.Count)
                        {
                            return new ValidationResult(ErrorMessage = "All Goal Names must be unique");
                        }
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
