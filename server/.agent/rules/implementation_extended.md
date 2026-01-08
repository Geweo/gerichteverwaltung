# Extended Implementation Rules

## External Services

- Supabase: Used for Auth (JWT validation) and Storage (file uploads)
- Bring.com: Used for exporting shopping lists
- OpenAI GPT: Used for OCR and recipe suggestions

## Authentication

- Supabase JWT tokens are validated in middleware
- `sub` from token is mapped to own `User` entity in PostgreSQL
- Backend validates against Supabase JWKS

## File Storage

- Use Supabase Storage for images and PDFs
- Implement file storage through port interface (not directly in use cases)

