import { supabase } from './supabase';

/**
 * Custom API client instance for Orval-generated clients.
 * Handles authentication via Supabase JWT tokens.
 */
export const customInstance = async <T>(
  config: {
    url: string;
    method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
    params?: Record<string, unknown>;
    data?: unknown;
    headers?: Record<string, string>;
  },
  options?: {
    signal?: AbortSignal;
  }
): Promise<T> => {
  const { data: { session } } = await supabase.auth.getSession();
  const token = session?.access_token;

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...config.headers,
  };

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const url = new URL(config.url, import.meta.env.VITE_API_URL || 'http://localhost:5000');
  
  if (config.params) {
    Object.entries(config.params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        url.searchParams.append(key, String(value));
      }
    });
  }

  const response = await fetch(url.toString(), {
    method: config.method,
    headers,
    body: config.data ? JSON.stringify(config.data) : undefined,
    signal: options?.signal,
  });

  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: response.statusText }));
    throw new Error(error.message || `HTTP error! status: ${response.status}`);
  }

  if (response.status === 204 || response.headers.get('content-length') === '0') {
    return undefined as T;
  }

  return response.json() as Promise<T>;
};

export default customInstance;

