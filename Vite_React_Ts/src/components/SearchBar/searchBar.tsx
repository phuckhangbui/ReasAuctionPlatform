import React, { useRef } from "react";

interface SearchBarProp {
  placeHolder: string;
  inputName: string;
  nameValue: string;
  onSearchChange: (value: string) => void;
}

const SearchBar = ({
  placeHolder,
  inputName,
  nameValue,
  onSearchChange,
}: SearchBarProp) => {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    onSearchChange(value);
  };

  return (
    <div>
      <label
        htmlFor="default-search"
        className="mb-2 text-sm font-medium text-gray-900 sr-only "
      >
        Search
      </label>
      <div className="relative">
        <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg
            className="w-4 h-4 text-gray-500 dark:text-gray-400"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 20 20"
          >
            <path
              stroke="currentColor"
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
            />
          </svg>
        </div>
        <input
          type="search"
          id="default-search"
          className="block sm:w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-3xl bg-gray-50 focus:ring-mainBlue focus:border-mainBlue focus:outline-none"
          placeholder={placeHolder}
          value={nameValue}
          name={inputName}
          onChange={handleChange}
        />
      </div>
    </div>
  );
};

export default SearchBar;
