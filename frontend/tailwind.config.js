/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#0D1117',
        'secondary': '#161B22',
        'accent': '#2185E5',
        'highlight': '#FF9900',
        'text-primary': '#FFFFFF',
        'text-secondary': '#AAAAAA',
        'dropdown': '#30363D',
      }
    },
  },
  plugins: [],
}
