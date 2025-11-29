# 📘 Specs Index — Domain First

This index defines how feature specifications are structured and interpreted across the system. Every feature spec in `/arc/specs` follows a **domain-first architecture**: the domain problem is the root, and interactions, contracts, scenarios, and deliverables grow outward from it.

This ensures clarity, cohesion, and architectural alignment between teams, tools, and AI agents.

---

## 🧭 Purpose of Specs

A spec describes a **feature slice** beginning from the domain’s perspective:
1. **What domain problem are we solving?**
2. **Why must the system support this behavior?**
3. **What domain rules constrain it?**
4. **Which domain use case embodies the behavior?**
5. **How does the outside world interact with the use case?**
6. **What contracts define inputs and outputs?**
7. **What scenarios validate correct behavior?**
8. **What deliverables should the agent generate?**

Specs are the primary driver of implementation, automation, testing, and cross-team alignment.

---

## 📂 Directory Structure

All spec files must follow this naming pattern:
```
/arc/specs/<context>.<feature>.v1.spec.yaml
```

Example:
```
arc/specs/users.register-user.v1.spec.yaml
```

Each spec uses the following top-level order:

1. `domain`
2. `interactions`
3. `contracts`
4. `scenarios`
5. `nfr`
6. `deliverables`
7. `packs`

This order is mandatory for readability and machine interpretation.

---

## 🌱 1. Domain (Required)

The domain section expresses the core intent of the feature.
It must include:

- **problem** — What domain issue or capability is needed.
- **rationale** — Why this is important to the business.
- **concepts** — Entities, value objects, and domain events.
- **rules** — Invariants, constraints, and prohibitions.
- **use_case** — The domain-level behavior.

Example:
```yaml
domain:
  problem: "Users need to register using an email and password."
  rationale: "Provide account-level security and personalization."
  concepts:
    entities: [User]
    value_objects: [Email]
    domain_events: [UserRegistered]
  rules:
    must:
      - "Email must be unique."
    cannot:
      - "User cannot be created without a valid email."
  use_case:
    name: RegisterUser
    trigger: "A new account is requested."
    description: "Creates a new User aggregate from provided credentials."
    steps:
      - Validate email
      - Ensure no existing user
      - Create user entity
      - Emit UserRegistered event
```

---

## 🔗 2. Interactions

Defines how the environment interacts with the domain use case.

```yaml
interactions:
  inbound:
    ui:        # UI routes or state triggers
    api:       # API endpoints
    events:    # Incoming system or integration events
  outbound:
    api_responses:   # What API responses expose
    events:          # Domain/integration events emitted
    persistence:     # Write/read interactions with data stores
    external_calls:  # Requests to other systems
```

Interactions describe how the domain is driven and how results flow outward.

---

## 📄 3. Contracts

Contracts are references to schemas that define the structure of:

- domain shapes
- inbound inputs (UI/API)
- outbound outputs (responses, events, persistence)
- UI state model derivation

The contract sections are:
```yaml
contracts:
  domain:
    - "../contracts/schemas/domain/<entity>.v1.json"
  inbound:
    - "../contracts/openapi/<feature>.v1.yaml#/paths/..."
  outbound:
    - "../contracts/schemas/domain/..."
    - "../contracts/asyncapi/..."
    - "../contracts/schemas/persistence/..."
  ui_state:
    derives_from:
      request: "../contracts/openapi/..."
      response: "../contracts/schemas/domain/..."
    template:
      include_status: true
```

UI state is **explicitly derived** from inbound requests and domain responses.

---

## 🧪 4. Scenarios

Scenarios validate that the implementation matches domain intent.

```yaml
scenarios:
  - name: "successful registration"
    given: "email not registered"
    when: "valid registration submitted"
    then:
      - "user entity created"
      - "UserRegistered event emitted"
      - "returns 201 Created"
```

Each scenario must:
- trigger the domain use case
- validate domain state change
- validate outbound effects

---

## 🛡️ 5. Non-Functional Requirements (NFR)

Optional but recommended. Examples:
```yaml
nfr:
  performance:
    - "respond within 200ms"
  reliability:
    - "retry external provider on transient failure"
  security:
    - "password never logged or persisted unhashed"
```

---

## 📦 6. Deliverables

What the agent must generate:
```yaml
deliverables:
  - domain logic
  - application service
  - inbound adapters
  - outbound adapters
  - ui state model
  - tests (scenario-aligned)
```

Deliverables ensure the expectation is explicit.

---

## ⚙️ 7. Packs

Each spec must include rule packs that enforce architectural correctness.
```yaml
packs:
  - "spec.linting"
  - "domain.purity"
  - "ui.state"
  - "ports.contracts"
```

---

## 🎯 Summary

- The **domain** is always the root section.
- Interactions and contracts describe how the world connects to the domain.
- Scenarios validate correctness.
- Deliverables guide agent output.
- Rule packs enforce proper structure and governance.

This approach ensures every feature is consistent, traceable, domain-driven, and agent-friendly.