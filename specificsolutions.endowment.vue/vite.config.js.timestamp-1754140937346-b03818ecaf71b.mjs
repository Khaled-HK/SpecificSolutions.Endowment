// vite.config.js
import { fileURLToPath } from "node:url";
import VueI18nPlugin from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/@intlify/unplugin-vue-i18n/lib/vite.mjs";
import vue from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import vueJsx from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/@vitejs/plugin-vue-jsx/dist/index.mjs";
import AutoImport from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/unplugin-auto-import/dist/vite.js";
import Components from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/unplugin-vue-components/dist/vite.js";
import { VueRouterAutoImports, getPascalCaseRouteName } from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/unplugin-vue-router/dist/index.mjs";
import VueRouter from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/unplugin-vue-router/dist/vite.mjs";
import { defineConfig } from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/vite/dist/node/index.js";
import VueDevTools from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/vite-plugin-vue-devtools/dist/vite.mjs";
import Layouts from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/vite-plugin-vue-layouts/dist/index.mjs";
import vuetify from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/vite-plugin-vuetify/dist/index.mjs";
import svgLoader from "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/node_modules/vite-svg-loader/index.js";
var __vite_injected_original_import_meta_url = "file:///C:/Users/Tajura/source/repos/SpecificSolutions.EndowmentOther/specificsolutions.endowment.vue/vite.config.js";
var vite_config_default = defineConfig({
  plugins: [
    // Docs: https://github.com/posva/unplugin-vue-router
    // ℹ️ This plugin should be placed before vue plugin
    VueRouter({
      getRouteName: (routeNode) => {
        return getPascalCaseRouteName(routeNode).replace(/([a-z\d])([A-Z])/g, "$1-$2").toLowerCase();
      },
      beforeWriteFiles: (root) => {
        root.insert("/apps/email/:filter", "/src/pages/apps/email/index.vue");
        root.insert("/apps/email/:label", "/src/pages/apps/email/index.vue");
      }
    }),
    vue({
      template: {
        compilerOptions: {
          isCustomElement: (tag) => tag === "swiper-container" || tag === "swiper-slide"
        }
      }
    }),
    VueDevTools(),
    vueJsx(),
    // Docs: https://github.com/vuetifyjs/vuetify-loader/tree/master/packages/vite-plugin
    vuetify({
      styles: {
        configFile: "src/assets/styles/variables/_vuetify.scss"
      }
    }),
    // Docs: https://github.com/johncampionjr/vite-plugin-vue-layouts#vite-plugin-vue-layouts
    Layouts({
      layoutsDirs: "./src/layouts/"
    }),
    // Docs: https://github.com/antfu/unplugin-vue-components#unplugin-vue-components
    Components({
      dirs: ["src/@core/components", "src/views/demos", "src/components"],
      dts: true,
      resolvers: [
        (componentName) => {
          if (componentName === "VueApexCharts")
            return { name: "default", from: "vue3-apexcharts", as: "VueApexCharts" };
        }
      ]
    }),
    // Docs: https://github.com/antfu/unplugin-auto-import#unplugin-auto-import
    AutoImport({
      imports: ["vue", VueRouterAutoImports, "@vueuse/core", "@vueuse/math", "vue-i18n", "pinia"],
      dirs: [
        "./src/@core/utils",
        "./src/@core/composable/",
        "./src/composables/",
        "./src/utils/",
        "./src/plugins/*/composables/*"
      ],
      vueTemplate: true,
      // ℹ️ Disabled to avoid confusion & accidental usage
      ignore: ["useCookies", "useStorage"],
      eslintrc: {
        enabled: true,
        filepath: "./.eslintrc-auto-import.json"
      }
    }),
    // Docs: https://github.com/intlify/bundle-tools/tree/main/packages/unplugin-vue-i18n#intlifyunplugin-vue-i18n
    VueI18nPlugin({
      runtimeOnly: true,
      compositionOnly: true,
      include: [
        fileURLToPath(new URL("./src/plugins/i18n/locales/**", __vite_injected_original_import_meta_url))
      ]
    }),
    svgLoader()
  ],
  define: { "process.env": {} },
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", __vite_injected_original_import_meta_url)),
      "@themeConfig": fileURLToPath(new URL("./themeConfig.js", __vite_injected_original_import_meta_url)),
      "@core": fileURLToPath(new URL("./src/@core", __vite_injected_original_import_meta_url)),
      "@layouts": fileURLToPath(new URL("./src/@layouts", __vite_injected_original_import_meta_url)),
      "@images": fileURLToPath(new URL("./src/assets/images/", __vite_injected_original_import_meta_url)),
      "@styles": fileURLToPath(new URL("./src/assets/styles/", __vite_injected_original_import_meta_url)),
      "@configured-variables": fileURLToPath(new URL("./src/assets/styles/variables/_template.scss", __vite_injected_original_import_meta_url)),
      "@db": fileURLToPath(new URL("./src/plugins/fake-api/handlers/", __vite_injected_original_import_meta_url)),
      "@api-utils": fileURLToPath(new URL("./src/plugins/fake-api/utils/", __vite_injected_original_import_meta_url))
    }
  },
  build: {
    chunkSizeWarningLimit: 5e3
  },
  optimizeDeps: {
    exclude: ["vuetify"],
    entries: [
      "./src/**/*.vue"
    ]
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcuanMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxUYWp1cmFcXFxcc291cmNlXFxcXHJlcG9zXFxcXFNwZWNpZmljU29sdXRpb25zLkVuZG93bWVudE90aGVyXFxcXHNwZWNpZmljc29sdXRpb25zLmVuZG93bWVudC52dWVcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIkM6XFxcXFVzZXJzXFxcXFRhanVyYVxcXFxzb3VyY2VcXFxccmVwb3NcXFxcU3BlY2lmaWNTb2x1dGlvbnMuRW5kb3dtZW50T3RoZXJcXFxcc3BlY2lmaWNzb2x1dGlvbnMuZW5kb3dtZW50LnZ1ZVxcXFx2aXRlLmNvbmZpZy5qc1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vQzovVXNlcnMvVGFqdXJhL3NvdXJjZS9yZXBvcy9TcGVjaWZpY1NvbHV0aW9ucy5FbmRvd21lbnRPdGhlci9zcGVjaWZpY3NvbHV0aW9ucy5lbmRvd21lbnQudnVlL3ZpdGUuY29uZmlnLmpzXCI7aW1wb3J0IHsgZmlsZVVSTFRvUGF0aCB9IGZyb20gJ25vZGU6dXJsJ1xyXG5pbXBvcnQgVnVlSTE4blBsdWdpbiBmcm9tICdAaW50bGlmeS91bnBsdWdpbi12dWUtaTE4bi92aXRlJ1xyXG5pbXBvcnQgdnVlIGZyb20gJ0B2aXRlanMvcGx1Z2luLXZ1ZSdcclxuaW1wb3J0IHZ1ZUpzeCBmcm9tICdAdml0ZWpzL3BsdWdpbi12dWUtanN4J1xyXG5pbXBvcnQgQXV0b0ltcG9ydCBmcm9tICd1bnBsdWdpbi1hdXRvLWltcG9ydC92aXRlJ1xyXG5pbXBvcnQgQ29tcG9uZW50cyBmcm9tICd1bnBsdWdpbi12dWUtY29tcG9uZW50cy92aXRlJ1xyXG5pbXBvcnQgeyBWdWVSb3V0ZXJBdXRvSW1wb3J0cywgZ2V0UGFzY2FsQ2FzZVJvdXRlTmFtZSB9IGZyb20gJ3VucGx1Z2luLXZ1ZS1yb3V0ZXInXHJcbmltcG9ydCBWdWVSb3V0ZXIgZnJvbSAndW5wbHVnaW4tdnVlLXJvdXRlci92aXRlJ1xyXG5pbXBvcnQgeyBkZWZpbmVDb25maWcgfSBmcm9tICd2aXRlJ1xyXG5pbXBvcnQgVnVlRGV2VG9vbHMgZnJvbSAndml0ZS1wbHVnaW4tdnVlLWRldnRvb2xzJ1xyXG5pbXBvcnQgTGF5b3V0cyBmcm9tICd2aXRlLXBsdWdpbi12dWUtbGF5b3V0cydcclxuaW1wb3J0IHZ1ZXRpZnkgZnJvbSAndml0ZS1wbHVnaW4tdnVldGlmeSdcclxuaW1wb3J0IHN2Z0xvYWRlciBmcm9tICd2aXRlLXN2Zy1sb2FkZXInXHJcblxyXG4vLyBodHRwczovL3ZpdGVqcy5kZXYvY29uZmlnL1xyXG5leHBvcnQgZGVmYXVsdCBkZWZpbmVDb25maWcoe1xyXG4gIHBsdWdpbnM6IFtcclxuICAgIC8vIERvY3M6IGh0dHBzOi8vZ2l0aHViLmNvbS9wb3N2YS91bnBsdWdpbi12dWUtcm91dGVyXHJcbiAgICAvLyBcdTIxMzlcdUZFMEYgVGhpcyBwbHVnaW4gc2hvdWxkIGJlIHBsYWNlZCBiZWZvcmUgdnVlIHBsdWdpblxyXG4gICAgVnVlUm91dGVyKHtcclxuICAgICAgZ2V0Um91dGVOYW1lOiByb3V0ZU5vZGUgPT4ge1xyXG4gICAgICAgIC8vIENvbnZlcnQgcGFzY2FsIGNhc2UgdG8ga2ViYWIgY2FzZVxyXG4gICAgICAgIHJldHVybiBnZXRQYXNjYWxDYXNlUm91dGVOYW1lKHJvdXRlTm9kZSlcclxuICAgICAgICAgIC5yZXBsYWNlKC8oW2EtelxcZF0pKFtBLVpdKS9nLCAnJDEtJDInKVxyXG4gICAgICAgICAgLnRvTG93ZXJDYXNlKClcclxuICAgICAgfSxcclxuICAgICAgYmVmb3JlV3JpdGVGaWxlczogcm9vdCA9PiB7XHJcbiAgICAgICAgcm9vdC5pbnNlcnQoJy9hcHBzL2VtYWlsLzpmaWx0ZXInLCAnL3NyYy9wYWdlcy9hcHBzL2VtYWlsL2luZGV4LnZ1ZScpXHJcbiAgICAgICAgcm9vdC5pbnNlcnQoJy9hcHBzL2VtYWlsLzpsYWJlbCcsICcvc3JjL3BhZ2VzL2FwcHMvZW1haWwvaW5kZXgudnVlJylcclxuICAgICAgfSxcclxuICAgIH0pLFxyXG4gICAgdnVlKHtcclxuICAgICAgdGVtcGxhdGU6IHtcclxuICAgICAgICBjb21waWxlck9wdGlvbnM6IHtcclxuICAgICAgICAgIGlzQ3VzdG9tRWxlbWVudDogdGFnID0+IHRhZyA9PT0gJ3N3aXBlci1jb250YWluZXInIHx8IHRhZyA9PT0gJ3N3aXBlci1zbGlkZScsXHJcbiAgICAgICAgfSxcclxuICAgICAgfSxcclxuICAgIH0pLFxyXG4gICAgVnVlRGV2VG9vbHMoKSxcclxuICAgIHZ1ZUpzeCgpLFxyXG5cclxuICAgIC8vIERvY3M6IGh0dHBzOi8vZ2l0aHViLmNvbS92dWV0aWZ5anMvdnVldGlmeS1sb2FkZXIvdHJlZS9tYXN0ZXIvcGFja2FnZXMvdml0ZS1wbHVnaW5cclxuICAgIHZ1ZXRpZnkoe1xyXG4gICAgICBzdHlsZXM6IHtcclxuICAgICAgICBjb25maWdGaWxlOiAnc3JjL2Fzc2V0cy9zdHlsZXMvdmFyaWFibGVzL192dWV0aWZ5LnNjc3MnLFxyXG4gICAgICB9LFxyXG4gICAgfSksXHJcblxyXG4gICAgLy8gRG9jczogaHR0cHM6Ly9naXRodWIuY29tL2pvaG5jYW1waW9uanIvdml0ZS1wbHVnaW4tdnVlLWxheW91dHMjdml0ZS1wbHVnaW4tdnVlLWxheW91dHNcclxuICAgIExheW91dHMoe1xyXG4gICAgICBsYXlvdXRzRGlyczogJy4vc3JjL2xheW91dHMvJyxcclxuICAgIH0pLFxyXG5cclxuICAgIC8vIERvY3M6IGh0dHBzOi8vZ2l0aHViLmNvbS9hbnRmdS91bnBsdWdpbi12dWUtY29tcG9uZW50cyN1bnBsdWdpbi12dWUtY29tcG9uZW50c1xyXG4gICAgQ29tcG9uZW50cyh7XHJcbiAgICAgIGRpcnM6IFsnc3JjL0Bjb3JlL2NvbXBvbmVudHMnLCAnc3JjL3ZpZXdzL2RlbW9zJywgJ3NyYy9jb21wb25lbnRzJ10sXHJcbiAgICAgIGR0czogdHJ1ZSxcclxuICAgICAgcmVzb2x2ZXJzOiBbXHJcbiAgICAgICAgY29tcG9uZW50TmFtZSA9PiB7XHJcbiAgICAgICAgICAvLyBBdXRvIGltcG9ydCBgVnVlQXBleENoYXJ0c2BcclxuICAgICAgICAgIGlmIChjb21wb25lbnROYW1lID09PSAnVnVlQXBleENoYXJ0cycpXHJcbiAgICAgICAgICAgIHJldHVybiB7IG5hbWU6ICdkZWZhdWx0JywgZnJvbTogJ3Z1ZTMtYXBleGNoYXJ0cycsIGFzOiAnVnVlQXBleENoYXJ0cycgfVxyXG4gICAgICAgIH0sXHJcbiAgICAgIF0sXHJcbiAgICB9KSxcclxuXHJcbiAgICAvLyBEb2NzOiBodHRwczovL2dpdGh1Yi5jb20vYW50ZnUvdW5wbHVnaW4tYXV0by1pbXBvcnQjdW5wbHVnaW4tYXV0by1pbXBvcnRcclxuICAgIEF1dG9JbXBvcnQoe1xyXG4gICAgICBpbXBvcnRzOiBbJ3Z1ZScsIFZ1ZVJvdXRlckF1dG9JbXBvcnRzLCAnQHZ1ZXVzZS9jb3JlJywgJ0B2dWV1c2UvbWF0aCcsICd2dWUtaTE4bicsICdwaW5pYSddLFxyXG4gICAgICBkaXJzOiBbXHJcbiAgICAgICAgJy4vc3JjL0Bjb3JlL3V0aWxzJyxcclxuICAgICAgICAnLi9zcmMvQGNvcmUvY29tcG9zYWJsZS8nLFxyXG4gICAgICAgICcuL3NyYy9jb21wb3NhYmxlcy8nLFxyXG4gICAgICAgICcuL3NyYy91dGlscy8nLFxyXG4gICAgICAgICcuL3NyYy9wbHVnaW5zLyovY29tcG9zYWJsZXMvKicsXHJcbiAgICAgIF0sXHJcbiAgICAgIHZ1ZVRlbXBsYXRlOiB0cnVlLFxyXG5cclxuICAgICAgLy8gXHUyMTM5XHVGRTBGIERpc2FibGVkIHRvIGF2b2lkIGNvbmZ1c2lvbiAmIGFjY2lkZW50YWwgdXNhZ2VcclxuICAgICAgaWdub3JlOiBbJ3VzZUNvb2tpZXMnLCAndXNlU3RvcmFnZSddLFxyXG4gICAgICBlc2xpbnRyYzoge1xyXG4gICAgICAgIGVuYWJsZWQ6IHRydWUsXHJcbiAgICAgICAgZmlsZXBhdGg6ICcuLy5lc2xpbnRyYy1hdXRvLWltcG9ydC5qc29uJyxcclxuICAgICAgfSxcclxuICAgIH0pLFxyXG5cclxuICAgIC8vIERvY3M6IGh0dHBzOi8vZ2l0aHViLmNvbS9pbnRsaWZ5L2J1bmRsZS10b29scy90cmVlL21haW4vcGFja2FnZXMvdW5wbHVnaW4tdnVlLWkxOG4jaW50bGlmeXVucGx1Z2luLXZ1ZS1pMThuXHJcbiAgICBWdWVJMThuUGx1Z2luKHtcclxuICAgICAgcnVudGltZU9ubHk6IHRydWUsXHJcbiAgICAgIGNvbXBvc2l0aW9uT25seTogdHJ1ZSxcclxuICAgICAgaW5jbHVkZTogW1xyXG4gICAgICAgIGZpbGVVUkxUb1BhdGgobmV3IFVSTCgnLi9zcmMvcGx1Z2lucy9pMThuL2xvY2FsZXMvKionLCBpbXBvcnQubWV0YS51cmwpKSxcclxuICAgICAgXSxcclxuICAgIH0pLFxyXG4gICAgc3ZnTG9hZGVyKCksXHJcbiAgXSxcclxuICBkZWZpbmU6IHsgJ3Byb2Nlc3MuZW52Jzoge30gfSxcclxuICByZXNvbHZlOiB7XHJcbiAgICBhbGlhczoge1xyXG4gICAgICAnQCc6IGZpbGVVUkxUb1BhdGgobmV3IFVSTCgnLi9zcmMnLCBpbXBvcnQubWV0YS51cmwpKSxcclxuICAgICAgJ0B0aGVtZUNvbmZpZyc6IGZpbGVVUkxUb1BhdGgobmV3IFVSTCgnLi90aGVtZUNvbmZpZy5qcycsIGltcG9ydC5tZXRhLnVybCkpLFxyXG4gICAgICAnQGNvcmUnOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoJy4vc3JjL0Bjb3JlJywgaW1wb3J0Lm1ldGEudXJsKSksXHJcbiAgICAgICdAbGF5b3V0cyc6IGZpbGVVUkxUb1BhdGgobmV3IFVSTCgnLi9zcmMvQGxheW91dHMnLCBpbXBvcnQubWV0YS51cmwpKSxcclxuICAgICAgJ0BpbWFnZXMnOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoJy4vc3JjL2Fzc2V0cy9pbWFnZXMvJywgaW1wb3J0Lm1ldGEudXJsKSksXHJcbiAgICAgICdAc3R5bGVzJzogZmlsZVVSTFRvUGF0aChuZXcgVVJMKCcuL3NyYy9hc3NldHMvc3R5bGVzLycsIGltcG9ydC5tZXRhLnVybCkpLFxyXG4gICAgICAnQGNvbmZpZ3VyZWQtdmFyaWFibGVzJzogZmlsZVVSTFRvUGF0aChuZXcgVVJMKCcuL3NyYy9hc3NldHMvc3R5bGVzL3ZhcmlhYmxlcy9fdGVtcGxhdGUuc2NzcycsIGltcG9ydC5tZXRhLnVybCkpLFxyXG4gICAgICAnQGRiJzogZmlsZVVSTFRvUGF0aChuZXcgVVJMKCcuL3NyYy9wbHVnaW5zL2Zha2UtYXBpL2hhbmRsZXJzLycsIGltcG9ydC5tZXRhLnVybCkpLFxyXG4gICAgICAnQGFwaS11dGlscyc6IGZpbGVVUkxUb1BhdGgobmV3IFVSTCgnLi9zcmMvcGx1Z2lucy9mYWtlLWFwaS91dGlscy8nLCBpbXBvcnQubWV0YS51cmwpKSxcclxuICAgIH0sXHJcbiAgfSxcclxuICBidWlsZDoge1xyXG4gICAgY2h1bmtTaXplV2FybmluZ0xpbWl0OiA1MDAwLFxyXG4gIH0sXHJcbiAgb3B0aW1pemVEZXBzOiB7XHJcbiAgICBleGNsdWRlOiBbJ3Z1ZXRpZnknXSxcclxuICAgIGVudHJpZXM6IFtcclxuICAgICAgJy4vc3JjLyoqLyoudnVlJyxcclxuICAgIF0sXHJcbiAgfSxcclxufSlcclxuIl0sCiAgIm1hcHBpbmdzIjogIjtBQUF1ZCxTQUFTLHFCQUFxQjtBQUNyZixPQUFPLG1CQUFtQjtBQUMxQixPQUFPLFNBQVM7QUFDaEIsT0FBTyxZQUFZO0FBQ25CLE9BQU8sZ0JBQWdCO0FBQ3ZCLE9BQU8sZ0JBQWdCO0FBQ3ZCLFNBQVMsc0JBQXNCLDhCQUE4QjtBQUM3RCxPQUFPLGVBQWU7QUFDdEIsU0FBUyxvQkFBb0I7QUFDN0IsT0FBTyxpQkFBaUI7QUFDeEIsT0FBTyxhQUFhO0FBQ3BCLE9BQU8sYUFBYTtBQUNwQixPQUFPLGVBQWU7QUFaeVIsSUFB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
