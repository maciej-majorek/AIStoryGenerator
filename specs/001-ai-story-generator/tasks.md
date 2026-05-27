---
description: "Task list for AI Story Generator feature"
---

# Tasks: AI Story Generator

**Input**: Design documents from `/specs/001-ai-story-generator/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [X] T001 Create backend .NET 10.0 project and solution at `backend/src/AIStoryGenerator.Api` and `backend/AIStoryGenerator.sln`
- [X] T002 [P] Create frontend React project at `frontend/` using approved toolchain (Vite or CRA) and initialize `frontend/src/`
- [X] T003 [P] Configure linting and formatting: add `.editorconfig`, `backend/.editorconfig`, `frontend/.eslintrc.js`, `.prettierrc` and enable `dotnet format` and `eslint` in CI
- [X] T004 [P] Add CI pipeline `.github/workflows/ci.yml` to run build, lint, tests, and bundle-size checks
- [X] T005 [P] Add performance and accessibility test harnesses: `tests/perf/` and `frontend/tests/accessibility/` with runners and baseline scripts

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

- [X] T006 [P] Add API controller scaffold `backend/src/Controllers/GenerationController.cs` implementing POST `/api/v1/generate`
- [X] T007 [P] Create models: `backend/src/Models/StoryRequest.cs`, `backend/src/Models/GenerationJob.cs`, `backend/src/Models/GeneratedStory.cs`
- [X] T008 [P] Implement `StoryGeneratorService` in `backend/src/Services/StoryGeneratorService.cs` (provider orchestration)
- [X] T009 [P] Add provider abstraction `backend/src/Services/IStoryProvider.cs` and a placeholder provider `backend/src/Services/LocalMockStoryProvider.cs`
- [X] T010 [P] Implement safety & moderation middleware `backend/src/Middleware/SafetyMiddleware.cs`
- [X] T011 [P] Implement in-memory job store `backend/src/Services/JobStore/InMemoryJobStore.cs` for session-only jobs
- [X] T012 [P] Configure app settings and secrets: `backend/appsettings.json` and docs for AI provider keys in `.env.example`
- [X] T013 [P] Add error handling and structured logging: `backend/src/Infrastructure/ErrorHandlingMiddleware.cs` + logging config in `backend/appsettings.json`
- [X] T014 [P] Add request validation for `StoryRequest` (`backend/src/Validators/StoryRequestValidator.cs`)

**Checkpoint**: Foundation ready — user story work can begin

---

## Phase 3: User Story 1 - Generate Story (Priority: P1) 🎯 MVP

**Goal**: Allow a user to submit preferences and receive a generated story that respects format, length, genre, characters, and setting.

**Independent Test**: Submit a sample `StoryRequest` and verify API returns a story with correct `format` and acceptable length; end-to-end UI test verifies story appears in right-side editor.

### Tests (must be written first)

- [X] T015 [P] [US1] Contract test for POST `/api/v1/generate` in `backend/tests/Contract/GenerationContractTests.cs`
- [X] T016 [P] [US1] Integration/end-to-end test for full UI flow in `frontend/tests/integration/generate_story.spec.ts`

### Implementation

- [X] T017 [US1] Implement controller action in `backend/src/Controllers/GenerationController.cs` (depends on T006, T007, T008)
- [X] T018 [US1] Implement orchestration in `backend/src/Services/StoryGeneratorService.cs` to call provider and return `GeneratedStory` (depends on T008, T009)
- [X] T019 [P] [US1] Create frontend form component `frontend/src/components/StoryForm.jsx` (or `.tsx`) to collect preferences (depends on T002)
- [X] T020 [P] [US1] Create frontend result viewer `frontend/src/components/StoryViewer.jsx` to display generated story
- [X] T021 [P] [US1] Implement frontend API client `frontend/src/services/api.ts` to call `/api/v1/generate`
- [X] T022 [US1] Implement loading, error, and retry UI behavior in `frontend/src/components/GenerateButton.jsx` and `StoryForm.jsx`
- [X] T023 [US1] Implement basic formatting helpers `frontend/src/utils/formatting.ts` to render Prose, Screenplay, Stageplay, Poem

**Checkpoint**: US1 should be independently testable and demoable

---

## Phase 4: User Story 2 - Adjust & Regenerate (Priority: P2)

**Goal**: Allow iterative edits to preferences and regenerate new story versions.

**Independent Test**: Change one preference and verify regenerated story reflects the change via integration test.

- [X] T024 [P] [US2] Integration test for regenerate flow in `frontend/tests/integration/regenerate.spec.ts`
- [X] T025 [P] [US2] Preserve and restore form state and allow repeated generations in `frontend/src/components/StoryForm.jsx`
- [X] T026 [P] [US2] Implement optional idempotency/variant metadata support in `backend/src/Services/GenerationVariantService.cs`
- [X] T027 [P] [US2] Instrument telemetry for regeneration events in `backend/src/Services/TelemetryService.cs`

---

## Phase 5: User Story 3 - Export & Copy (Priority: P3)

**Goal**: Allow users to copy, download, or export generated stories.

**Independent Test**: Verify copy/download actions transfer exact content to clipboard/file.

- [X] T028 [P] [US3] Add copy-to-clipboard and download controls in `frontend/src/components/ExportControls.jsx`
- [X] T029 [P] [US3] Unit tests for export and clipboard functionality in `frontend/tests/unit/export.spec.ts`
- [X] T030 [US3] Add `Download as .txt` server-side export support (if needed) in `backend/src/Controllers/ExportController.cs`

---

## Phase N: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [X] T031 [P] Update documentation and quickstart: `specs/001-ai-story-generator/quickstart.md`, `README.md`
- [X] T032 [P] Accessibility and ARIA updates across `frontend/src/components/` (WCAG AA)
- [ ] T033 [P] Bundle size optimization and enforcement: `frontend/package.json` budget + CI checks
- [X] T034 [P] Add end-to-end smoke tests and add to CI: `frontend/tests/e2e/smoke.spec.ts`
- [X] T035 [P] Security & safety hardening: ensure moderation checks run in `SafetyMiddleware` and are tested

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies — start immediately
- **Foundational (Phase 2)**: Depends on Setup completion — blocks user stories
- **User Stories (Phase 3+)**: Depend on Foundational completion
- **Polish (Final Phase)**: Depends on user stories completion

### User Story Dependencies

- **User Story 1 (P1)**: Depends on foundational tasks T006-T014
- **User Story 2 (P2)**: Depends on US1 implementation (T017-T023)
- **User Story 3 (P3)**: Depends on US1 for story content generation and frontend components

## Parallel Opportunities

- Setup tasks T002-T005 are parallelizable
- Foundational tasks T006-T014 marked [P] can be executed in parallel by different engineers
- Within US1, frontend components (T019-T021) are parallelizable with backend implementation once foundational APIs are ready
- Tests marked [P] can run in parallel in CI

## Implementation Strategy

### MVP First

1. Complete Phase 1 (Setup)
2. Complete Phase 2 (Foundational)
3. Implement Phase 3 (US1) end-to-end and validate (MVP)
4. Stop and validate US1 independently; then proceed to US2 and US3

### Incremental Delivery

- Deliver US1 as the MVP slice; deploy and validate metrics
- Add US2 and US3 in subsequent iterations

---

## Task Summary

- Total tasks: 35
- Tasks per story: US1: 9 (T015-T023), US2: 4 (T024-T027), US3: 3 (T028-T030)
- Parallel opportunities: Many foundational and frontend tasks marked [P]

---

## Notes

- All tasks include file paths for direct implementation references.
- Tests are required by constitution and must be run in CI on PRs.
- Persistence is session-only (see spec.md) — any change to persist stories requires a separate feature and constitution re-check.
