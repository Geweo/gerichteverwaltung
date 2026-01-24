import axios, { type AxiosRequestConfig, type AxiosResponse } from 'axios';
import { supabase } from './supabase';

/**
 * Custom API client instance for Orval-generated clients.
 * Uses axios with Supabase JWT token authentication.
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

  const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

  const axiosConfig: AxiosRequestConfig = {
    baseURL,
    url: config.url,
    method: config.method.toLowerCase() as 'get' | 'post' | 'put' | 'delete' | 'patch',
    params: config.params,
    data: config.data,
    headers: {
      'Content-Type': 'application/json',
      ...config.headers,
      ...(token && { Authorization: `Bearer ${token}` }),
    },
    signal: options?.signal,
  };

  try {
    const response: AxiosResponse<T> = await axios(axiosConfig);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      // Backend gibt Fehler als { error: "..." } oder { error: "...", errors: [...] } zurück
      const errorData = error.response?.data as { error?: string; errors?: Array<{ field?: string; message: string }> } | undefined;
      
      if (errorData?.errors && errorData.errors.length > 0) {
        // FluentValidation-Fehler: mehrere Fehler
        const errorMessages = errorData.errors.map(e => e.message).join(', ');
        throw new Error(errorMessages);
      } else if (errorData?.error) {
        // Einzelner Fehler
        throw new Error(errorData.error);
      } else {
        // Fallback
        throw new Error(error.message || 'An error occurred');
      }
    }
    throw error;
  }
};

export default customInstance;
