# Data Model

## StoryRequest
- `id`: uuid
- `plot`: string
- `characters`: string
- `setting`: string
- `format`: enum {Prose, Screenplay, Stageplay, Poem}
- `length`: integer (50-5000)
- `genre`: string
- `theme`: string
- `pointOfView`: string
- `dialogueBalance`: string
- `additionalContext`: string
- `createdAt`: timestamp

## GenerationJob
- `jobId`: uuid
- `requestId`: uuid (StoryRequest.id)
- `status`: enum {queued, running, succeeded, failed}
- `startedAt`: timestamp
- `completedAt`: timestamp
- `error`: nullable string

## GeneratedStory
- `storyId`: uuid
- `jobId`: uuid
- `content`: text
- `format`: enum
- `length`: integer (characters)
- `tokensUsed`: integer (if provider reports)
- `safetyFlags`: array
- `createdAt`: timestamp

## UserPreferences (optional future)
- `userId`: uuid
- `savedPreferences`: JSON blob
