import { useMemo } from 'react';
import type { PaginationState, SortingState } from '@tanstack/react-table';

/**
 * Sort direction enum for API calls
 */
export type SortDirectionEnum = 'Asc' | 'Desc';

export interface PaginationQueryParams {
  page: number; // 1-based for API
  pageSize: number;
}

export interface SortingQueryParams<TSortBy> {
  sortBy?: TSortBy;
  sortDirection?: SortDirectionEnum;
}

/**
 * Combines pagination and sorting query params for server-side table queries.
 * Handles the conversion from TanStack Table pagination and sorting state to API query params.
 *
 * @param pagination - TanStack Table pagination state (0-based pageIndex)
 * @param sorting - TanStack Table sorting state
 * @param columnToSortMapping - Mapping of column IDs to API sortBy enum/type
 * @returns Pagination and sorting query params ready to spread into API calls
 *
 * @example
 * ```tsx
 * const queryParams = useServerTableQueryParams({
 *   pagination,
 *   sorting,
 *   columnToSortMapping: { name: 'Name', date: 'Date' }
 * })
 *
 * const query = useGetItems({ ...otherParams, ...queryParams })
 * ```
 */
export function useServerTableQueryParams<TSortBy>({
  pagination,
  sorting,
  columnToSortMapping,
}: {
  pagination: PaginationState;
  sorting?: SortingState;
  columnToSortMapping: Record<string, TSortBy | undefined>;
}): PaginationQueryParams & SortingQueryParams<TSortBy> {
  return useMemo(() => {
    // Pagination params (0-based pageIndex to 1-based page)
    const paginationParams: PaginationQueryParams = {
      page: pagination.pageIndex + 1,
      pageSize: pagination.pageSize,
    };

    // Sorting params
    let sortingParams: SortingQueryParams<TSortBy> = {};
    if (sorting && sorting.length > 0) {
      // API only supports one sort column at a time - so we ignore the rest of the sorting state
      const firstSort = sorting[0];
      const queryParamSortBy = columnToSortMapping[firstSort.id];

      if (queryParamSortBy) {
        sortingParams = {
          sortBy: queryParamSortBy,
          sortDirection: firstSort.desc ? 'Desc' : 'Asc',
        };
      }
    }

    return {
      ...paginationParams,
      ...sortingParams,
    };
  }, [pagination.pageIndex, pagination.pageSize, sorting, columnToSortMapping]);
}
