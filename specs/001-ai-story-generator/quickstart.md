# Quickstart — AI Story Generator (local)

## Overview

The AI Story Generator is a web application that enables users to create custom stories based on their preferences. The application consists of a .NET 10.0 backend API and a React frontend.

## Architecture

- **Backend**: ASP.NET Core Web API (`.NET 10.0`)
  - Handles story generation requests
  - Orchestrates AI provider calls
  - Manages job tracking and session state
  
- **Frontend**: React + Vite
  - Left panel: Story preference form
  - Right panel: Generated story viewer
  - Export controls for copy/download

- **Storage**: Session-only in-memory (v1)
  - No persistent user accounts
  - Stories exist only during the session

## Prerequisites

- .NET 10 SDK installed
- Node.js 18+ installed
- npm or yarn package manager

## Quick Start

### 1. Backend Setup

```powershell
cd backend/src/AIStoryGenerator.Api

# Restore dependencies
dotnet restore

# Run the API (defaults to http://localhost:5000)
dotnet run
```

The API will start on `http://localhost:5000` and be available at:
- POST `/api/v1/generate` - Generate a story
- GET `/api/v1/generate/{jobId}` - Check job status
- GET `/api/v1/export/{jobId}/download` - Download story

### 2. Frontend Setup

In a new terminal:

```bash
cd frontend

# Install dependencies
npm install

# Start development server (defaults to http://localhost:5173)
npm run dev
```

The frontend will be available at `http://localhost:5173`

### 3. Open Application

Navigate to `http://localhost:5173` in your browser. You should see:
1. **Left panel**: Form to enter story preferences
2. **Right panel**: Generated story viewer

## Usage

1. Fill out the story preferences form:
   - **Plot**: Main story plot (required)
   - **Characters**: Main characters (required)
   - **Setting**: Story setting (required)
   - **Format**: Choose Prose, Screenplay, Stageplay, or Poem
   - **Length**: Adjust story length with slider (50-5000 characters)
   - **Genre**: Story genre (e.g., Fantasy, Romance, Mystery)
   - **Theme**: Story theme (e.g., Redemption, Adventure)
   - **Point of View**: Select narrative perspective
   - **Dialogue Balance**: Adjust dialogue-to-narration ratio
   - **Additional Context**: Any extra details

2. Click **"Generate Story"**

3. Wait for the story to be generated (typically 1-2 seconds with mock provider)

4. Use export controls:
   - **📋 Copy Story**: Copy full story to clipboard
   - **⬇️ Download**: Download as `.txt` file
   - **ℹ️ Copy Info**: Copy story metadata

5. Modify preferences and click **"Generate Story"** again to create variations

## Environment Configuration

### Development

Create `.env` file in project root (optional for local testing with mock provider):

```
OPENAI_API_KEY=your_key_here
ANTHROPIC_API_KEY=your_key_here
```

### API Configuration

Backend settings in `backend/src/AIStoryGenerator.Api/appsettings.json`:

```json
{
  "StoryGeneration": {
    "DefaultProvider": "LocalMock",
    "RequestTimeoutSeconds": 30,
    "MaxConcurrentRequests": 10
  }
}
```

## Running Tests

### Backend Unit Tests

```powershell
cd backend
dotnet test
```

### Frontend Tests

```bash
cd frontend

# Unit tests with Vitest
npm run test

# Accessibility tests with Playwright
npm run test:a11y

# Headed mode (see browser)
npm run test:a11y:headed
```

## Project Structure

```
backend/
├── src/
│   └── AIStoryGenerator.Api/
│       ├── Controllers/       # API endpoints
│       ├── Services/          # Business logic
│       ├── Models/            # Data models
│       ├── Validators/        # Input validation
│       ├── Middleware/        # HTTP middleware
│       └── Program.cs         # Configuration
└── tests/
    └── Contract/              # API contract tests

frontend/
├── src/
│   ├── components/            # React components
│   ├── services/              # API client
│   ├── utils/                 # Utilities
│   ├── styles/                # CSS files
│   └── App.jsx                # Main app
└── tests/
    ├── integration/           # E2E tests
    └── unit/                  # Unit tests
```

## Performance Targets

- **Generation time**: Median < 8s, p95 < 20s
- **Frontend bundle**: ≤ 500KB gzipped
- **Memory per request**: < 500MB

## Features

### Phase 1: MVP (Complete ✓)
- [x] Generate stories with custom preferences
- [x] Multiple format support (Prose, Screenplay, Stageplay, Poem)
- [x] Copy and download functionality
- [x] WCAG AA accessibility compliance
- [x] Responsive design

### Phase 2: Enhancements (Planned)
- [ ] User accounts and story history
- [ ] Multiple AI provider support
- [ ] Advanced formatting options
- [ ] Social sharing

## Troubleshooting

**Frontend can't reach backend:**
- Verify backend is running on port 5000
- Check CORS is enabled (should be in development mode)
- Check browser console for network errors

**API returns 500 error:**
- Check backend logs for detailed error messages
- Verify environment variables are set if using real AI providers

**Stories seem repetitive (LocalMock provider):**
- This is expected with the mock provider
- Switch to real AI provider (OpenAI, Anthropic) for varied output

## Next Steps

1. **Configure AI Provider**: Update `appsettings.json` with real provider API key
2. **Deploy**: See deployment guide for cloud deployment options
3. **Scale**: Implement persistent storage and user authentication for production

## Support

For issues or questions:
1. Check the specification in `specs/001-ai-story-generator/spec.md`
2. Review the implementation plan in `specs/001-ai-story-generator/plan.md`
3. See the contract documentation in `specs/001-ai-story-generator/contracts/generation-api.md`

