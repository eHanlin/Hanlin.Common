namespace Hanlin.Domain.Application
{
    public class PageableCommand : Pageable
    {
        private const int DefaultPageSize = 20;

        public PageableCommand() : base(DefaultPageSize)
        {
            
        }

        public PageableCommand(int pageSize = DefaultPageSize) : base(pageSize)
        {
            
        }
    }
}
