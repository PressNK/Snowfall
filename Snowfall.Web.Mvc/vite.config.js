import { defineConfig } from 'vite'
import { resolve } from 'path'

export default defineConfig({
    build: {
        manifest: true,
        rollupOptions: {
            input: resolve(__dirname, 'assets/js/app.js'),
        },
        outDir: resolve(__dirname, 'wwwroot/dist'),
    },
    resolve: {
        alias: {
        }
    },
})