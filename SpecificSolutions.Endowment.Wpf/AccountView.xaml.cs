using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Windows;
namespace SpecificSolutions.Endowment.Wpf;

public partial class AccountView : Window
{
    //TODO concret Iuser
    public AccountView(IMediator mediator, IAuthenticator authenticator)
    {
        InitializeComponent();
        DataContext = new AccountViewModel(mediator, authenticator: authenticator);
    }
}