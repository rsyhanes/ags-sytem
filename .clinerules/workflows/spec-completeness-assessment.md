# Spec Completeness Assessment Workflow

**Purpose:** Analyze implementation completeness against spec requirements, producing cross-layer dependency graphs with completion indicators.

**Input:**
- `spec_id` - Specification identifier (e.g., `items.manage-catalog-item.v1`)

**Output:**
- ASCII dependency graph showing spec-to-implementation mapping across all architecture layers
- Completion status with ✅ (implemented) or ❓ (missing/incomplete) indicators

---

## Workflow Execution Steps

### 1. Spec Validation & Loading
- Validate spec_id format matches `context.feature.v1` pattern
- Locate spec file at `arc/specs/{spec_id}.spec.yaml`
- Parse YAML structure and extract deliverables/scenarios

### 2. Architecture Layer Analysis

#### PRESENTATION LAYER (HTTP API)
**Check:**
- Endpoint routes match spec's `interactions.inbound.api`
- Request/Response contracts align with OpenAPI specs
- Controller actions exist for each CRUD operation

**Criteria:**
- ✅ Endpoints implemented and routable
- ❓ Missing endpoints or incorrect signatures

#### APPLICATION LAYER (Use Cases)
**Check:**
- Command/Query handlers implemented
- MediatR orchestration pattern followed
- Domain logic delegation through ports
- Domain events published correctly

**Criteria:**
- ✅ Handlers exist and follow CQRS pattern
- ✅ Dependency injection of ports/publishers
- ✅ Error handling with BusinessRuleViolationException

#### DOMAIN LAYER (Business Logic)
**Check:**
- Entities match spec concept definitions
- Business rules enforced (validation, invariants)
- Domain events defined as records
- Ports defined as interfaces

**Criteria:**
- ✅ Entities are immutable (record types)
- ✅ Business logic contains no infrastructure dependencies
- ✅ Ports define clean contracts

#### INFRASTRUCTURE LAYER (Adapters)
**Check:**
- Port implementations exist
- Persistence adapters follow contract
- Event dispatching infrastructure available
- Real database connections configured

**Criteria:**
- ✅ Mock implementations for testing
- ❓ Real database adapters (marked incomplete for mocks)
- ❓ Event subscribers (infrastructure exists but may not be connected)

#### TESTING LAYER (Validation)
**Check:**
- Unit tests cover all spec scenarios
- Test utilities and builders available
- Scenarios map 1:1 to test methods
- Mock infrastructure in place

**Criteria:**
- ✅ All spec scenarios have corresponding tests
- ✅ Test coverage includes happy paths + error cases
- ✅ Test data builders provide consistent fixtures

### 3. Contract Verification
- Verify referenced contracts exist in `arc/contracts/`
- Check OpenAPI, AsyncAPI, and JSON schema compliance
- Validate internal command/query schemas

### 4. Completion Scoring
**Per Layer:**
- Calculate completion percentage based on deliverables vs implementation
- ✅ 100% = All deliverables implemented and tested
- 🟡 50-99% = Core functionality complete, minor gaps
- ❓ 0-49% = Significant implementation missing

**Overall Score:**
- Weighted average across layers
- Presentation/APPLICATION/DOMAIN = critical (required)
- Infrastructure/Testing = supporting (can be mocked)

---

## Visualization Template

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                SPEC: {spec_id}                                   │
│                     "{spec.objective}"                                           │
└─────────────────────┬──────────────────────────────────────────────────────────────┘
                      │                                                ┌─────────────┐
                      │ Dependencies flow                           ┌─►│ CONTRACTS    │
                      │ downward (no circular)                      │  └─────────────┘
                      │                                            │
                      │                                            │ Existing: {contracts}
┌─────────────────────┼───────────────────────────────────────────────────────────────┐
│                     │                                                               │
│  PRESENTATION      │ {presentation_status}                                │
│  (HTTP API)        │                                                               │
│                     │ {presentation_details}                                        │
├─────────────────────┼───────────────────────────────────────────────────────────────┤
│  APPLICATION       │ {application_status}                                │
│  (Use Cases)       │                                                               │
│                     │ {application_operations}                                     │
└─────────────────────┼───────────────────────────────────────────────────────────────┘
                      │                                                Domain events:
                      │                                                {events_published}
                      ▼
┌─────────────────────┼───────────────────────────────────────────────────────────────┐
│                     │                                                               │
│  DOMAIN            │ {domain_status}                                     │
│  (Business Logic)  │                                                               │
│                     │ Entities: {entities}                                          │
│                     │ Value Objects: {value_objects}                               │
│                     │ Events: {domain_events}                                      │
│                     │ Ports: {ports}                                               │
└─────────────────────┼───────────────────────────────────────────────────────────────┘
                        Infrastructure mocking
                                ▼
┌─────────────────────┼───────────────────────────────────────────────────────────────┐
│                     │                                                               │
│  INFRASTRUCTURE     │ {infrastructure_status}                            │
│  (Adapters)         │                                                               │
│                     │ Mocks: {mock_implementations}                               │
│                     │ Real DB: {database_status}                                  │
│                     │ Events: {event_dispatching}                                │
└─────────────────────┼───────────────────────────────────────────────────────────────┘
                      │
                      ▼ Testing validation
┌─────────────────────┼───────────────────────────────────────────────────────────────┐
│                     │                                                               │
│  TESTING           │ {testing_status}                                    │
│  (Scenarios)       │                                                               │
│                     │ Coverage: {test_coverage}                                     │
│                     │ Cases: {test_cases_list}                                     │
└─────────────────────┼───────────────────────────────────────────────────────────────┘
                      │
                      ▼ Final assessment
┌─────────────────────┼───────────────────────────────────────────────────────────────┘
                      │
                   🎯 OVERALL COMPLETENESS: {overall_percentage}%
                      │
                      {completion_summary}

```

## Usage Examples

**Manual Agent Execution:**
- When implementing a spec, agents should follow this workflow after initial development
- Replace `{variable}` placeholders with actual analysis results from codebase inspection
- Determine layer status by checking implementation against workflow criteria
- Generate the ASCII visualization showing completion status
- Use output to identify remaining gaps before marking implementation complete

**Integration Points:**
- Run after implementing domain, application, and basic infrastructure layers
- Validate contracts, scenarios, and deliverables before considering spec "done"
- Feed results back into development planning (real persistence, event subscribers, etc.)

## Agent Guidelines

**When to Execute:**
- After initial spec implementation to assess completeness
- Before marking pull requests or features as production-ready
- During architectural reviews of spec implementations

**Expected Output:**
- Visual completion assessment showing spec-to-code alignment
- Clear identification of missing elements (database adapters, event wiring, etc.)
- Actionable feedback for completing the implementation
