import React from "react";
import { Link } from "react-router";
import { Moon, Sun } from "lucide-react";
import { useTheme } from "../../context/ThemeContext";

export const Navbar: React.FC = () => {
  const { theme, toggleTheme } = useTheme();

  return (
    <header className="sticky top-0 z-50 bg-white shadow-sm transition-colors duration-200 dark:bg-[#2b3844] dark:shadow-md dark:shadow-black/20">
      <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-6 sm:px-8">
        <Link
          to="/"
          className="text-lg font-extrabold tracking-tight text-gray-900 transition-opacity hover:opacity-85 sm:text-2xl dark:text-white"
        >
          Where in the world?
        </Link>

        <button
          type="button"
          onClick={toggleTheme}
          aria-label={
            theme === "dark" ? "Switch to Light Mode" : "Switch to Dark Mode"
          }
          className="flex cursor-pointer items-center gap-2 text-sm font-semibold text-gray-800 transition-opacity hover:opacity-75 focus:outline-hidden sm:text-base dark:text-white"
        >
          {theme === "dark" ? (
            <>
              <Sun className="h-4 w-4 fill-current sm:h-5 sm:w-5" />
              <span>Light Mode</span>
            </>
          ) : (
            <>
              <Moon className="h-4 w-4 sm:h-5 sm:w-5" />
              <span>Dark Mode</span>
            </>
          )}
        </button>
      </div>
    </header>
  );
};
