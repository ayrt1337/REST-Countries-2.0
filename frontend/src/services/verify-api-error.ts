import axios from 'axios';
import type { ApiResponse } from '../types/api';

export function getApiErrorMessage(
  error: unknown,
  fallbackMessage = 'An error occurred while processing your request.'
): string {
  if (axios.isAxiosError<ApiResponse<unknown>>(error)) {
    if (error.response?.data?.message) {
      return error.response.data.message;
    }
    if (error.response?.status === 500) {
      return 'An internal server error occurred (500). Please try again later.';
    }
  }
  if (error instanceof Error) {
    return error.message;
  }
  return fallbackMessage;
}
