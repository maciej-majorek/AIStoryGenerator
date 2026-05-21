# Research: AI Story Generator (decisions and rationale)

Decision: Session-only persistence
Rationale: Minimizes backend complexity for MVP, avoids immediate auth and storage requirements, accelerates time-to-value. Alternatives considered: persistent accounts (higher complexity) and hybrid (opt-in) — deferred.

Decision: Technology stack — .NET 10.0 backend, React frontend
Rationale: Team standard and constitution alignment. .NET offers robust HTTP hosting and testing frameworks; React enables component-driven UI and existing design token approaches.

Decision: Testing tools
Rationale: Use xUnit for backend unit tests; Playwright for end-to-end browser tests; React Testing Library/Jest for component tests. These choices balance reliability, cross-platform support, and integration with CI.

Decision: Performance targets
Rationale: From spec success criteria: median generation <8s; p95 <20s under staging load. These will be refined once a generation provider and capacity are known.

Decision: Safety & moderation
Rationale: The plan requires safety gating: if AI returns flagged content, the backend must surface safe-fail and optionally attempt a sanitized regeneration. Implement policy checks before displaying content.

Alternatives considered summary:
- Using a hosted low-level model vs external API: external API reduces hosting complexity but increases cost and latency variability.
- Persisted storage: adds privacy and retention requirements.
