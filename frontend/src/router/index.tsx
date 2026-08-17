import { createBrowserRouter } from "react-router";
import ListCountriesPage from "../pages/list-countries/ListCountriesPage";
import CountryDetailsPage from "../pages/country-details/CountryDetailsPage";
import NotFoundPage from "../pages/not-found/NotFoundPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <ListCountriesPage />,
  },
  {
    path: "/country/:code",
    element: <CountryDetailsPage />,
  },
  {
    path: "*",
    element: <NotFoundPage />,
  },
]);

export default router;
