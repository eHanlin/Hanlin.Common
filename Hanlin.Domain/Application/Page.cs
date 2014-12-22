using System;
using System.Collections.Generic;
using System.Linq;

namespace Hanlin.Domain.Application
{
    public class Page<T> : IPage<T>
    {
        private readonly Pageable _pageable;

        public Page(IEnumerable<T> content, Pageable pageable, long totalElements)
        {
            _pageable = pageable;
            Content = new List<T>(content);
            TotalElements = totalElements;
        }

        public List<T> Content { get; private set; }

        public int Number { get { return _pageable.PageNumber; } }

        public int Size { get { return _pageable.PageSize; } }

        public int NumberOfElements { get { return Content.Count; } }
        
        public long TotalElements { get; private set; }

        public int TotalPages
        {
            get
            {
                var total = (int)Math.Ceiling(TotalElements / (double) Size);
                return total;
            }
        }

        public bool HasContent { get { return Content.Any(); } }
        public bool HasNextPage { get { return Number < TotalPages; } }
        public bool HasPreviousPage { get { return Number > 1; } }
        public bool IsFirstPage { get { return !HasPreviousPage; } }
        public bool IsLastPage { get { return !HasNextPage; } }
    }
}