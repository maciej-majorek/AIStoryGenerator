# Quickstart — AI Story Generator (local)

## Prerequisites
- .NET 8+/10 SDK installed
- Node.js 16+
- Yarn or npm

## Backend (development)

```powershell
cd backend

dotnet restore

dotnet run --project src/AIStoryGenerator.Api
```

## Frontend (development)

```bash
cd frontend
npm install
npm start
```

## Run tests

```powershell
# Backend unit tests
cd backend

dotnet test

# Frontend tests
cd frontend
npm test
```

## Notes
- Environment variables for AI provider keys must be set in CI and local `.env` for development.
- The system runs session-only by default; generated stories are not persisted in v1.
