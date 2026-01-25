import axios, { type AxiosRequestConfig, type AxiosError, type AxiosResponse } from 'axios';
import { supabase } from './supabase';

/**
 * Configure axios defaults for Orval-generated API calls.
 * 
 * This replaces the customInstance mutator approach with direct axios usage
 * (like zentreo pattern), using interceptors for authentication and error handling.
 */

// Set baseURL for all axios requests
const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5000';
axios.defaults.baseURL = baseURL;

// Set default headers
axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.timeout = 30000; // 30 seconds

// Serialize params without indexes (no array brackets), ASP.NET core expects &id=1&id=2
axios.defaults.paramsSerializer = {
  indexes: null,
};

/**
 * Request Interceptor: Add Supabase JWT token to all requests
 */
axios.interceptors.request.use(
  async (config) => {
    try {
      const { data: { session } } = await supabase.auth.getSession();
      const token = session?.access_token;
      
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    } catch (error) {
      // If getting session fails, continue without token
      console.warn('Failed to get Supabase session:', error);
    }
    
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

/**
 * Response Interceptor: Handle common errors and parse responses
 */
axios.interceptors.response.use(
  (response: AxiosResponse) => {
    // Parse JSON when responseType is 'text' but content-type is JSON
    // This fixes orval-generated endpoints that use responseType: 'text'
    if (typeof response.data === 'string' && response.headers['content-type']?.includes('application/json')) {
      try {
        response.data = JSON.parse(response.data);
      } catch (e) {
        // If parsing fails, leave it as a string
        console.warn('Failed to parse JSON response:', e);
      }
    }
    return response;
  },
  (error: AxiosError) => {
    // Handle backend error responses
    if (error.response?.data) {
      const errorData = error.response.data as 
        | { error?: string; errors?: Array<{ field?: string; message: string }> } 
        | undefined;
      
      if (errorData?.errors && errorData.errors.length > 0) {
        // FluentValidation-Fehler: mehrere Fehler
        const errorMessages = errorData.errors.map(e => e.message).join(', ');
        error.message = errorMessages;
      } else if (errorData?.error) {
        // Einzelner Fehler
        error.message = errorData.error;
      }
    }
    
    // Handle HTTP status codes
    if (error.response?.status === 401) {
      console.error('Unauthorized - Token may be invalid or expired');
      // TODO: Optional: Redirect to login or refresh token
    } else if (error.response?.status === 500) {
      console.error('500 - Internal server error');
    } else if (error.response?.status === 503) {
      console.error('503 - Server unavailable');
    }
    
    return Promise.reject(error);
  }
);

export default axios;
