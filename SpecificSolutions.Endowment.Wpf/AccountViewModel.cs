using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Core.Enums.Accounts;

namespace SpecificSolutions.Endowment.Wpf;
public partial class AccountViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private readonly IAuthenticator _authenticator;

    public AccountViewModel(IMediator mediator, IAuthenticator authenticator)
    {
        _mediator = mediator;
        _authenticator = authenticator;
        CreateAccountCommand = new AsyncRelayCommand(CreateAccount);
    }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string motherName;

    [ObservableProperty]
    private DateTime birthDate;

    [ObservableProperty]
    private Gender gender;

    [ObservableProperty]
    private string barcode;

    [ObservableProperty]
    private Status status;

    [ObservableProperty]
    private int lockerFileNumber;

    [ObservableProperty]
    private SocialStatus socialStatus;

    [ObservableProperty]
    private int bookNumber;

    [ObservableProperty]
    private int paperNumber;

    [ObservableProperty]
    private int registrationNumber;

    [ObservableProperty]
    private string accountNumber;

    [ObservableProperty]
    private AccountType type;

    [ObservableProperty]
    private bool lookOver;

    [ObservableProperty]
    private string note;

    [ObservableProperty]
    private int nid;

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private decimal balance;

    public IAsyncRelayCommand CreateAccountCommand { get; }
    public int NID { get; private set; }

    private async Task CreateAccount()
    {
        var command1 = new LoginCommand
        {
            Email = "admin@clean.com",
            Password = "admin@123$"
        };

        //var command = new CreateAccountCommand
        //{
        //    Name = Name,
        //    MotherName = MotherName,
        //    BirthDate = DateTime.Now,
        //    Gender = Gender.Female,
        //    Barcode = "Barcode",
        //    Status = Status.Active,
        //    LockerFileNumber = 23,
        //    SocialStatus = SocialStatus.Child,
        //    BookNumber = 121,
        //    PaperNumber = 12,
        //    RegistrationNumber = 12,
        //    AccountNumber = "AccountNumber",
        //    Type = AccountType.Marketer,
        //    LookOver = LookOver,
        //    Note = "Note",
        //    NID = 13232323,
        //    IsActive = IsActive,
        //    Balance = 121211m
        //};

        var result = await _mediator.Send(command1);

        //var result = await _mediator.Send(command);
        await Login(command1);
    }

    public async Task<IUserLogin> Login(LoginCommand command)
    {

        return await _authenticator.LoginAsync(command);
    }
}