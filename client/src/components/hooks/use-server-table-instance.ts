import { useState } from 'react';
import {
  type ColumnDef,
  getCoreRowModel,
  type PaginationState,
  type SortingState,
  useReactTable,
  type VisibilityState,
} from '@tanstack/react-table';

import { useEnforcePageBounds } from '@/components/hooks/use-enforce-page-bounds';

/**
 * Sets up a React Table instance for server-side tables.
 * Manages additional local UI state (row selection, column visibility) and configures
 * the table to be ready for server-side pagination, sorting, and filtering.
 * Optionally enforces page bounds to ensure pagination stays within valid range. (true by default)
 *
 * @example
 * ```tsx
 * const table = useServerTableInstance({
 *   columns,
 *   data: items,
 *   pageCount: totalPages,
 *   pagination,
 *   onPaginationChange,
 *   sorting,
 *   onSortingChange,
 * })
 * ```
 */
export function useServerTableInstance<TData>({
  columns,
  data,
  pageCount,
  pagination,
  onPaginationChange,
  sorting,
  onSortingChange,
  defaultColumnVisibility = {},
  enforcePageBounds = true,
}: {
  /**
   * Columns definition
   */
  columns: ColumnDef<TData, unknown>[];
  /**
   * Data for the table coming from the API
   */
  data: TData[];
  /**
   * Total number of pages (used to initialize the table)
   */
  pageCount: number | undefined;
  /**
   * Pagination state (used to initialize the pagination state and to access the pagination state within the table)
   */
  pagination: PaginationState;
  /**
   * Callback to handle pagination changes.
   */
  onPaginationChange: (p: PaginationState) => void;
  /**
   * Sorting state (used to initialize the sorting state and to access the sorting state within the table)
   */
  sorting?: SortingState;
  /**
   * Callback to handle sorting changes.
   */
  onSortingChange?: (s: SortingState | undefined) => void;
  /**
   * Default column visibility state (used to initialize the column visibility state)
   */
  defaultColumnVisibility?: VisibilityState;
  /**
   * Whether to enforce page bounds. Defaults to true.
   * When enabled, automatically corrects pagination if it goes out of bounds.
   */
  enforcePageBounds?: boolean;
}) {
  // Local UI states to manage additional UI state like row selection and column visibility
  const [rowSelection, setRowSelection] = useState<Record<string, boolean>>({});
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(defaultColumnVisibility);

  // Enforce page bounds if enabled (pageCount is 1-based totalPages)
  useEnforcePageBounds({
    pagination,
    totalPages: pageCount,
    onPaginationChange,
    enabled: enforcePageBounds,
  });

  // Create the React Table instance with preconfigured options for server-side pagination, sorting, and filtering
  const table = useReactTable({
    columns,
    data,
    pageCount: pageCount ?? -1,
    state: {
      rowSelection,
      columnVisibility,
      sorting: sorting ?? [],
      pagination,
    },
    enableRowSelection: true,
    enableSorting: true,
    manualPagination: true,
    manualSorting: true,
    manualFiltering: true,
    onRowSelectionChange: setRowSelection,
    onSortingChange: onSortingChange
      ? (updater: SortingState | ((old: SortingState) => SortingState)) =>
          onSortingChange(typeof updater === 'function' ? updater(sorting ?? []) : updater)
      : undefined,
    onColumnVisibilityChange: setColumnVisibility,
    onPaginationChange: (updater: PaginationState | ((old: PaginationState) => PaginationState)) =>
      onPaginationChange(typeof updater === 'function' ? updater(pagination) : updater),
    getCoreRowModel: getCoreRowModel(),
  });

  return table;
}
