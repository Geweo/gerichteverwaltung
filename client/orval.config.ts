import { defineConfig } from 'orval';

export default defineConfig({
  api: {
    input: {
      target: process.env.VITE_API_URL
        ? `${process.env.VITE_API_URL}/openapi/v1.json`
        : 'http://localhost:5000/openapi/v1.json',
    },
    output: {
      mode: 'tags-split',
      target: './src/generated/api',
      schemas: './src/generated/api/schemas',
      client: 'react-query',
      httpClient: 'axios',
      mock: false,
      override: {
        query: {
          useQuery: true,
          useInfinite: true,
          useInfiniteQueryParam: 'page',
          queryOptions: {
            path: './src/lib/query-options.ts',
            name: 'useQueryOptions',
          },
        },
      },
    },
    hooks: {
      afterAllFilesWrite: 'prettier --write',
    },
  },
});
