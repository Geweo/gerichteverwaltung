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

/**
 * Query options factory for recipes.
 * TODO: Replace with generated query options once API client is available.
 */
export const queryOptions = {
  recipes: {
    list: (filters?: {
      mealCategory?: string;
      tags?: number[];
      source?: string;
      favorites?: boolean;
    }) => ({
      queryKey: ['recipes', filters] as const,
      queryFn: async () => {
        // TODO: Use generated API client
        return [];
      },
    }),
  },
};
