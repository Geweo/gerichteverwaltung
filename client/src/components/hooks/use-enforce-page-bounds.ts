import { useEffect } from 'react';
import type { PaginationState } from '@tanstack/react-table';

interface UseEnforcePageBoundsOptions {
  /**
   * Current pagination state (TanStack format with 0-based pageIndex)
   */
  pagination: PaginationState;

  /**
   * Total number of pages available (1-based)
   */
  totalPages?: number;

  /**
   * Callback to change the pagination state
   */
  onPaginationChange: (pagination: PaginationState) => void;

  /**
   * Whether to enforce page bounds. Defaults to true.
   */
  enabled?: boolean;
}

/**
 * Enforces that the current page is within valid bounds and corrects if needed.
 * Works with TanStack Table's PaginationState (0-based pageIndex).
 * Handles edge cases:
 * - If totalPages is 0, redirects to pageIndex 0 (page 1)
 * - If pageIndex is less than 0, redirects to pageIndex 0 (page 1)
 * - If pageIndex exceeds totalPages, redirects to the last valid pageIndex
 */
export function useEnforcePageBounds({
  pagination,
  totalPages,
  onPaginationChange,
  enabled = true,
}: UseEnforcePageBoundsOptions) {
  useEffect(() => {
    if (!enabled || totalPages === undefined) return;

    // Convert 0-based pageIndex to 1-based page for validation
    const currentPage = pagination.pageIndex + 1;

    if (totalPages === 0) {
      // No pages available, redirect to pageIndex 0 (page 1)
      if (pagination.pageIndex !== 0) {
        onPaginationChange({ ...pagination, pageIndex: 0 });
      }
    } else if (pagination.pageIndex < 0) {
      // Negative pageIndex, redirect to pageIndex 0 (page 1)
      onPaginationChange({ ...pagination, pageIndex: 0 });
    } else if (currentPage > totalPages) {
      // Page exceeds max, redirect to last valid pageIndex (totalPages - 1)
      onPaginationChange({ ...pagination, pageIndex: totalPages - 1 });
    }
  }, [enabled, totalPages, pagination, onPaginationChange]);
}
