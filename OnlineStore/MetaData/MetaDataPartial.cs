using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }

    [MetadataType(typeof(CountryMetaData))]
    public partial class Country
    {
    }

    [MetadataType(typeof(StateMetaData))]
    public partial class State
    {
    }

    [MetadataType(typeof(LgaMetaData))]
    public partial class Lga
    {
    }

    [MetadataType(typeof(SubCategoryMetaData))]
    public partial class SubCategory
    {
    }

    [MetadataType(typeof(SecurityQuestionMetaData))]
    public partial class SecurityQuestion
    {
    }

    [MetadataType(typeof(RoleMetaData))]
    public partial class Role
    {
    }
}