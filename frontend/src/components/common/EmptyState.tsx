import React from 'react';
import { SearchX, RotateCcw } from 'lucide-react';

interface EmptyStateProps {
  title?: string;
  description?: string;
  onReset?: () => void;
  resetLabel?: string;
}

export const EmptyState: React.FC<EmptyStateProps> = ({
  title = 'No countries found',
  description = 'Try adjusting your search or region filter to find what you are looking for.',
  onReset,
  resetLabel = 'Clear filters',
}) => {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl bg-white px-6 py-16 text-center shadow-sm dark:bg-[#2b3844] dark:shadow-md">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-blue-50 text-blue-600 dark:bg-slate-700/60 dark:text-blue-400">
        <SearchX className="h-8 w-8" />
      </div>
      <h3 className="mb-2 text-xl font-bold text-gray-900 dark:text-white sm:text-2xl">
        {title}
      </h3>
      <p className="mb-6 max-w-md text-sm text-gray-600 dark:text-gray-300 sm:text-base">
        {description}
      </p>
      {onReset && (
        <button
          type="button"
          onClick={onReset}
          className="flex cursor-pointer items-center gap-2 rounded-lg bg-blue-600 px-5 py-3 text-sm font-bold text-white shadow-md shadow-blue-500/20 transition-all hover:bg-blue-700 active:scale-95 dark:bg-blue-600 dark:hover:bg-blue-700"
        >
          <RotateCcw className="h-4 w-4" />
          <span>{resetLabel}</span>
        </button>
      )}
    </div>
  );
};
