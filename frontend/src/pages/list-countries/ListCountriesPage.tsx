import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router";
import { Layout } from "../../components/layout/Layout";
import { SearchInput } from "../../components/common/SearchInput";
import {
  SelectDropdown,
  type SelectOption,
} from "../../components/common/SelectDropdown";
import { CountryGrid } from "../../components/country/CountryGrid";
import { LoadingSkeleton } from "../../components/common/LoadingSkeleton";
import { EmptyState } from "../../components/common/EmptyState";
import { ErrorMessage } from "../../components/common/ErrorMessage";
import { Pagination } from "../../components/common/Pagination";
import { fetchCountries } from "../../services/country-service";
import { getApiErrorMessage } from "../../services/verify-api-error";
import type { CountryListItem, PaginationMeta } from "../../types/country";

const REGION_OPTIONS: SelectOption[] = [
  { value: "", label: "Filter by Region" },
  { value: "Africa", label: "Africa" },
  { value: "Americas", label: "Americas" },
  { value: "Asia", label: "Asia" },
  { value: "Europe", label: "Europe" },
  { value: "Oceania", label: "Oceania" },
];

export const ListCountriesPage: React.FC = () => {
  const [searchParams, setSearchParams] = useSearchParams();

  const appliedSearch = searchParams.get("search") || "";
  const appliedRegion = searchParams.get("region") || "";
  const currentPage = Math.max(1, parseInt(searchParams.get("page") || "1", 10) || 1);

  const [searchInput, setSearchInput] = useState(appliedSearch);
  const [selectedRegion, setSelectedRegion] = useState(appliedRegion);

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

  const [retryTrigger, setRetryTrigger] = useState(0);

  useEffect(() => {
    setSearchInput(appliedSearch);
    setSelectedRegion(appliedRegion);
    setIsLoading(true);

    fetchCountries({
      name: appliedSearch,
      region: appliedRegion,
      page: currentPage,
    })
      .then((result) => {
        setCountries(result.countries);
        setPagination(result.pagination);
        setError(null);
      })
      .catch((err: unknown) => {
        console.error("Failed to load countries:", err);
        setError(getApiErrorMessage(err, "Unable to fetch countries list."));
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [appliedSearch, appliedRegion, currentPage, retryTrigger]);

  const handleRetry = () => {
    setIsLoading(true);
    setRetryTrigger((prev) => prev + 1);
  };

  const handleSearchSubmit = (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    const params = new URLSearchParams();

    if (appliedRegion !== selectedRegion || appliedSearch !== searchInput){
      console.log(appliedRegion, selectedRegion, appliedSearch, searchInput)
      if (searchInput.trim()) params.set("search", searchInput.trim());
      if (selectedRegion.trim()) params.set("region", selectedRegion.trim());
      setSearchParams(params);
    }
  };

  const handlePageChange = (page: number) => {
    const params = new URLSearchParams();
    if (appliedSearch.trim()) params.set("search", appliedSearch.trim());
    if (appliedRegion.trim()) params.set("region", appliedRegion.trim());
    if (page > 1) params.set("page", page.toString());

    setSearchParams(params);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  const handleResetFilters = () => {
    setSearchInput("");
    setSelectedRegion("");
    setSearchParams(new URLSearchParams());
  };

  return (
    <Layout>
      <form
        onSubmit={handleSearchSubmit}
        className="mb-10 flex flex-col items-stretch gap-4 md:flex-row md:items-center md:justify-between"
      >
        <SearchInput
          value={searchInput}
          onChange={setSearchInput}
          onSearch={handleSearchSubmit}
          placeholder="Search for a country..."
        />

        <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
          <SelectDropdown
            options={REGION_OPTIONS}
            value={selectedRegion}
            onChange={setSelectedRegion}
            placeholder="Filter by Region"
            className="w-full sm:w-56"
          />

          <button
            type="submit"
            aria-label="Search"
            className="flex h-14 cursor-pointer items-center justify-center rounded-lg bg-blue-600 px-7 text-sm font-bold text-white shadow-sm transition-all hover:bg-blue-700 active:scale-95 dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            Search
          </button>
        </div>
      </form>

      <section aria-label="Countries list">
        {isLoading ? (
          <LoadingSkeleton count={8} />
        ) : error ? (
          <ErrorMessage
            title="Connection error"
            message={error}
            onRetry={handleRetry}
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
