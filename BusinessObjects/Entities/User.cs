using System;
using static Enum.Enums;

namespace Entities
{
    public class User
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }
    public class Registration: AddressDetails
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string EmaillAddress { get; set; }
        public RoleType RoleId { get; set; }
        public string IpAddress { get; set; }
    }
    public class AddressDetails:CompanyDetails
    {
        public string Building_Name { get; set; }
        public string Locality { get; set; }
        public string PinCode { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public string Landmark { get; set; }
        public AddressType AddressType { get; set; }
    }
    public class CompanyDetails
    {
        public string CompanyName { get; set; }
        public string GST_No { get; set; }
        public string PanNumber { get; set; }
        public string BusinessId { get; set; }
        public string CompanyPhoto { get; set; }
        public string  Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class ChangePassword
    {
        public string  Username { get; set; }
        public string  OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string  ConfirmPassword { get; set; }
    }
    public class LoginDetails
    {
        public int LoginId { get; set; }
        public string MobileNumber { get; set; }
        public string  Password { get; set; }
        public int MemberId { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime ? LastLoginDate { get; set; }
        public bool IsLocked { get; set; }
        public bool IsLogedIn { get; set; }
    }
}
