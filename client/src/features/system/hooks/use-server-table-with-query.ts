/** biome-ignore-all lint/suspicious/noExplicitAny: TanStack Table and Query are using any within */
import {
  keepPreviousData,
  type UndefinedInitialDataOptions,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import type { ColumnDef, PaginationState, SortingState, VisibilityState } from '@tanstack/react-table';
import type { AxiosRequestConfig } from 'axios';

import { useServerTableInstance } from '@/components/hooks/use-server-table-instance';
import {
  type PaginationQueryParams,
  type SortingQueryParams,
  useServerTableQueryParams,
} from '@/features/system/hooks/use-server-table-query-params';

type QueryOptions<TQueryFnData, TData> = {
  query?: Partial<UseQueryOptions<TQueryFnData, any, TData>> &
    Pick<UndefinedInitialDataOptions<TQueryFnData, any, TQueryFnData>, 'initialData'>;
  axios?: AxiosRequestConfig;
};

/**
 * Convenience hook for server-side tables with simple list queries.
 * Combines query params conversion, query execution, and table instance setup.
 * Use this for queries that only need pagination/sorting params (no ID or complex params).
 *
 * For queries that require an ID parameter, use `useServerTableWithQueryById` instead.
 * For more complex query logic, compose the hooks manually.
 */
export function useServerTableWithQuery<
  TData,
  TQueryParams,
  TSortBy,
  TResponse extends { items: TData[]; totalPages: number },
  TQueryFnData = any,
>({
  queryFn,
  queryParams,
  queryOptions,
  pagination,
  onPaginationChange,
  sorting,
  onSortingChange,
  columnToSortMapping,
  columns,
  defaultColumnVisibility = {},
}: {
  queryFn: (
    params: TQueryParams & SortingQueryParams<TSortBy> & PaginationQueryParams,
    options?: QueryOptions<TQueryFnData, TData>
  ) => UseQueryResult<{ data?: TResponse }>;
  queryParams?: TQueryParams;
  queryOptions?: QueryOptions<TQueryFnData, TData>;
  pagination: PaginationState;
  onPaginationChange: (p: PaginationState) => void;
  sorting?: SortingState;
  onSortingChange?: (s: SortingState | undefined) => void;
  columnToSortMapping: Record<string, TSortBy | undefined>;
  columns: ColumnDef<TData, any>[];
  defaultColumnVisibility?: VisibilityState;
}) {
  // Convert to query params for the API call
  const tableQueryParams = useServerTableQueryParams<TSortBy>({
    pagination,
    sorting,
    columnToSortMapping,
  });

  // Call API with default placeholderData (keeps previous data during pagination/sorting)
  const query = queryFn(
    {
      ...queryParams,
      ...tableQueryParams,
    } as TQueryParams & SortingQueryParams<TSortBy> & PaginationQueryParams,
    {
      ...queryOptions,
      query: {
        placeholderData: keepPreviousData,
        ...queryOptions?.query,
      },
    }
  );

  const items = query.data?.data?.items ?? [];
  const totalPages = query.data?.data?.totalPages;

  // Create the React Table instance via the useServerTableInstance hook
  // (preconfigured for server-side pagination, sorting, and filtering + more)
  const table = useServerTableInstance<TData>({
    columns,
    data: items,
    pageCount: totalPages,
    pagination,
    onPaginationChange,
    sorting,
    onSortingChange,
    defaultColumnVisibility,
  });

  return {
    table,
    query,
    items,
    totalPages,
  };
}
