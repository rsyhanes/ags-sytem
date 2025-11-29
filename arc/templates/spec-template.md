# 📐 Domain‑First Spec Template (v1)

This template reflects a **domain‑centric** architecture: the domain problem is the root, interactions are branches, and adapters/ports are leaves. Use this whenever defining a new feature slice.

---

## 🆔 Metadata
```yaml
spec:
  id: <context.feature.v1>
  context: <bounded-context>
  version: 1
```

---

## 🌱 1. Domain (Root of the Spec)
```yaml
  domain:
    problem: |
      <What domain problem does this feature solve?>

    rationale: |
      <Why does this behavior exist? Business value, motivation>

    concepts:
      entities:
        - <EntityName>
      value_objects:
        - <ValueObjectName>
      domain_events:
        - <EventName>

    rules:
      must:
        - <Domain rule>
      cannot:
        - <Prohibition>

    use_case:
      name: <UseCaseName>
      trigger: <Domain-level trigger>
      description: |
        <High-level narrative of the use case within the domain>

      steps:
        - <Domain step>
        - <Domain step>
```

---

## 🔗 2. Interactions (Ports → Domain)
```yaml
  interactions:
    inbound:
      ui:
        - <route or view state that triggers the use case>
      api:
        - <HTTP or GraphQL endpoint>
      events:
        - <Incoming integration events>

    outbound:
      api_responses:
        - <Response shapes>
      events:
        - <Domain events emitted>
      persistence:
        - <Write or read models touched>
      external_calls:
        - <Outbound requests to other systems>
```

---

## 📄 3. Contracts (Reference Truths)
```yaml
  contracts:
    domain:
      - "../contracts/schemas/domain/<entity>.v1.json"

    inbound:
      - "../contracts/openapi/<feature>.v1.yaml#/paths/..."
      - "../contracts/schemas/ui/<ui-state>.v1.json"

    outbound:
      - "../contracts/schemas/domain/<entity>.v1.json"
      - "../contracts/asyncapi/<events>.v1.yaml#/channels/..."
      - "../contracts/schemas/persistence/<record>.v1.json"

    ui_state:
      derives_from:
        request: "../contracts/openapi/..."
        response: "../contracts/schemas/domain/..."
      template:
        include_status: true
```

---

## 🧪 4. Scenarios (Given/When/Then)
```yaml
  scenarios:
    - name: <scenario name>
      given: <context>
      when: <action>
      then:
        - <expected domain change>
        - <expected outbound interaction>
```

---

## 🔐 5. NFR (Non‑Functional Requirements)
```yaml
  nfr:
    performance:
      - <rule>
    reliability:
      - <rule>
    security:
      - <rule>
```

---

## 📦 6. Deliverables (What Agents Must Generate)
```yaml
  deliverables:
    - domain logic
    - application service (use case implementation)
    - inbound adapters (ui/api/event)
    - outbound adapters (db/events/external)
    - ui state model
    - tests (scenario-aligned)
```

---

## 🧩 7. Packs (Rule Packs to Apply)
```yaml
  packs:
    - "spec.linting"
    - "domain.purity"
    - "ui.state"
    - "ports.contracts"
```

---

## 📝 Notes
- Always fill in **domain** first.
- Interactions should be **thin descriptions**, the domain drives behavior.
- Contracts reference the ground truth files.
- Keep `deliverables` explicit so the coding agent knows what to generate.

