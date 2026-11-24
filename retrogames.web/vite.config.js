import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Front (5173) e API (5080) na mesma origem: o Vite encaminha /api para a API.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: { '/api': 'http://localhost:5080' },
  },
})
