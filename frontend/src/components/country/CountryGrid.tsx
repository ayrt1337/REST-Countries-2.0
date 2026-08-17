import React from 'react';
import type { CountryListItem } from '../../types/country';
import { CountryCard } from './CountryCard';

interface CountryGridProps {
  countries: CountryListItem[];
}

export const CountryGrid: React.FC<CountryGridProps> = ({ countries }) => {
  return (
    <div className="grid grid-cols-1 gap-12 sm:grid-cols-2 lg:grid-cols-4">
      {countries.map((country, index) => {
        const key = country.codes?.alpha_3 || country.names?.common || `country-${index}`;
        return <CountryCard key={key} country={country} />;
      })}
    </div>
  );
};
