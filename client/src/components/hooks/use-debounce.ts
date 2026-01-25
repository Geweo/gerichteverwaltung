import { useEffect } from 'react';

/**
 * Custom debounce hook
 * @param fn - Function to debounce
 * @param delay - Delay in milliseconds
 * @param deps - Dependencies array
 */
export function useDebounce(fn: () => void, delay: number, deps: unknown[]) {
  useEffect(() => {
    const timer = setTimeout(() => {
      fn();
    }, delay);

    return () => {
      clearTimeout(timer);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps);
}
