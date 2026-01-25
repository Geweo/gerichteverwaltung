import { useCallback } from 'react';
import type { NavigateOptions } from '@tanstack/react-router';

type UpdateSearchParamsOptions = Omit<NavigateOptions, 'search'>;

interface UseSearchParamsOptions<TSearchParams extends Record<string, unknown>> {
  /**
   * Validated search params from TanStack Router (after Valibot validation)
   */
  search: TSearchParams;

  /**
   * Navigate function from TanStack Router
   */
  navigate: (options: NavigateOptions) => void;
}

interface UseSearchParamsReturn<TSearchParams extends Record<string, unknown>> {
  /**
   * Generic handler to update any search param
   */
  updateSearchParam: <TKey extends keyof TSearchParams>(
    key: TKey,
    value: TSearchParams[TKey] | undefined,
    options?: UpdateSearchParamsOptions
  ) => void;

  /**
   * Generic handler to update multiple search params at once
   */
  updateSearchParams: (updates: Partial<TSearchParams>, options?: UpdateSearchParamsOptions) => void;

  /**
   * Generic handler to update any param that acts as a filter -
   * Works like `updateSearchParam` but with `replace: true`
   */
  updateFilter: <TKey extends keyof TSearchParams>(
    key: TKey,
    value: TSearchParams[TKey] | undefined,
    options?: UpdateSearchParamsOptions
  ) => void;

  /**
   * Generic handler to update multiple params that act as filters at once -
   * Works like `updateSearchParams` but with `replace: true` by default
   */
  updateFilters: (updates: Partial<TSearchParams>, options?: UpdateSearchParamsOptions) => void;

  /**
   * Similar to `updateFilters` but takes care of resetting the page to `1`
   */
  updateTableFilters: (updates: Partial<TSearchParams>, options?: UpdateSearchParamsOptions) => void;

  /**
   * remove table filter. Main usecase scenario is when clicking close icon of active filter tag.
   * used for multi select type filter (ex: tagIds, mealCategory)
   */
  removeTableMultiSelectFilter: <TKey extends keyof TSearchParams>(
    key: TKey,
    valueToRemove: TSearchParams[TKey]
  ) => void;
}

/**
 * Generic hook to update search params and synchronize them with the URL via navigate function.
 */
export function useSearchParams<TSearchParams extends Record<string, unknown>>({
  search,
  navigate,
}: UseSearchParamsOptions<TSearchParams>): UseSearchParamsReturn<TSearchParams> {
  // Generic update function
  const updateSearchParams = useCallback(
    (updates: Partial<TSearchParams>, options?: UpdateSearchParamsOptions) => {
      // Check if any values actually changed
      const hasChanges = Object.entries(updates).some(([key, value]) => {
        return search[key as keyof TSearchParams] !== value;
      });

      // Only navigate if something actually changed
      if (!hasChanges) return;

      navigate({
        search: (current) => ({
          ...current,
          ...updates,
        }),
        ...options,
      });
    },
    [navigate, search]
  );

  const updateSearchParam = useCallback(
    <TKey extends keyof TSearchParams>(
      key: TKey,
      value: TSearchParams[TKey] | undefined,
      options: UpdateSearchParamsOptions = { replace: false }
    ) => {
      const updates = { [key]: value } as Partial<TSearchParams>;
      updateSearchParams(updates, options);
    },
    [updateSearchParams]
  );

  const updateFilter = useCallback(
    <TKey extends keyof TSearchParams>(
      key: TKey,
      value: TSearchParams[TKey] | undefined,
      options: UpdateSearchParamsOptions = { replace: true }
    ) => {
      const updates = { [key]: value } as Partial<TSearchParams>;
      updateSearchParams(updates, options);
    },
    [updateSearchParams]
  );

  const updateFilters = useCallback(
    (updates: Partial<TSearchParams>, options: UpdateSearchParamsOptions = { replace: true }) => {
      updateSearchParams(updates, options);
    },
    [updateSearchParams]
  );

  const updateTableFilters = useCallback(
    (updates: Partial<TSearchParams>, options?: UpdateSearchParamsOptions) => {
      // Check if any filter values actually changed
      const filtersChanged = Object.entries(updates).some(([key, value]) => {
        return search[key as keyof TSearchParams] !== value;
      });

      // If no filters changed, don't navigate
      if (!filtersChanged) return;

      // Spreading order is important here as we want to allow to override the page parameter
      // by passing another value for page.
      updateFilters({ page: '1', ...updates }, options);
    },
    [updateFilters, search]
  );

  const removeTableMultiSelectFilter = useCallback(
    <TKey extends keyof TSearchParams>(key: TKey, valueToRemove: TSearchParams[TKey]) => {
      const currentFilter = search[key];

      if (!Array.isArray(currentFilter)) {
        return;
      }

      const updatedFilters = currentFilter.filter((val: string) => val !== valueToRemove);

      const updates = {
        page: '1',
        [key]: updatedFilters,
      } as unknown as Partial<TSearchParams>;

      updateFilters(updates);
    },
    [search, updateFilters]
  );

  return {
    updateSearchParam,
    updateSearchParams,
    updateFilter,
    updateFilters,
    updateTableFilters,
    removeTableMultiSelectFilter,
  };
}
