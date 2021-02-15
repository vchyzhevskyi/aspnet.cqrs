using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class SimpleQueryTestCase1 : IQueryHandler<IQuery>
    {
        public Task Handle(IQuery query)
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleQueryTestCase2 : IQueryHandler<IQuery>
    {
        public Task Handle(IQuery query)
        {
            return Task.CompletedTask;
        }
    }
}
