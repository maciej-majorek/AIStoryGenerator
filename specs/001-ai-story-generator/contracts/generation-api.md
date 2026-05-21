# Generation API Contract

## POST /api/v1/generate

Request JSON:

```json
{
  "plot": "string",
  "characters": "string",
  "setting": "string",
  "format": "Prose|Screenplay|Stageplay|Poem",
  "length": 100,
  "genre": "string",
  "theme": "string",
  "pointOfView": "string",
  "dialogueBalance": "string",
  "additionalContext": "string"
}
```

Response 200 JSON:

```json
{
  "jobId": "uuid",
  "story": {
    "storyId": "uuid",
    "content": "string",
    "format": "string",
    "length": 123,
    "tokensUsed": 456,
    "safetyFlags": []
  }
}
```

Errors:
- 400: Invalid input (validation errors with field-level messages)
- 413: Request too large (length/size limits)
- 429: Rate limit
- 500: Internal error (provider error)

Notes:
- The API is synchronous for v1; server may accept the request, call AI provider, and return generated content; if asynchronous queueing is implemented, return `jobId` immediately and use a GET `/api/v1/generate/{jobId}` to fetch results.
