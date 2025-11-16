# agent-guidelines

You are working in a monorepo that uses:
- Spec-driven development
- Ports & Adapters (Hexagonal) architecture
- Domain-first design
- .NET for backend services and .NET Aspire for orchestrating the distributed app environment

This document tells you exactly how to use the structure in `/arc`.

---

## 1. Default Behavior

When asked to implement or modify something:

1. Start from `/arc/index.yaml`.
2. Load the target spec from `/arc/specs/*.spec.yaml`.
3. Respect the layout and conventions defined there.
4. Only touch files and folders that align with that structure.

If you need something that does not exist yet, create it following these rules.

---

## 2. Folder Layout (Ports & Adapters Lite)

Follow the structure at /arc/views/development and learn from that.

**Rules for dependencies:**

- `X.domain` → depends on nothing in `X.adapters`, `X.ports` or frameworks.
- `X.app` → depends on `X.domain` and `X.ports`, not on `X.adapters`.
- `X.ports` → no dependency on `X.adapters`.
- `X.adapters` → may depend on `X.domain`, `X.app`, and `X.ports`.

Do **not** introduce new top-level patterns instead of this structure.

---

## 3. Using Specs

Specs live in `/arc/specs` and are the primary input.

When implementing a spec:

1. Read the `spec`:
    - `objective`, `scope.in/out`
    - `scenarios` (Given/When/Then)
    - `packs` (rule packs by ID)
    - `contracts` references (OpenAPI/AsyncAPI/JSON Schemas)
    - any `implementation.layout` hints

2. Implement exactly what the spec describes:
    - Add or update domain types in `src/<context>.domain`.
    - Add or update use-cases in `src/<context>.app`.
    - Define or extend ports in `src/<context>.ports`.
    - Implement adapters in `src/<context>.adapters`.

3. For each scenario in the spec, ensure there is a corresponding test.

If something is ambiguous:
- Prefer extending the spec (or proposing an update) over inventing ad-hoc behavior.

---

## 4. Using Contracts

Contracts live in `/arc/contracts`:

- HTTP APIs: `/arc/contracts/openapi/*.yaml`
- Messaging: `/arc/contracts/asyncapi/*.yaml`
- Shared schemas: `/arc/contracts/schemas/**`

When creating or updating adapters:

- Driving ports (e.g. HTTP endpoints) MUST conform to the referenced OpenAPI.
- Messaging ports MUST conform to the AsyncAPI/message schemas.
- If a spec references a contract ID, use **that** as the single source of truth.
- If a new port is introduced, add or reference an appropriate schema/contract.

Do **not** create APIs or message shapes that contradict their contracts.

---

## 5. Using Rules & Packs

Rule packs live in `/arc/rules/packs`.

Specs reference rules by **ID**, for example:

- `domain.purity`
- `ports.contracts`
- `adapters.conformance`
- `web-api`
- `messaging`
- `platform.aspire`

Your responsibilities:

- When a spec references a rule pack, follow its intent in your implementation.
- Do not duplicate rule text in specs or code; always reference by ID.
- If you cannot satisfy a MUST rule, propose a **waiver** in the spec with:
    - rule ID
    - reason
    - owner
    - expiry
    - mitigation

---

## 6. Tests & Scenarios

For each spec:

- Every key `scenario` should be backed by:
    - domain tests (for core invariants),
    - contract tests (for HTTP/messaging),
    - adapter/port conformance tests where relevant,
    - and optionally e2e tests for cross-component flows.

When you add or modify behavior:

- Prefer adding/updating tests that map clearly to scenarios.
- Keep names aligned:
    - Scenario: `register-happy-path`
    - Test: `RegisterUser_HappyPath` or equivalent.

---

## 7. Views & Ontology

- `/arc/views` contains 4+1 view markdowns and diagrams.
    - They are for orientation only.
    - If they conflict with specs or contracts, the specs/contracts win.

- `/arc/ontology` (if present) contains a machine-readable graph.
    - Use it for navigation and impact analysis.
    - Do not treat it as a separate source of truth; it reflects specs, rules, and contracts.

---

## 8. Things You MUST NOT Do

- Do **not**:
    - invent new top-level folder structures without updating `/arc/index.yaml`.
    - bypass `*.domain` by putting core business logic directly in adapters.
    - define new rules inline in specs; always extend rule packs instead.
    - change contracts without aligning the corresponding specs and tests.
    - ignore referenced rule packs when a spec includes them.

If you need a new pattern:
- Propose it by updating:
    - `/arc/index.yaml`
    - this file
    - and, if appropriate, adding/adjusting a rule pack.

---

Follow this and you’ll generate code and docs that fit naturally into the system instead of fighting it.
