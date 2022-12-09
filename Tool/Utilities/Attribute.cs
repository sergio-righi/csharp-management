
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using Tool.Resources;
using Tool.Utilities;

namespace Tool.Utilities
{
    public enum Compare
    {
        MoreThan,
        MoreOrEquals,
        Equals,
        LessThan,
        LessOrEquals
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxLengthResource : MaxLengthAttribute
    {
        public MaxLengthResource(int maximumSize) : base(maximumSize)
        {
            base.ErrorMessage = Message.MaximumSize;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MinLengthResource : MinLengthAttribute
    {
        public MinLengthResource(int minumumSize) : base(minumumSize)
        {
            base.ErrorMessage = Message.MinimumSize;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredResource : RequiredAttribute
    {
        public RequiredResource() : base()
        {
            base.ErrorMessage = Message.RequiredField;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CompareResource : ValidationAttribute
    {
        private string property { get; set; }

        public CompareResource(string property)
        {
            this.property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string currentField = value.ToString();

                string otherField = validationContext.ObjectType.GetProperty(property).GetValue(validationContext.ObjectInstance, null).ToString();

                if (!otherField.Equals(currentField))
                {
                    return new ValidationResult(Message.EqualField);
                }

                return ValidationResult.Success;
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PasswordResource : ValidationAttribute
    {
        private string property { get; set; }

        public PasswordResource(string property)
        {
            this.property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string confirmation = value.ToString();

                string password = validationContext.ObjectType.GetProperty(property).GetValue(validationContext.ObjectInstance, null).ToString();

                if (!password.Equals(confirmation))
                {
                    return new ValidationResult(Message.UnevenPassword);
                }

                return ValidationResult.Success;
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateCompareResource : ValidationAttribute
    {
        private string property { get; set; }
        private Compare compare { get; set; }

        public DateCompareResource(string property, Compare compare)
        {
            this.compare = compare;
            this.property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime originalField = (DateTime)value;

                DateTime otherField = (DateTime)(validationContext.ObjectType.GetProperty(property)?.GetValue(validationContext.ObjectInstance, null) ?? DateTime.MinValue);

                switch (compare)
                {
                    case Compare.Equals:
                        if (originalField.Date.Equals(otherField.Date))
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.LessThan:
                        if (originalField.Date < otherField.Date)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.LessOrEquals:
                        if (originalField.Date <= otherField.Date)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.MoreThan:
                        if (originalField.Date > otherField.Date)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.MoreOrEquals:
                        if (originalField.Date >= otherField.Date)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                }

                DisplayNameResource displayNameResource = validationContext.ObjectType.GetProperty(property).GetCustomAttribute(typeof(DisplayNameResource)) as DisplayNameResource;

                if (displayNameResource != null)
                {
                    return new ValidationResult(GetMessageFromResource(this.ErrorMessageResourceName ?? string.Empty, validationContext.DisplayName, displayNameResource.DisplayName));
                }
                else
                {
                    return new ValidationResult(GetMessageFromResource(this.ErrorMessageResourceName ?? string.Empty, validationContext.DisplayName, property));
                }
            }

            return null;
        }

        private static string GetMessageFromResource(string resource, string originalField, string otherField)
        {
            string message = Message.ResourceManager.GetString(resource);

            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            return string.Format(message, originalField, otherField);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CurrentDateResource : ValidationAttribute
    {
        private Compare compare { get; set; }

        public CurrentDateResource(Compare compare)
        {
            this.compare = compare;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime originalField = (DateTime)value;

                DateTime otherField = DateTime.Now.Date;

                switch (compare)
                {
                    case Compare.Equals:
                        if (originalField.Date.Equals(otherField))
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.LessThan:
                        if (originalField.Date < otherField)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.LessOrEquals:
                        if (originalField.Date <= otherField)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.MoreThan:
                        if (originalField.Date > otherField)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                    case Compare.MoreOrEquals:
                        if (originalField.Date >= otherField)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                }

                return new ValidationResult(string.Format(this.ErrorMessageResourceName ?? string.Empty, validationContext.DisplayName, otherField.ToShortDateString()));
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateValidationResource : ValidationAttribute
    {
        public DateValidationResource()
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (DateTime.TryParse(value.ToString(), out _))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(Message.InvalidDateFormat);
                }
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DisplayFormatResource : ValidationAttribute
    {
        public DisplayFormatResource()
        {
            //base.ApplyFormatInEditMode = true;
            //base.ConvertEmptyStringToNull = true;
            //base.DataFormatString = "{0:MM/dd/yyyy}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (Util.ValidateDateFormat(value.ToString()))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(Message.InvalidDateFormat);
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CPFResource : ValidationAttribute
    {
        public CPFResource()
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (Util.ValidateCPFOrCNPJ(value.ToString()))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(Message.InvalidDateFormat);
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DisplayNameResource : DisplayNameAttribute
    {
        public DisplayNameResource(string resource) : base(GetMessageFromResource(resource))
        { }

        private static string GetMessageFromResource(string resource)
        {
            return Label.ResourceManager.GetString(resource) ?? string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataTypeResource : DataTypeAttribute
    {
        public DataTypeResource(DataType dataType) : base(dataType)
        {
            string errorMessage = string.Empty;

            switch (DataType)
            {
                case DataType.Date:
                    errorMessage = Message.InvalidDateFormat;
                    break;
                case DataType.PostalCode:
                    break;
                case DataType.EmailAddress:
                    errorMessage = Message.InvalidEmail;
                    break;
                case DataType.ImageUrl:
                    errorMessage = Message.InvalidImageURL;
                    break;
                case DataType.PhoneNumber:
                    errorMessage = Message.InvalidPhone;
                    break;
                case DataType.Url:
                    errorMessage = Message.InvalidURL;
                    break;
            }

            base.ErrorMessage = errorMessage;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class EnumDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;

        public EnumDescriptionAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
            _resource = new ResourceManager(typeof(Label));
        }

        public EnumDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrWhiteSpace(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredOneResource : ValidationAttribute
    {
        private string property { get; set; }

        public RequiredOneResource(string property)
        {
            this.property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string propertyValue = validationContext.ObjectType.GetProperty(property).GetValue(validationContext.ObjectInstance, null)?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(value?.ToString() ?? string.Empty) && string.IsNullOrWhiteSpace(propertyValue))
            {
                return new ValidationResult(string.Format(Message.MinimumOneRequired, "", ""));
            }

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BirthdayResource : ValidationAttribute
    {
        private int minimumAge { get; set; }

        public BirthdayResource(int minimumAge)
        {
            this.minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date;

                if (DateTime.TryParse(value.ToString(), out date))
                {
                    if (!Util.VerifyMinimumAge(date, minimumAge))
                    {
                        return new ValidationResult(string.Format(Message.MinimumAgeRequired, minimumAge));
                    }

                    return ValidationResult.Success;
                }

                return new ValidationResult(Message.InvalidDateFormat);
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class HiddenValueAttribute : Attribute
    {
        public bool hidden { get; protected set; }

        public HiddenValueAttribute()
        {
            this.hidden = true;
        }
    }

    //[AttributeUsage(AttributeTargets.Property)]
    //public sealed class DateFormatResource : ValidationAttribute
    //{
    //    public DateFormatResource()
    //    {
    //        //base.ApplyFormatInEditMode = true;
    //        //base.ConvertEmptyStringToNull = true;
    //        //base.DataFormatString = "{0:MM/dd/yyyy}";
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        return true;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        //if (value != null)
    //        //{
    //        //    if (Utilitario.ValidarFormatoData(value.ToString()))
    //        //    {
    //        //        return ValidationResult.Success;
    //        //    }

    //        //    return new ValidationResult(Mensagem.FormatoDataInvalido);
    //        //}

    //        return ValidationResult.Success;
    //    }
    //}

    //[AttributeUsage(AttributeTargets.Property)]
    //public sealed class CPFResource : ValidationAttribute
    //{
    //    public CPFResource()
    //    { }

    //    public override bool IsValid(object value)
    //    {
    //        return true;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value != null)
    //        {
    //            if (Utilitario.ValidarCPFOuCNPJ(value.ToString()))
    //            {
    //                return ValidationResult.Success;
    //            }

    //            return new ValidationResult(Mensagem.FormatoDataInvalido);
    //        }

    //        return null;
    //    }
    //}

    //public sealed class DisplayEnumResourseAttribute : DescriptionAttribute
    //{
    //    private Type _resourceType;
    //    private PropertyInfo _nameProperty;

    //    public DisplayEnumResourseAttribute(string displayNameKey) : base(displayNameKey)
    //    {
    //    }

    //    public Type ResourceType
    //    {
    //        get
    //        {
    //            return _resourceType;
    //        }
    //        set
    //        {
    //            _resourceType = value;

    //            _nameProperty = _resourceType.GetProperty(this.Description, BindingFlags.Static | BindingFlags.Public);
    //        }
    //    }

    //    public override string Description
    //    {
    //        get
    //        {
    //            if (_nameProperty == null)
    //            {
    //                return base.Description;
    //            }

    //            return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
    //        }
    //    }
    //}
}
