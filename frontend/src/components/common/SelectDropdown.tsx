import React, { useEffect, useRef, useState } from 'react';
import { ChevronDown } from 'lucide-react';

export interface SelectOption {
  value: string;
  label: string;
}

interface SelectDropdownProps {
  options: SelectOption[];
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  className?: string;
}

export const SelectDropdown: React.FC<SelectDropdownProps> = ({
  options,
  value,
  onChange,
  placeholder = 'Filter by Region',
  className = '',
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const selectedOption = options.find((opt) => opt.value === value);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    document.addEventListener('keydown', handleKeyDown);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      document.removeEventListener('keydown', handleKeyDown);
    };
  }, []);

  const handleSelect = (val: string) => {
    onChange(val);
    setIsOpen(false);
  };

  return (
    <div ref={dropdownRef} className={`relative w-56 ${className}`}>
      <button
        type="button"
        id="region-filter-button"
        onClick={() => setIsOpen((prev) => !prev)}
        aria-haspopup="listbox"
        aria-expanded={isOpen}
        className="flex w-full cursor-pointer items-center justify-between rounded-lg bg-white px-6 py-4 text-sm font-semibold text-gray-900 shadow-sm transition-all duration-200 hover:shadow-md focus:outline-hidden dark:bg-[#2b3844] dark:text-white dark:shadow-md"
      >
        <span className="truncate font-semibold">
          {selectedOption ? selectedOption.label : placeholder}
        </span>
        <ChevronDown
          className={`h-4 w-4 text-gray-700 transition-transform duration-200 dark:text-gray-200 ${
            isOpen ? 'rotate-180' : ''
          }`}
        />
      </button>

      {isOpen && (
        <ul
          role="listbox"
          className="absolute z-30 mt-2 w-full overflow-hidden rounded-lg bg-white py-2 shadow-lg transition-all duration-200 animate-in fade-in slide-in-from-top-2 dark:bg-[#2b3844] dark:shadow-xl dark:shadow-black/30"
        >
          {options.map((option) => {
            const isSelected = option.value === value;
            return (
              <li key={option.value || 'all-regions'}>
                <button
                  type="button"
                  role="option"
                  aria-selected={isSelected}
                  onClick={() => handleSelect(option.value)}
                  className={`w-full cursor-pointer px-6 py-2.5 text-left text-sm font-semibold transition-colors duration-150 ${
                    isSelected
                      ? 'bg-blue-50 text-blue-600 dark:bg-slate-700/60 dark:text-blue-300'
                      : 'text-gray-800 hover:bg-gray-50 hover:text-black dark:text-gray-100 dark:hover:bg-slate-700/40 dark:hover:text-white'
                  }`}
                >
                  {option.label}
                </button>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};
