using System;

namespace Hanlin.Domain.Application
{
    /// <summary>
    /// Adopted from Spring Data: 
    /// http://docs.spring.io/spring-data/data-commons/docs/1.1.x/api/org/springframework/data/domain/Pageable.html
    /// </summary>
    public abstract class Pageable
    {
        protected Pageable(int pageSize, int pageNumber = 1)
        {
            if (pageSize < 1) throw new ArgumentException("pageSize cannot be less than 1.");
            if (pageNumber < 1) throw new ArgumentException("pageNumber cannot be less than 1.");

            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Offset
        {
            get { return (PageNumber - 1 ) * PageSize; }
        }
    }
}
