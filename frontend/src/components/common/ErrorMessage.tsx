import React from 'react';
import { AlertCircle, RefreshCcw } from 'lucide-react';

interface ErrorMessageProps {
  title?: string;
  message?: string;
  onRetry?: () => void;
  retryLabel?: string;
}

export const ErrorMessage: React.FC<ErrorMessageProps> = ({
  title = 'Error loading data',
  message = 'Unable to communicate with the server. Please check your connection and try again.',
  onRetry,
  retryLabel = 'Try again',
}) => {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl bg-white px-6 py-16 text-center shadow-sm dark:bg-[#2b3844] dark:shadow-md">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-red-50 text-red-600 dark:bg-red-950/40 dark:text-red-400">
        <AlertCircle className="h-8 w-8" />
      </div>
      <h3 className="mb-2 text-xl font-bold text-gray-900 dark:text-white sm:text-2xl">
        {title}
      </h3>
      <p className="mb-6 max-w-md text-sm text-gray-600 dark:text-gray-300 sm:text-base">
        {message}
      </p>
      {onRetry && (
        <button
          type="button"
          onClick={onRetry}
          className="flex cursor-pointer items-center gap-2 rounded-lg bg-red-600 px-5 py-3 text-sm font-bold text-white transition-all hover:bg-red-700 active:scale-95 dark:bg-red-600 dark:hover:bg-red-700"
        >
          <RefreshCcw className="h-4 w-4" />
          <span>{retryLabel}</span>
        </button>
      )}
    </div>
  );
};
