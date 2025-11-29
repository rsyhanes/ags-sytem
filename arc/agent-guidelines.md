# 🤖 Agent Code Generation Guidelines — Domain First

These guidelines ensure that all AI agents generate consistent, domain-aligned, architecture-safe code across the entire monorepo. Agents should follow this file whenever interpreting specs under `/arc/specs`.

This guide reflects the **domain-first specification model**, where the domain is the root of all behavior, and adapters exist only as projections of domain use cases.

---

# 🧩 1. Read Specs in Strict Order (Domain → Interactions → Contracts → Scenarios → Deliverables)

Agents must process every spec in the following order:

1. **`domain`** — Always the starting point.
2. **`interactions`** — How the environment triggers the domain use case.
3. **`contracts`** — The ground-truth schemas for inputs/outputs.
4. **`scenarios`** — Behavioral validation.
5. **`deliverables`** — What code must be generated.
6. **`packs`** — Which rules apply.

Never derive behavior from UI or API first. The **domain drives everything**.

---

# 🌱 2. Domain First (Primary Source of Truth)

Agents must start with the `domain` section.

### The domain section defines:
- **the problem** to solve
- **why** it exists (business rationale)
- **concepts**: entities, value objects, domain events
- **rules**: must/cannot
- **the use case**: the core domain behavior

### Agent responsibilities:
- Generate domain entities and value objects.
- Implement rules as invariants or guards.
- Implement the use case as a **pure domain behavior**.
- Domain code must contain **no framework**, **no persistence**, **no HTTP**, and **no UI logic**.

---

# 🔗 3. Interactions (Ports → Domain → Ports)

After establishing domain behavior, agents then interpret the `interactions` section.

### Inbound interactions drive the use case:
- UI state changes
- API requests
- incoming integration events

### Outbound interactions reflect outcomes:
- domain events
- API responses
- persistence operations
- external provider calls

Agents must:
- generate **thin adapters** that map inbound contracts → domain use case
- generate **thin outbound adapters** from domain outcome → outbound contract
- keep all logic inside the domain or use case service

Adapters are strictly I/O boundaries.

---

# 📄 4. Contracts (Schemas Reference Layer)

Agents must treat contracts as **immutable, authoritative structures**.

### Contract categories:
- **domain** → canonical shapes
- **inbound** → what triggers the use case
- **outbound** → what the use case produces for the outside world
- **ui_state** → client-side state derived from requests/responses

### Agent responsibilities:
- Ensure all request/response mapping follows schemas *exactly*.
- Never infer or add fields not defined by contracts.
- Keep domain entities separate from persistence models.
- Keep UI state separate from API request/response shapes.

---

# 🧪 5. Scenarios (Behavioral Backbone)

Each scenario defines:
- the initial condition
- the trigger
- the domain behavior
- the observed outbound effects

Agents must:
- implement scenario-based tests
- validate both domain state change **and** outbound effects
- ensure all branches of domain logic are covered

---

# 🎨 6. UI State Model Generation

UI state models derive from contracts and domain outcomes.

Agents must:
- derive `input` fields from **inbound request schemas**
- derive `result` fields from **domain schemas**
- ensure presence of status flags (`loading`, `error`, `success`)
- ensure UI state is framework-agnostic (not tied to React/Gatsby/Vue)

UI state models represent **client-side feature state**, not forms.

---

# 🏗️ 7. Deliverables (Required Generation Targets)

Every spec lists the exact deliverables agents must produce:

- domain logic
- application service (use case orchestrator)
- inbound adapters (UI/API/events)
- outbound adapters (db/events/external)
- UI state model
- tests following scenarios

Agents must generate all deliverables unless explicitly not applicable.

---

# 📦 8. Rule Packs

Agents must enforce rule packs listed under `packs`.

Examples:
- `spec.linting` → enforce spec shape correctness
- `domain.purity` → no framework contamination inside domain
- `ui.state` → enforce UI state derivation
- `ports.contracts` → ensure adapters match contracts exactly

Agents should treat rule packs as **hard constraints**.

---

# 🔒 9. Architecture Purity Rules

Agents must:
- isolate domain from frameworks
- generate ports and adapters explicitly
- use contracts instead of implicit shapes
- strictly separate domain models, DTOs, persistence models, and UI state models
- ensure domain never knows about the outside world
- ensure adapters never contain domain logic

This is required for correctness.

---

# 🧭 10. Final Agent Checklist

Before generating any code, an agent must confirm:

- [ ] Domain problem, rationale, rules, and use_case fully understood
- [ ] Domain entities/value objects designed
- [ ] Domain use case implemented in pure domain style
- [ ] Inbound adapters map requests → domain
- [ ] Outbound adapters map domain → responses/events/persistence
- [ ] UI state derived using request/response schemas
- [ ] All contracts referenced correctly
- [ ] All scenarios covered in tests
- [ ] All deliverables produced
- [ ] All rule packs satisfied

Agents that follow this checklist will consistently produce correct, traceable, domain-aligned implementations.