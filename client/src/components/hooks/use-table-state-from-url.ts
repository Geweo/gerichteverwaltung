import { useMemo } from 'react';
import type { PaginationState, SortingState } from '@tanstack/react-table';

interface TableSearchParams {
  /**
   * Page number from search params (1-based, as string from URL)
   */
  page?: Readonly<string>;

  /**
   * Page size from search params (as string from URL)
   */
  pageSize?: Readonly<string>;

  /**
   * Field to sort by from search params
   */
  sortBy?: Readonly<string>;

  /**
   * Sort direction from search params ('asc' | 'desc')
   */
  sortDirection?: Readonly<'asc' | 'desc'>;
}

interface UseTableStateFromUrlOptions {
  /**
   * Search params from URL (pagination and sorting)
   */
  search: TableSearchParams;

  /**
   * Default page size if not provided in search params
   */
  defaultPageSize?: number;

  /**
   * Callback to update pagination in URL (receives 1-based page and pageSize as strings)
   */
  onPaginationChange: (page: string, pageSize: string) => void;

  /**
   * Callback to update sorting in URL (receives sortBy and sortDirection, or undefined to clear)
   */
  onSortingChange: (sortBy: string | undefined, sortDirection: 'asc' | 'desc' | undefined) => void;
}

interface UseTableStateFromUrlReturn {
  /**
   * Pagination state in TanStack Table format (0-based pageIndex)
   */
  pagination: PaginationState;

  /**
   * Sorting state in TanStack Table format
   */
  sorting: SortingState;

  /**
   * Handler for pagination changes (converts TanStack format and calls onPaginationChange)
   */
  handlePaginationChange: (pagination: PaginationState) => void;

  /**
   * Handler for sorting changes (converts TanStack format and calls onSortingChange)
   */
  handleSortingChange: (sorting: SortingState | undefined) => void;
}

/**
 * Hook to convert table state (pagination, sorting) from URL format to TanStack Table format.
 * Handles conversion between URL (1-based page strings) and TanStack Table (0-based pageIndex).
 * You provide callbacks to update the URL when table state changes.
 */
export function useTableStateFromUrl({
  search,
  defaultPageSize = 10,
  onPaginationChange,
  onSortingChange,
}: UseTableStateFromUrlOptions): UseTableStateFromUrlReturn {
  // Convert search params to TanStack format
  const pagination = useMemo<PaginationState>(
    () => ({
      pageIndex: Number(search.page ?? '1') - 1, // Convert 1-based to 0-based
      pageSize: Number(search.pageSize ?? String(defaultPageSize)),
    }),
    [search.page, search.pageSize, defaultPageSize]
  );

  // Convert search params to TanStack Table format
  const sorting = useMemo<SortingState>(() => {
    if (search.sortBy && search.sortDirection) {
      return [
        {
          id: search.sortBy,
          desc: search.sortDirection === 'desc',
        },
      ];
    }
    return [];
  }, [search.sortBy, search.sortDirection]);

  // Handler for pagination changes (convert TanStack format to URL and call callback)
  const handlePaginationChange = (newPagination: PaginationState) => {
    const page = String(newPagination.pageIndex + 1); // Convert 0-based to 1-based
    const pageSize = String(newPagination.pageSize);
    onPaginationChange(page, pageSize);
  };

  // Handler for sorting changes (convert TanStack format to URL and call callback)
  const handleSortingChange = (newSorting: SortingState | undefined) => {
    if (newSorting && newSorting.length > 0) {
      const firstSort = newSorting[0];
      onSortingChange(firstSort.id, firstSort.desc ? 'desc' : 'asc');
    } else {
      onSortingChange(undefined, undefined);
    }
  };

  return {
    pagination,
    sorting,
    handlePaginationChange,
    handleSortingChange,
  };
}
