import { api } from "./api";
import type { ApiResponse } from "../types/api";
import type {
  CountryDetailItem,
  CountryListItem,
  ListCountriesQueryParams,
  ListCountriesResponse,
  PaginationMeta,
} from "../types/country";

export async function fetchCountries(
  params: ListCountriesQueryParams = {}
): Promise<{ countries: CountryListItem[]; pagination: PaginationMeta }> {
  const queryParams: Record<string, string | number> = {};

  if (params.name && params.name.trim()) {
    queryParams.name = params.name.trim();
  }

  if (params.region && params.region.trim()) {
    queryParams.region = params.region.trim();
  }

  if (params.page && params.page > 0) {
    queryParams.page = params.page;
  }

  const response = await api.get<ApiResponse<ListCountriesResponse>>('/countries/list', {
    params: queryParams,
  });

  const responseData = response.data?.data;

  const countries = responseData?.objects || [];
  const pagination: PaginationMeta = responseData?.pagination || {
    currentPage: 1,
    totalPages: 1,
    totalItems: countries.length,
    pageSize: 50,
    count: countries.length,
  };

  return {
    countries,
    pagination,
  };
}

export async function fetchCountryByCode(code: string): Promise<CountryDetailItem> {
  const response = await api.get<ApiResponse<any>>(`/countries/${encodeURIComponent(code)}`);
  const responseData = response.data?.data;

  const objects: CountryDetailItem[] = responseData?.data?.objects; 

  if (!objects || objects.length === 0) {
    throw new Error('Country not found.');
  }

  return objects[0];
}
