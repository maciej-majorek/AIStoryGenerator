<!--
Sync Impact Report

Version change: none -> 1.0.0
Modified principles: Placeholder template -> Quality, Testing, UX Consistency, Performance
Added sections: Technology Stack & Constraints
Removed sections: none
Templates requiring updates: ✅ .specify/templates/plan-template.md
												 ✅ .specify/templates/spec-template.md
												 ✅ .specify/templates/tasks-template.md
Follow-up TODOs: RATIFICATION_DATE must be provided (TODO)
-->

# AIStoryGenerator Constitution

## Core Principles

### 1. Code Quality (NON-NEGOTIABLE)
All production code MUST be clear, maintainable, and reviewable. Code quality requirements:
- **MUST** pass static analysis and linting rules defined in the repository.
- **MUST** include clear public API documentation and usage examples for libraries/components.
- **MUST** adhere to established naming, layering, and dependency rules to avoid cyclic or tightly-coupled modules.
- **Rationale**: Maintainability scales with clarity; enforcing CI checks prevents entropy.

### 2. Testing Standards (NON-NEGOTIABLE)
Testing is mandatory and test-first where practical. Testing requirements:
- **MUST** include unit tests that cover critical logic with a target minimum coverage defined per module (coverage targets set by the team).
- **MUST** include integration tests for cross-component boundaries: backend↔frontend, API contracts, and persistence.
- **MUST** include end-to-end tests for primary user journeys described in specs.
- **MUST** run all tests in CI on every PR and block merges on failing tests.
- **Rationale**: Automated tests are the primary guardrail for regressions and enable safe refactoring.

### 3. User Experience Consistency (SHOULD)
UX consistency across React frontends and any UI surfaces is required:
- **SHOULD** use the shared component library and design tokens for layout, typography, and interaction patterns.
- **MUST** document user-facing behavior and accessibility expectations in the spec for each feature (contrast, keyboard navigation, screen-reader labels).
- **Rationale**: Consistent UX reduces user confusion and support burden; accessibility is a baseline requirement.

### 4. Performance & Resource Constraints (MUST)
Performance expectations must be explicit and measurable:
- **MUST** define performance goals in the plan (e.g., p95 latency, memory footprint, throughput) for any feature that has SLA expectations.
- **MUST** include performance tests or benchmarks for critical paths before merge.
- **MUST** set and enforce resource budgets for client bundles (React) and server components (.NET) where applicable.
- **Rationale**: Measurable targets protect user experience and platform costs.

## Technology Stack & Constraints
- **Backend**: .NET 10.0 (target runtime for server components and shared libraries).
- **Frontend**: React (latest stable) with a shared component library for UI consistency.
- **Builds**: CI pipelines MUST produce reproducible builds for both backend and frontend.
- **Runtime Constraints**: Target deployment platforms and required OS/hosting specifics MUST be listed in the implementation plan.

## Development Workflow & Quality Gates
- All changes MUST be delivered via feature branches and pull requests.
- PRs **MUST** include: a short description, linked spec/plan, tests demonstrating the change, and evidence of passing CI checks.
- Code review: at least one approving review from a team member other than the author; for high-risk or architectural changes two reviewers are recommended.
- Gating: The following gates are enforced before merge:
	- Linting and static analysis: pass
	- Unit & integration tests: pass
	- Performance benchmarks (when applicable): within target
	- Accessibility checklist (for UI changes): pass

## Governance
- Amendments to this constitution **MUST** be proposed via a documented PR referencing the reason and migration plan.
- Approval for amendments: majority approval from active maintainers; material governance changes (principle removals/rewrites) require an explicit maintainer quorum.
- Versioning policy: semantic versioning for the constitution itself. Bumps:
	- MAJOR: Backward-incompatible governance or principle removals.
	- MINOR: New principle or materially expanded guidance.
	- PATCH: Clarifications, typos, non-semantic refinements.
- Compliance: All feature plans **MUST** include a constitution check section documenting how the feature meets principles; CI or PR templates **MUST** reference this requirement.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): provide original ratification date | **Last Amended**: 2026-05-21

