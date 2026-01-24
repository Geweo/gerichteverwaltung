import type { UseQueryOptions } from '@tanstack/react-query';

/**
 * Custom query options for Orval-generated hooks.
 * Provides default configuration like staleTime for all queries.
 */
export const useQueryOptions = <
  TData = unknown,
  TError = Error,
  TQueryKey extends readonly unknown[] = readonly unknown[]
>(
  options: UseQueryOptions<TData, TError, TData, TQueryKey>
): UseQueryOptions<TData, TError, TData, TQueryKey> => {
  return {
    ...options,
    staleTime: 1000 * 60 * 5, // 5 minutes
  };
};
