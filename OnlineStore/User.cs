//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.UsersInRoles = new HashSet<UsersInRole>();
        }
    
        public long UserID { get; set; }
        public int CountryID { get; set; }
        public int GenderID { get; set; }
        public int SecurityQuestionID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserAddress1 { get; set; }
        public string UserAddress2 { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string UserConfirmPassword { get; set; }
        public string UserPhone { get; set; }
        public string SecurityAnswer { get; set; }
        public string UserBillingFirstName { get; set; }
        public string UserBillingLastName { get; set; }
        public string UserBillingCity { get; set; }
        public string UserBillingState { get; set; }
        public string UserBillingCountry { get; set; }
        public string UserBillingAddress { get; set; }
        public string UserBillingPhone { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UserRole { get; set; }
        public Nullable<System.Guid> ActivationID { get; set; }
        public Nullable<bool> IsActivated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual SecurityQuestion SecurityQuestion { get; set; }
        public virtual ICollection<UsersInRole> UsersInRoles { get; set; }
    }
}
