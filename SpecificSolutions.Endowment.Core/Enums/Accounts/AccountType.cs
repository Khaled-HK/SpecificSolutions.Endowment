
using System.ComponentModel;
namespace SpecificSolutions.Endowment.Core.Enums.Accounts;

public enum AccountType
{
    [Description("All")]
    All = 0,

    [Description("Martyr")]
    Martyr = 1,

    [Description("Disability")]
    Disability = 2,

    [Description("Beneficiary")]
    Beneficiary = 3,

    [Description("Treasury")]
    Treasury = 4,

    [Description("Payment Method")]
    PaymentMethod = 5,

    [Description("Market")]
    Market = 6,

    [Description("Marketer")]
    Marketer = 7,

    [Description("Customer")]
    Customer = 8,

    [Description("Supplier Company")]
    SupplierCompany = 9,

    [Description("Accounting")]
    Accounting = 10,

    [Description("Revenues")]
    Revenues = 11,

    [Description("Products")]
    Productos = 12,

    [Description("Liabilities")]
    Liabilities = 13,

    [Description("Expenses")]
    Expenses = 14,
}
