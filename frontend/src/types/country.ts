export interface CountryNames {
  common: string;
  official: string;
}

export interface CountryCodes {
  alpha_3: string;
}

export interface CountryCapital {
  name: string;
}

export interface CountryFlag {
  url_svg: string;
}

export interface CountryListItem {
  names: CountryNames;
  codes: CountryCodes;
  capitals: CountryCapital[];
  flag: CountryFlag;
  population: number;
  region?: string;
}

export interface PaginationData {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  count: number;
}

export type PaginationMeta = PaginationData;

export interface ListCountriesResponse {
  pagination: PaginationData;
  objects: CountryListItem[];
}

export interface CountryCurrency {
  name: string;
  symbol?: string;
}

export interface BorderCountry {
  commonName: string;
  alpha3: string;
}

export interface CountryDetailItem {
  names: CountryNames;
  codes: CountryCodes;
  capitals?: CountryCapital[];
  flag: CountryFlag;
  currencies?: CountryCurrency[];
  region?: string;
  subregion?: string;
  continents?: string[];
  borders?: Array<BorderCountry | string>;
  population?: number;
  tld?: string[];
}

export interface CountryDetailResponse {
  data?: {
    objects?: CountryDetailItem[];
  };
  objects?: CountryDetailItem[];
}

export interface ListCountriesQueryParams {
  name?: string;
  region?: string;
  page?: number;
}

export type RegionOption = 'Africa' | 'Americas' | 'Asia' | 'Europe' | 'Oceania' | '';
