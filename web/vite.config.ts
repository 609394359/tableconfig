import { defineConfig, loadEnv } from 'vite'
import type { ConfigEnv,UserConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path';


const viteConfig = defineConfig((mode: ConfigEnv): UserConfig => {
	const env = loadEnv(mode.mode, process.cwd());

	return {
		plugins: [vue()],		
    root: process.cwd(),
    resolve: {
      alias: {
          '@': resolve(__dirname, 'src'),
        }
    },
		base: mode.command === 'serve' ? './' : env.VITE_PUBLIC_PATH,
		optimizeDeps: { exclude: ['vue-demi'] },
		server: {
			host: '0.0.0.0',
			port: env.VITE_PORT as unknown as number,
			open: JSON.parse(env.VITE_OPEN),
			hmr: true,
			proxy: {
				[env.VITE_APP_BASE_API]: {
					target: env.VITE_APP_TARGET_URL,
					ws: true,
					changeOrigin: true,
					rewrite: (path) =>
						path.replace(
							new RegExp("^" + env.VITE_APP_BASE_API),
							env.VITE_APP_TARGET_BASE_API
						),
				},
			},
		},
		css: { preprocessorOptions: { scss: { charset: false } } },
		define: {
			__VUE_I18N_LEGACY_API__: JSON.stringify(false),
			__VUE_I18N_FULL_INSTALL__: JSON.stringify(false),
			__INTLIFY_PROD_DEVTOOLS__: JSON.stringify(false),
			__NEXT_VERSION__: JSON.stringify(process.env.npm_package_version),
			__NEXT_NAME__: JSON.stringify(process.env.npm_package_name),
		},
	};
});

export default viteConfig;
