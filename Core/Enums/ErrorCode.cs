using System.ComponentModel;

namespace Core.Enums;

public enum ErrorCode
{
    [Description("invalid login or pass")]
    InvalidLoginPass = 1000,
    [Description("account or employee is locked")]
    AccountOrEmployeeLocked = 1001,
    [Description("email must be unique")]
    EmailMustBeUnique = 1002,
    [Description("phone must be unique")]
    PhoneMustBeUnique = 1003,
    [Description("passport number must be unique")]
    PassportNumberMustBeUnique = 1004,
    [Description("Name must be unique")]
    NameMustBeUnique = 1005,
    [Description("Email sender mustbe unique")]
    EmailSenderMustBeUnique = 1006,
    [Description("User not exists")]
    UserNotExists = 1007,
    [Description("SystemId and Host must be unique")]
    SystemIdAndHostMustBeUnique = 1008,


    [Description("ShopTitle must be unique")]
    ShopTitleMustBeUnique = 1009,
    [Description("A field must be unique")]
    FieldMustBeUnique = 1010,
}
