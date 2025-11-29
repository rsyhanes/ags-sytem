# 🧭 Contracts Index

Domain contracts are the primary source of truth.
All other contracts (API, UI, persistence, events) must align to the domain.

## 🚀 Getting Started
This folder contains all **contracts** your system uses across every layer (Domain, Application, API, UI, Persistence, Events, External APIs). These contracts act as the **source of truth** for data structures and message formats.

When adding a new feature or integration:
1. **Identify the layer** (Domain, DTO, API, Event, Persistence, External, UI).
2. **Create a new versioned schema** in the appropriate directory.
3. **Reference the contract** from your spec under `contracts:` or `internal_contracts:`.
4. **Keep contracts atomic**—one concept per file.
5. **Never duplicate shapes**—reuse via references.

---


This document serves as a quick reference for both humans and AI agents to understand the structure, purpose, and spec reference points for all contract types within the `/arc/contracts` folder.

Each subfolder contains contracts that represent a specific interaction layer of the system — from core domain entities to external integrations and UI bindings.

---

## 📂 Folder Summary

### 📁 Directory Layout (Updated)
```
/arc/contracts
│
├── openapi/                # Public API surface (driving port)
│   └── *.yaml
│
├── asyncapi/               # Event contracts (driven port)
│   └── *.yaml
│
├── schemas/
│   ├── domain/             # Canonical domain models
│   ├── persistence/        # Database-bound models
│   ├── external/           # 3rd-party provider models
│   └── ui/                 # UI → API state models & state models
│
└── internal/               # 🔥 Internal application contracts
    ├── commands/           # Input to use cases
    ├── results/            # Output from use cases
    ├── queries/            # Query DTOs
    └── events/             # Internal (non-asyncapi) events
```

The `internal` folder is reserved strictly for **application-layer messages** that never cross a system boundary.



| Layer | Example | Directory | Spec Reference | Purpose |
|--------|----------|------------|----------------|----------|
| **Domain** | `user.v1.json` | `/arc/contracts/schemas/domain/` | `contracts.schemas` | Canonical business entities and value objects that define the semantic truth of the domain. |
| **Application (DTO)** | `register-user-command.v1.json` | `/arc/contracts/schemas/dto/` | `internal_contracts` | In-process messages such as commands, queries, and results used by the application layer. |
| **API (Driving Adapter)** | `users.v1.yaml` | `/arc/contracts/openapi/` | `contracts.openapi` | REST or GraphQL definitions that describe the external API surface exposed to clients. |
| **Async/Event** | `user-events.v1.yaml` | `/arc/contracts/asyncapi/` | `contracts.asyncapi` | Message-driven contracts that describe events the system publishes or subscribes to. |
| **Persistence** | `user-record.v1.json` | `/arc/contracts/schemas/persistence/` | `contracts.schemas` | Data models bound to the storage layer (e.g., database tables, collections, or records). |
| **External API** | `email-provider.v1.json` | `/arc/contracts/schemas/external/` | `contracts.schemas` | Request and response payload definitions for communication with third-party services. |
| **UI State Model** | `register-form.v1.json` | `/arc/contracts/schemas/ui/                 # UI state models (client-side feature state)). |

---

## 🧱 Conventions

- **File naming:** `feature-name.vX.json` or `feature-name.vX.yaml`  (versioned using `v1`, `v2`, etc.)
- **Schema versioning:** Each file is self-contained and versioned independently to ensure backward compatibility.
- **Referencing:** Specs reference contracts **relatively**, e.g.:
  ```yaml
  contracts:
    openapi: "../contracts/openapi/users.v1.yaml#/paths/~1api~1users~1register"
    schemas:
      - "../contracts/schemas/domain/user.v1.json"
  ```
- **Directory policy:**
  - All domain models and internal DTOs reside under `/arc/contracts/schemas/`.
  - API specifications (OpenAPI, AsyncAPI) reside at the top-level `/arc/contracts/openapi/` and `/arc/contracts/asyncapi/`.
  - Each schema describes a single logical entity or message; reuse by reference only.

---

### 📘 Example Reference from a Spec
```yaml
contracts:
  openapi: "../contracts/openapi/users.v1.yaml#/paths/~1api~1users~1register"
  asyncapi:
    - "../contracts/asyncapi/user-events.v1.yaml#/channels/user.created"
  schemas:
    - "../contracts/schemas/domain/user.v1.json"
    - "../contracts/schemas/persistence/user-record.v1.json"
    - "../contracts/schemas/external/email-provider.v1.json"
    - "../contracts/schemas/ui/register-form.v1.json"
```

---

### 💡 Tip for Spec Authors
When defining new features:
- Start with **Domain** contracts → define canonical entities.
- Add **API / Async** contracts → define inputs and outputs.
- Include **Persistence** and **External API** contracts only when necessary.
- Optionally define **UI** schemas if a form or component binds directly to this feature.

Each contract should serve as a **single source of truth** for structure and validation within that interaction layer.

