const withMT = require("@material-tailwind/react/utils/withMT");
const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = withMT({
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
    "./node_modules/@material-tailwind/react/components/**/*.{js,ts,jsx,tsx}",
    "./node_modules/@material-tailwind/react/theme/components/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    screens: {
      sm: "350px",
    },
    extend: {
      colors: {
        mainBlue: "#2482c2",
        darkerMainBlue: "#2075ae",
        secondaryYellow: "#fda720",
      },
      spacing: {
        17: "4.25rem",
        86: "21.5rem",
        30: "7.5rem",
        69: "17.25rem",
        120: "30rem",
        "75vh": "75vh",
      },
    },
  },
  plugins: [require("@tailwindcss/line-clamp")],
});
