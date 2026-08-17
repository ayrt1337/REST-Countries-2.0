import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router';
import { ArrowLeft } from 'lucide-react';
import { Layout } from '../../components/layout/Layout';
import { ErrorMessage } from '../../components/common/ErrorMessage';
import { fetchCountryByCode } from '../../services/country-service';
import { getApiErrorMessage } from '../../services/verify-api-error';
import type { CountryDetailItem } from '../../types/country';

export const CountryDetailsPage: React.FC = () => {
  const { code } = useParams<{ code: string }>();
  const navigate = useNavigate();

  const [country, setCountry] = useState<CountryDetailItem | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!code) {
      setError('Country code not provided.');
      setIsLoading(false);
      return;
    }

    let isMounted = true;
    setIsLoading(true);
    setError(null);

    fetchCountryByCode(code)
      .then((data) => {
        if (isMounted) {
          setCountry(data);
        }
      })
      .catch((err) => {
        if (isMounted) {
          console.error('Failed to load country details:', err);
          setError(getApiErrorMessage(err, 'Unable to load country details.'));
        }
      })
      .finally(() => {
        if (isMounted) {
          setIsLoading(false);
        }
      });

    return () => {
      isMounted = false;
    };
  }, [code]);

  const handleBack = () => {
    if (window.history.length > 2) {
      navigate(-1);
    } else {
      navigate('/');
    }
  };

  const commonName = country?.names?.common || country?.names?.official || 'Unknown Country';
  const nativeName = country?.names?.official || country?.names?.common || 'N/A';
  const populationText = country?.population != null ? country.population.toLocaleString('en-US') : '0';
  const regionText = country?.region || 'N/A';
  const subregionText = country?.subregion || 'N/A';
  
  const capitalsText = country?.capitals && country.capitals.length > 0
    ? country.capitals.map((c) => c.name).filter(Boolean).join(', ')
    : 'N/A';

  const currenciesText = country?.currencies && country.currencies.length > 0
    ? country.currencies.map((c) => c.name).filter(Boolean).join(', ')
    : 'N/A';

  const borders = country?.borders || [];

  return (
    <Layout>
      <div className="mb-8">
        <button
          type="button"
          onClick={handleBack}
          className="group inline-flex cursor-pointer items-center gap-2.5 rounded-lg bg-white px-8 py-2.5 text-sm font-semibold text-gray-800 shadow-md transition-all duration-200 hover:bg-gray-50 hover:shadow-lg active:scale-95 dark:bg-[#2b3844] dark:text-white dark:hover:bg-slate-700/60"
        >
          <ArrowLeft className="h-4 w-4 transition-transform duration-200 group-hover:-translate-x-1" />
          <span>Back</span>
        </button>
      </div>

      {isLoading ? (
        <div className="mt-8 grid grid-cols-1 items-center gap-12 lg:grid-cols-2 lg:gap-24 animate-pulse">
          <div className="aspect-[16/10] w-full rounded-xl bg-gray-200 dark:bg-slate-700/50 shadow-lg" />
          <div className="space-y-6">
            <div className="h-10 w-3/4 rounded-lg bg-gray-200 dark:bg-slate-700/50" />
            <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div className="space-y-3">
                <div className="h-5 w-full rounded bg-gray-200 dark:bg-slate-700/50" />
                <div className="h-5 w-4/5 rounded bg-gray-200 dark:bg-slate-700/50" />
                <div className="h-5 w-3/4 rounded bg-gray-200 dark:bg-slate-700/50" />
                <div className="h-5 w-5/6 rounded bg-gray-200 dark:bg-slate-700/50" />
              </div>
              <div className="space-y-3">
                <div className="h-5 w-full rounded bg-gray-200 dark:bg-slate-700/50" />
                <div className="h-5 w-4/5 rounded bg-gray-200 dark:bg-slate-700/50" />
                <div className="h-5 w-3/4 rounded bg-gray-200 dark:bg-slate-700/50" />
              </div>
            </div>
            <div className="h-12 w-full rounded-lg bg-gray-200 dark:bg-slate-700/50" />
          </div>
        </div>
      ) : error ? (
        <ErrorMessage
          title="Unable to load country"
          message={error}
          onRetry={() => {
            if (code) {
              setIsLoading(true);
              setError(null);
              fetchCountryByCode(code)
                .then((data) => setCountry(data))
                .catch((err) => setError(getApiErrorMessage(err, 'Unable to load country details.')))
                .finally(() => setIsLoading(false));
            }
          }}
        />
      ) : !country ? (
        <ErrorMessage
          title="Country not found"
          message="We could not find the requested country details."
          onRetry={() => navigate('/')}
          retryLabel="Back to Home"
        />
      ) : (
        <div className="mt-8 grid grid-cols-1 items-center gap-12 lg:grid-cols-2 lg:gap-24">
          <div className="relative aspect-[16/10] w-full overflow-hidden rounded-xl bg-gray-100 shadow-xl dark:bg-slate-700/40 dark:shadow-2xl dark:shadow-black/50">
            {country.flag?.url_svg ? (
              <img
                src={country.flag.url_svg}
                alt={`Flag of ${commonName}`}
                className="h-full w-full object-cover"
              />
            ) : (
              <div className="flex h-full w-full items-center justify-center text-sm text-gray-400">
                No flag available
              </div>
            )}
          </div>

          <div className="flex flex-col">
            <h1 className="mb-6 text-3xl font-extrabold text-gray-900 sm:text-4xl dark:text-white">
              {commonName}
            </h1>

            <div className="grid grid-cols-1 gap-8 text-sm sm:grid-cols-2 sm:text-base mb-10">
              <div className="space-y-3">
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Native Name: </span>
                  <span className="text-gray-700 dark:text-gray-300">{nativeName}</span>
                </p>
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Population: </span>
                  <span className="text-gray-700 dark:text-gray-300">{populationText}</span>
                </p>
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Region: </span>
                  <span className="text-gray-700 dark:text-gray-300">{regionText}</span>
                </p>
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Sub Region: </span>
                  <span className="text-gray-700 dark:text-gray-300">{subregionText}</span>
                </p>
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Capital: </span>
                  <span className="text-gray-700 dark:text-gray-300">{capitalsText}</span>
                </p>
              </div>

              <div className="space-y-3">
                <p>
                  <span className="font-bold text-gray-900 dark:text-white">Currencies: </span>
                  <span className="text-gray-700 dark:text-gray-300">{currenciesText}</span>
                </p>
              </div>
            </div>

            <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
              <span className="font-bold text-gray-900 whitespace-nowrap dark:text-white">
                Border Countries:
              </span>
              <div className="flex flex-wrap items-center gap-2.5">
                {borders.length > 0 ? (
                  borders.map((border) => {
                    const borderName = typeof border === 'string' ? border : border.commonName;
                    const borderCode = typeof border === 'string' ? border : border.alpha3;
                    return (
                      <Link
                        key={borderCode}
                        to={`/country/${borderCode.toLowerCase()}`}
                        className="inline-flex min-w-20 items-center justify-center rounded-md bg-white px-5 py-1.5 text-xs font-semibold text-gray-800 shadow-sm transition-all hover:-translate-y-0.5 hover:shadow-md active:scale-95 dark:bg-[#2b3844] dark:text-gray-200 dark:hover:bg-slate-700/70 sm:text-sm"
                      >
                        {borderName}
                      </Link>
                    );
                  })
                ) : (
                  <span className="text-sm text-gray-500 dark:text-gray-400">None</span>
                )}
              </div>
            </div>
          </div>
        </div>
      )}
    </Layout>
  );
};

export default CountryDetailsPage;
