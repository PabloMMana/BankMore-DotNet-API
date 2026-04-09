using MediatR;

namespace BankMore.Application.Commands
{
   
    public class DesativarContaCommand : IRequest<bool>
    {
        public int NumeroConta { get; set; }
    }
}