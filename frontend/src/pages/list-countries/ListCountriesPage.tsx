import React, { useCallback, useEffect, useState } from 'react';
import { useSearchParams } from 'react-router';
import { Layout } from '../../components/layout/Layout';
import { SearchInput } from '../../components/common/SearchInput';
import { SelectDropdown, type SelectOption } from '../../components/common/SelectDropdown';
import { CountryGrid } from '../../components/country/CountryGrid';
import { LoadingSkeleton } from '../../components/common/LoadingSkeleton';
import { EmptyState } from '../../components/common/EmptyState';
import { ErrorMessage } from '../../components/common/ErrorMessage';
import { Pagination } from '../../components/common/Pagination';
import { fetchCountries } from '../../services/country-service';
import { getApiErrorMessage } from '../../services/verify-api-error';
import type { CountryListItem, PaginationMeta } from '../../types/country';

// A PAGINAÇÃO DEVE REDIRECIONAR PARA UM ENDEREÇO PARA FICAR NA STACK NO HISTORICO

const REGION_OPTIONS: SelectOption[] = [
  { value: '', label: 'Filter by Region' },
  { value: 'Africa', label: 'Africa' },
  { value: 'Americas', label: 'America' },
  { value: 'Asia', label: 'Asia' },
  { value: 'Europe', label: 'Europe' },
  { value: 'Oceania', label: 'Oceania' },
];

export const ListCountriesPage: React.FC = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const initialSearch = searchParams.get('search') || '';
  const initialRegion = searchParams.get('region') || '';
  const initialPage = parseInt(searchParams.get('page') || '1', 10);

  // TODO DEIXAR APENAS UM USE STATE PARA FILTRO

  const [searchInput, setSearchInput] = useState<string>(initialSearch);
  const [selectedRegion, setSelectedRegion] = useState<string>(initialRegion);

  const [activeSearch, setActiveSearch] = useState<string>(initialSearch);
  const [activeRegion, setActiveRegion] = useState<string>(initialRegion);
  const [currentPage, setCurrentPage] = useState<number>(isNaN(initialPage) ? 1 : initialPage);

  const [countries, setCountries] = useState<CountryListItem[]>([]);
  const [pagination, setPagination] = useState<PaginationMeta>({
    currentPage: 1,
    totalPages: 1,
    totalItems: 0,
    pageSize: 50,
    count: 0,
  });
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const updateUrlParams = useCallback(
    (newSearch: string, newRegion: string, newPage: number) => {
      const params = new URLSearchParams();
      if (newSearch.trim()) params.set('search', newSearch.trim());
      if (newRegion.trim()) params.set('region', newRegion.trim());
      if (newPage > 1) params.set('page', newPage.toString());

      setSearchParams(params, { replace: true });
    },
    [setSearchParams]
  );

  const loadCountries = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    try {
      const result = await fetchCountries({
        name: activeSearch,
        region: activeRegion,
        page: currentPage,
      });

      setCountries(result.countries);
      setPagination(result.pagination);
    } catch (err: unknown) {
      console.error('Failed to load countries:', err);
      setError(getApiErrorMessage(err, 'Unable to fetch countries list.'));
    } finally {
      setIsLoading(false);
    }
  }, [activeSearch, activeRegion, currentPage]);

  useEffect(() => {
    loadCountries();
  }, [loadCountries]);

  const handleSearchSubmit = () => {
    setActiveSearch(searchInput);
    setActiveRegion(selectedRegion);
    setCurrentPage(1);
    updateUrlParams(searchInput, selectedRegion, 1);
  };

  const handleRegionChange = (value: string) => {
    setSelectedRegion(value);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    updateUrlParams(activeSearch, activeRegion, page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const handleResetFilters = () => {
    setSearchInput('');
    setSelectedRegion('');
    setActiveSearch('');
    setActiveRegion('');
    setCurrentPage(1);
    updateUrlParams('', '', 1);
  };

  return (
    <Layout>
      <section className="mb-10 flex flex-col items-stretch justify-between gap-6 sm:flex-row sm:items-center sm:gap-4">
        <div className="flex flex-1 flex-col gap-3 sm:flex-row sm:items-center">
          <SearchInput
            value={searchInput}
            onChange={setSearchInput}
            onSearch={handleSearchSubmit}
            placeholder="Search for a country..."
          />

          <button
            type="button"
            onClick={handleSearchSubmit}
            aria-label="Search"
            className="flex h-14 cursor-pointer items-center justify-center rounded-lg bg-blue-600 px-6 text-sm font-bold text-white shadow-sm transition-all hover:bg-blue-700 active:scale-95 dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            Search
          </button>
        </div>

        <SelectDropdown
          options={REGION_OPTIONS}
          value={selectedRegion}
          onChange={handleRegionChange}
          placeholder="Filter by Region"
        />
      </section>

      <section aria-label="Countries list">
        {isLoading ? (
          <LoadingSkeleton count={8} />
        ) : error ? (
          <ErrorMessage
            title="Connection error"
            message={error}
            onRetry={loadCountries}
          />
        ) : countries.length === 0 ? (
          <EmptyState
            title="No countries found"
            description="No countries match the applied filters. Try searching for another keyword or resetting the region filter."
            onReset={handleResetFilters}
          />
        ) : (
          <>
            <CountryGrid countries={countries} />

            <Pagination
              currentPage={currentPage}
              totalItems={pagination.totalItems || countries.length}
              pageSize={pagination.pageSize || 50}
              onPageChange={handlePageChange}
            />
          </>
        )}
      </section>
    </Layout>
  );
};

export default ListCountriesPage;
