import React from 'react';
import { Search, X } from 'lucide-react';

interface SearchInputProps {
  value: string;
  onChange: (value: string) => void;
  onSearch?: () => void;
  placeholder?: string;
  className?: string;
}

export const SearchInput: React.FC<SearchInputProps> = ({
  value,
  onChange,
  onSearch,
  placeholder = 'Search for a country...',
  className = '',
}) => {
  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      onSearch?.();
    }
  };

  const handleClear = () => {
    onChange('');
  };

  return (
    <div
      className={`relative flex h-14 w-full max-w-md items-center rounded-lg bg-white shadow-sm transition-all duration-200 focus-within:ring-2 focus-within:ring-blue-500/20 dark:bg-[#2b3844] dark:shadow-md dark:focus-within:ring-blue-400/20 ${className}`}
    >
      <div className="pointer-events-none pl-6 pr-3 text-gray-400 dark:text-gray-300">
        <Search className="h-5 w-5" />
      </div>
      <input
        type="text"
        id="country-search-input"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={handleKeyDown}
        placeholder={placeholder}
        aria-label={placeholder}
        className="h-full w-full bg-transparent pr-10 text-sm font-semibold text-gray-900 placeholder:text-gray-400 placeholder:font-normal focus:outline-hidden dark:text-white dark:placeholder:text-gray-300"
      />
      {value && (
        <button
          type="button"
          onClick={handleClear}
          aria-label="Clear search"
          className="absolute right-4 rounded-full p-1 text-gray-400 hover:bg-gray-100 hover:text-gray-600 dark:text-gray-300 dark:hover:bg-slate-700/50 dark:hover:text-white"
        >
          <X className="h-4 w-4" />
        </button>
      )}
    </div>
  );
};

