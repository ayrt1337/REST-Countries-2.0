import React from 'react';
import { Link } from 'react-router';
import type { CountryListItem } from '../../types/country';

interface CountryCardProps {
  country: CountryListItem;
}

export const CountryCard: React.FC<CountryCardProps> = ({ country }) => {
  const commonName = country.names?.common || country.names?.official || 'Unknown';
  const flagUrl = country.flag?.url_svg || '';
  const population = country.population ? country.population.toLocaleString('en-US') : '0';
  const capital = country.capitals?.[0]?.name || 'N/A';
  const alpha3 = country.codes?.alpha_3 || '';

  return (
    <Link
      to={alpha3 ? `/country/${alpha3.toLowerCase()}` : '#'}
      className="group flex flex-col overflow-hidden rounded-lg bg-white shadow-sm transition-all duration-300 hover:-translate-y-1.5 hover:shadow-xl dark:bg-[#2b3844] dark:shadow-md dark:hover:shadow-2xl dark:hover:shadow-black/40"
    >
      <div className="relative aspect-[16/10] w-full overflow-hidden bg-gray-100 dark:bg-slate-700/50">
        {flagUrl ? (
          <img
            src={flagUrl}
            alt={`Flag of ${commonName}`}
            loading="lazy"
            className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        ) : (
          <div className="flex h-full w-full items-center justify-center text-xs text-gray-400">
            No flag available
          </div>
        )}
      </div>

      <div className="flex flex-1 flex-col p-6 pb-8">
        <h2 className="mb-4 text-lg font-extrabold text-gray-900 line-clamp-1 transition-colors group-hover:text-blue-600 dark:text-white dark:group-hover:text-blue-400">
          {commonName}
        </h2>

        <div className="space-y-1.5 text-sm text-gray-800 dark:text-gray-200">
          <p>
            <span className="font-semibold text-gray-900 dark:text-white">Population: </span>
            <span>{population}</span>
          </p>
          <p>
            <span className="font-semibold text-gray-900 dark:text-white">Region: </span>
            <span>{country.region || (country as { region?: string }).region || 'N/A'}</span>
          </p>
          <p>
            <span className="font-semibold text-gray-900 dark:text-white">Capital: </span>
            <span>{capital}</span>
          </p>
        </div>
      </div>
    </Link>
  );
};
