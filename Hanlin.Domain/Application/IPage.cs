using System.Collections.Generic;

namespace Hanlin.Domain.Application
{
    /// <summary>
    /// Adopted from Spring Data: 
    /// http://docs.spring.io/spring-data/data-commons/docs/1.1.x/api/org/springframework/data/domain/Page.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPage<T>
    {
        /// <summary>
        /// Returns the page content as List.
        /// </summary>
        List<T> Content { get; }

        /// <summary>
        /// Returns the number of the current page.
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Returns the size of the page.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Returns the number of elements currently on this page.
        /// </summary>
        int NumberOfElements { get; }

        /// <summary>
        /// Returns the total amount of elements.
        /// </summary>
        /// <returns></returns>
        long TotalElements { get; }
          
        /// <summary>
        /// Returns the number of total pages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Returns whether the Page has content at all.
        /// </summary>
        bool HasContent { get; }

        /// <summary>
        /// Returns if there is a next page.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Returns if there is a previous page.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Returns whether the current page is the first one.
        /// </summary>
        /// <returns></returns>
        bool IsFirstPage { get; }
          
        /// <summary>
        /// Returns whether the current page is the last one.
        /// </summary>
        bool IsLastPage { get; }
    }
}
