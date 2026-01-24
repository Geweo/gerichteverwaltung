import { defineConfig } from 'orval';

export default defineConfig({
  api: {
    input: {
      target: process.env.VITE_API_URL
        ? `${process.env.VITE_API_URL}/swagger/v1/swagger.json`
        : 'http://localhost:5000/swagger/v1/swagger.json',
    },
    output: {
      mode: 'tags-split',
      target: './src/generated/api',
      schemas: './src/generated/api/schemas',
      client: 'react-query',
      mock: false,
      override: {
        mutator: {
          path: './src/lib/api-client.ts',
          name: 'customInstance',
        },
        query: {
          useQuery: true,
          useInfinite: true,
          useInfiniteQueryParam: 'page',
          options: {
            staleTime: 1000 * 60 * 5, // 5 minutes
          },
        },
      },
    },
    hooks: {
      afterAllFilesWrite: 'prettier --write',
    },
  },
});

