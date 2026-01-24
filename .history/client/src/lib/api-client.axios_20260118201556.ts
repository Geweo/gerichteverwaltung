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
      const errorMessage = error.response?.data?.message || error.message || 'An error occurred';
      throw new Error(errorMessage);
    }
    throw error;
  }
};

export default customInstance;
