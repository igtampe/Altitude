import { createTheme } from '@mui/material/styles';

//TODO: These are the Neco light and dark themes
//please replace these later.

export const lightTheme = createTheme({
    palette: {
      mode: 'light',
      primary: { main: '#F4B770', },
      secondary: { main: '#94BAE0', },
    },
  });
  
  export const darkTheme = createTheme({
    palette: {
      mode: 'dark',
      primary: { main: '#C26C25', },
      secondary: { main: '#6088AF', },
    },
  })