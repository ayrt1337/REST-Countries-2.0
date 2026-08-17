import React from 'react';
import { Link } from 'react-router';
import { Compass, ArrowLeft } from 'lucide-react';
import { Layout } from '../../components/layout/Layout';

export const NotFoundPage: React.FC = () => {
  return (
    <Layout>
      <div className="flex min-h-[60vh] flex-col items-center justify-center text-center">
        <div className="relative mb-6 flex h-28 w-28 items-center justify-center rounded-3xl bg-blue-50 shadow-inner dark:bg-[#2b3844] dark:shadow-black/20">
          <Compass className="h-14 w-14 text-blue-600 animate-spin-slow dark:text-blue-400" />
          <span className="absolute -top-2 -right-2 rounded-full bg-red-500 px-2.5 py-0.5 text-xs font-extrabold text-white shadow-sm">
            404
          </span>
        </div>

        <h1 className="mb-3 text-3xl font-extrabold tracking-tight text-gray-900 sm:text-5xl dark:text-white">
          Page Not Found
        </h1>

        <p className="mb-8 max-w-md text-base text-gray-600 sm:text-lg dark:text-gray-300">
          Looks like you've ventured into uncharted territory. The page you are looking for does not exist or has been moved.
        </p>

        <Link
          to="/"
          className="group flex cursor-pointer items-center gap-3 rounded-xl bg-blue-600 px-6 py-3.5 text-base font-bold text-white shadow-lg shadow-blue-500/25 transition-all duration-200 hover:bg-blue-700 hover:shadow-xl hover:shadow-blue-500/30 active:scale-95 dark:bg-blue-600 dark:hover:bg-blue-700"
        >
          <ArrowLeft className="h-5 w-5 transition-transform duration-200 group-hover:-translate-x-1" />
          <span>Back to Home</span>
        </Link>
      </div>
    </Layout>
  );
};

export default NotFoundPage;
