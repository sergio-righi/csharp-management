
using Tool.Resources;

namespace Tool.Utilities
{
    public enum EDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public enum EFeedback
    {
        Info = 1,
        Success = 2,
        Warning = 3,
        Error = 4
    };

    public enum EExtension
    {
        [HiddenValue]
        Undefined = -1,
        All = 1,
        Audio = 2,
        Image = 3,
        Document = 4,
        Spreadsheet = 5,
        Pdf = 6,
        Compressed = 7,
        Presentation = 8,
        Video = 9
    };

    public enum ECulture
    {
        Portuguese = 1,
        English = 2,
        Spanish = 3,
        Korean = 4
    };

    public enum EPerson
    {
        Natural = 1,
        Juridical = 2
    }

    public enum EAddress
    {
        Residential = 1,
        Commercial = 2,
        Other = 3
    }

    public enum EEmail
    {
        Personal = 1,
        Commercial = 2,
        Other = 3
    }

    public enum EPhone
    {
        Residential = 1,
        Commercial = 2,
        Cellphone = 3,
        Other = 4
    }

    public enum EGender
    {
        Male = 1,
        Female = 2,
        Uninformed = 3,
        Other = 4
    };

    public enum EBusinessArea
    {
        HumanResources = 1,
        Accounting = 2,
        Finance = 3,
        Marketing = 4,
        Production = 5,
        Technology = 6,
        Operation = 7,
        CustomerService = 8,
        Purshasing = 9,
        Legal = 10,
        Uninformed = 11,
        Other = 12
    }

    public enum EBusinessActivity
    {
        Uninformed = 1,
        Other = 2
    }

    public enum EBusinessType
    {
        SoleProprietorship = 1,
        Partnership = 2,
        LimitedPartnership = 3,
        Corporation = 4,
        LimitedLiabilityCompany = 5,
        NonprofitOrganization = 6,
        Cooperative = 7,
        Uninformed = 8,
        Other = 9
    }

    public enum ERent
    {
        [HiddenValue]
        All = 0,
        New = 1,
        Budgeted = 2,
        Confirmed = 3,
        Finalized = 4,
        Canceled = 5
    }

    public enum ESale
    {
        [HiddenValue]
        All = 0,
        New = 1,
        Budgeted = 2,
        Confirmed = 3,
        InProduction = 4,
        Ordered = 5,
        Delivered = 6,
        Shipped = 7,
        Finalized = 8,
        Canceled = 9
    }

    public enum ERole
    {
        [HiddenValue]
        All = 0,
        [HiddenValue]
        IT = 1,
        [HiddenValue]
        President = 2,
        Administrative = 3,
        Marketing = 4,
        Operations = 5,
        Production = 6,
        Finance = 7,
        Sales = 8,
        HumanResource = 9,
        Purchasing = 10,
        Research = 11,
        CustomerService = 12,
        Distribuition = 13,
        Legal = 14,
        [HiddenValue]
        Vendor = 15,
        [HiddenValue]
        Customer = 16
    }

    public enum EMonth
    {
        [HiddenValue]
        All = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum EAttendance
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        Excused = 4
    }

    public enum EShift
    {
        Morning = 1,
        Afternoon = 2,
        Evening = 3,
        Night = 4
    }

    public enum EScheduleRule
    {
        FullTime = 1,
        PartTime = 2,
        Unpredictable = 3,
        Flexible = 4,
        Compressed = 5,
        Shift = 6,
        OnDemand = 7,
        Overtime = 8,
        NoSchedule = 9
    }

    public enum ECalculation
    {
        Unit = 1,
        SquareMeter = 2,
        Meter = 3
    }

    public enum EDocument
    {
        Id = 1,
        Passport = 2,
        DriversLicense = 3,
        EmployeeId = 4
    }

    public enum EBill
    {
        Purchase = 1,
        Tax = 2,
        Loan = 3,
        Other = 4
    }

    public enum EPaymentMethod
    {
        Cash = 1,
        Deposit = 2,
        DebitCard = 3,
        CreditCard = 4,
        BankTransfer = 5,
        BankCheck = 6
    }

    public enum EController
    {
        Address = 1,
        Bill = 2,
        Drive = 3,
        Email = 4,
        Group = 5,
        Home = 6,
        Item = 7,
        JuridicalPerson = 8,
        Login = 9,
        NaturalPerson = 10,
        Phone = 11,
        Product = 12,
        Rent = 13,
        Role = 14,
        Sale = 15,
        Subgroup = 16
    }

    public enum ELog
    {
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warning = 4,
        Error = 5
    }

    public enum ECurrency
    {
        BRL = 1,
        USD = 2,
        EUR = 3,
        JPY = 4,
        GBP = 5,
        AUD = 6,
        CAD = 7,
        MXN = 8,
        TRY = 9,
        NZD = 10,
        WON = 11,
        RUB = 12,
    }

    public enum EToken
    {
        Rookie = 1,
        Password = 2
    }

    public enum ESize
    {
        None = 0
    }

    public enum EColor
    {
        None = 0
    }

    public enum EMaterial
    {
        None = 0
    }

    public enum EShape
    {
        None = 0
    }

    public enum EQuizQuestion
    {
        OpenQuestion = 1,
        MultipleChoice = 2,
        RatingScale = 3,
        LikertScale = 4
    }

    public enum EStringGender
    {
        Male = 1,
        Female = 2
    }

    public enum EStringFormat
    {
        One = 1,
        Many = 2,
        OneOrMany = 3
    }

    public enum EExam
    {
        All = 0,
        Periodic = 1,
        Admission = 2,
        Dismissal = 3
    }
}