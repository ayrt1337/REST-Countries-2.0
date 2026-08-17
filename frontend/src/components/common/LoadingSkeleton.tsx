import React from 'react';

interface LoadingSkeletonProps {
  count?: number;
}

export const LoadingSkeleton: React.FC<LoadingSkeletonProps> = ({ count = 8 }) => {
  return (
    <div className="grid grid-cols-1 gap-12 sm:grid-cols-2 lg:grid-cols-4">
      {Array.from({ length: count }).map((_, index) => (
        <div
          key={`skeleton-${index}`}
          className="overflow-hidden rounded-lg bg-white shadow-md dark:bg-[#2b3844] animate-pulse"
        >
          <div className="h-44 w-full bg-gray-200 dark:bg-slate-700" />

          <div className="p-6">
            <div className="mb-4 h-6 w-3/4 rounded-md bg-gray-200 dark:bg-slate-700" />
            <div className="space-y-2">
              <div className="h-4 w-1/2 rounded-md bg-gray-200 dark:bg-slate-700" />
              <div className="h-4 w-2/5 rounded-md bg-gray-200 dark:bg-slate-700" />
              <div className="h-4 w-3/5 rounded-md bg-gray-200 dark:bg-slate-700" />
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
